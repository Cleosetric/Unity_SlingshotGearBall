using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Charachter
{
    [Space]
    [Header("Mob Info")]
    public MobStats mob;
    public string mobName;
    public Transform initialPost;
    public GameObject deathEffectPrefab;

    [Space]
    [Header("Mob Movement")]
    public LayerMask actorLayer;
    public float radiusSight;
    public float attackSight;
    public float moveSpeed;
    private Vector2 lastVel;
    protected Transform target;
    
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

        moveSpeed = mob.statAGI.GetValue();
        radiusSight = mob.radiusSight;
        attackSight = mob.attackSight;
        charSprite.sprite = mob.mobSprite;
        nameText.SetText(mob.name);
    }

    private void Start() {
        currentHP = Mathf.RoundToInt(statMHP.GetValue());
        currentSP = Mathf.RoundToInt(statMSP.GetValue());
        initialPost.position = transform.position;
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

    public override void ApplyDamage(Charachter user)
    {
        base.ApplyDamage(user); 
        StartCoroutine(shakeObject());
        StartCoroutine(HitEffect());
    }

    public virtual void Move(){}

    public virtual void Attack(){}

    public override void Die()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<CircleCollider2D>().enabled = false;
        GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        ParticleSystem particle = effect.GetComponent<ParticleSystem>();
        effect.transform.SetParent(transform);
        Destroy(gameObject, particle.main.duration);
    }

    public virtual void OnCollisionEnter2D(Collision2D other) {
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
