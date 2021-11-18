using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mob : Charachter
{
    [Space]
    [Header("Mob Info")]
    public MobStats mob;
    public string mobName;

    [Space]
    [Header("Mob Drop")]
    public int mobScore;
    public int mobExp;
    [Range(1, 100)] public int mobCoinDrop;
    public List<EquipmentHolder> dropEquip = new List<EquipmentHolder>();
    [Range(1, 100)] public int equipDropChanche;

    [Space]
    [Header("Mob Movement")]
    public LayerMask actorLayer;
    public float radiusSight;
    public float attackSight;
    public float moveSpeed;
    private Vector2 lastVel;
    
    //shake
    private float time = 0.2f;
    private float delayShake = 0f;

    private void Awake() {
        mobName = mob.name;
        statMHP.SetValue(mob.statMHP.GetValue());
        statMSP.SetValue(mob.statMSP.GetValue());
        statATK.SetValue(mob.statATK.GetValue());
        statDEF.SetValue(mob.statDEF.GetValue());
        statMATK.SetValue(mob.statMATK.GetValue());
        statMDEF.SetValue(mob.statMDEF.GetValue());
        statAGI.SetValue(mob.statAGI.GetValue());
        statLUK.SetValue(mob.statLUK.GetValue());
        statHRG.SetValue(mob.statHRG.GetValue());
        statSRG.SetValue(mob.statSRG.GetValue());

        statHIT.SetValue(mob.statHIT.GetValue());
        statCRI.SetValue(mob.statCRI.GetValue());
        statEVA.SetValue(mob.statEVA.GetValue());
        statHRR.SetValue(mob.statHRR.GetValue());
        statSRR.SetValue(mob.statSRR.GetValue());

        currentHP = Mathf.RoundToInt(statMHP.GetValue());
        currentSP = Mathf.RoundToInt(statMSP.GetValue());

        mobScore = mob.score;
        mobExp = mob.experience;
        mobCoinDrop = mob.coin;
        dropEquip = mob.equip;
        equipDropChanche = mob.equipChance;

        moveSpeed = mob.statAGI.GetValue();
        radiusSight = mob.radiusSight;
        attackSight = mob.attackSight;
        charSprite.sprite = mob.mobSprite;
        nameText.SetText(mob.name);
    }

    protected virtual void Start() {
        currentHP = Mathf.RoundToInt(statMHP.GetValue());
        currentSP = Mathf.RoundToInt(statMSP.GetValue());
    }

    private void OnValidate()
    {
        if (delayShake > time) delayShake = time;
    }

    private void Update() {
        lastVel = rb.velocity;
        DisplayHPBar();
        Move();
    }

    private void DisplayHPBar()
    {
        hpBar.fillAmount = (float)currentHP / (float)statMHP.GetValue();
        if(hpEffect.fillAmount > hpBar.fillAmount){
            hpEffect.fillAmount -= 0.005f;
        }else{
            hpEffect.fillAmount = hpBar.fillAmount;
        }
    }

    void HideHPBar(){
        hpBar.transform.parent.gameObject.SetActive(false);
    }

    public override void ApplyDamage(Charachter user)
    {
        base.ApplyDamage(user); 
        StartCoroutine(shakeObject());
        StartCoroutine(HitEffect());
    }

    public virtual void Move(){
        if(GameManager.Instance.isGamePaused) return;
    }

    protected void MoveToward(Transform selectedTarget)
    {
        if(rb.bodyType == RigidbodyType2D.Static) return;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, selectedTarget.position, Time.deltaTime * moveSpeed);
        rb.MovePosition(newPosition);
    }

    protected Transform CheckTargetsInSight(float range)
    {
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(transform.position, range, actorLayer);
        hitBox.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
        if(hitBox.Length > 0){
            return hitBox[0].transform;
        }
        return null;
    }

    public override void Die()
    {
        SoundManager.Instance.Play("Explosion");
        HideHPBar();
        charSprite.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<CircleCollider2D>().enabled = false;

        GameManager.Instance.IncreaseTotalExp(mobExp);
        ScoreCounter.Instance.IncreaseScore(mobScore);
        TimeManager.Instance.StartImpactMotion();

        GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        ParticleSystem particle = effect.GetComponent<ParticleSystem>();
        effect.transform.SetParent(transform);

        SpawnPoolCoin(mobCoinDrop);
        SpawnMobDrop();

        Destroy(parent.gameObject, particle.main.duration);
    }
    

    private void SpawnMobDrop()
    {
        float randChance = UnityEngine.Random.Range(0, 100f);
        if (randChance <= equipDropChanche)
        {
            int dropIndex = UnityEngine.Random.Range(0, dropEquip.Count);
            Instantiate(dropEquip[dropIndex], transform.position, Quaternion.identity);
        }
    }

    void SpawnPoolCoin(int total)
    {
        for (int i = 0; i < total; i++)
        {
            GameObject coin = ObjectPooling.Instance.GetPooledObject("Coin");
            if(coin != null){
                coin.GetComponent<Coin>().CoinSpawn(transform.position);
            }
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other) {
        if(rb.bodyType == RigidbodyType2D.Static) return;
        Vector2 inNormal = other.contacts[0].normal;
        rb.velocity = Vector3.Reflect(lastVel, inNormal);
        rb.velocity += inNormal * 2.0f;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSight);
    }

    IEnumerator shakeObject(){
        float timer = 0f;
        Vector3 blockPos = charSprite.transform.localPosition;
        while (timer < 0.15f)
        {
            timer += Time.deltaTime;
    
            Vector2 randomPos = blockPos + (Random.insideUnitSphere * 0.075f);
            charSprite.transform.localPosition = randomPos;

            if (delayShake > 0f)
            {
                yield return new WaitForSeconds(delayShake);
            }
            else
            {
                yield return null;
            }
        }
 
       charSprite.transform.localPosition = blockPos;
   }

   IEnumerator HitEffect() {
        Shader shaderGUItext = Shader.Find("GUI/Text Shader");
        charSprite.material.shader = shaderGUItext;
        charSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        Shader shaderSpritesDefault = Shader.Find("Sprites/Default");
        charSprite.material.shader = shaderSpritesDefault;
        charSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }
}
