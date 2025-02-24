using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Charachter
{
    public delegate void OnActorStatChanged();
    public OnActorStatChanged onActorStatChanged;

    [Space]
    [Header("Actor Info")]
    public ActorStats charachter;
    public ActorClass actorClass;
    public string actorName;
    public string actorDesc;
    public Sprite actorSprite;
    public Sprite actorFace;
    public float curveStat;

    [Space]
    [Header("Actor Status")]
    public int currentLevel;
    public int maxLevel = 10;
    public int currentExp;
    public bool isAlive = true;
    public int[] nextLevelExp;

    [Space]
    [Header("Actor Equipment")]
    public List<Ability> abilities = new List<Ability>();

    [Space]
    [Header("Actor Equipment")]
    public Equipment[] equipment;

    [Space]
    [Header("Control Parameter")]
    public float actionSight;
    public int actionCost;
    private Coroutine regen;
    private Coroutine dash;

    [Space]
    [Header("Combat Info")]
    public LayerMask enemyLayers;
    public Transform directionFace;
    public GameObject bounceEffect;
    public Projectile projectile;

    public float projectileLife;
    private float attackRate;
    private float dashRate = 1;
    private float attackTime;
    private float dashTime = 1;
    private float nextAttackTime = 0;
    private float nextDashTime = 0;

    public int index;
    Vector2 lastVel;

    private void Awake()
    {
        LoadData();
    }

    private void LoadData() {
        //Setup Info
        actorName = charachter.name;
        actorDesc = charachter.description;
        actorClass = charachter.actorClass;
        actorSprite = charachter.actorSprite;
        actorFace = charachter.actorFace;
        curveStat = charachter.growthCurve;

        //Sprite
        charSprite.sprite = charachter.actorSprite;

        //Setup Stats
        statMHP.SetValue(charachter.statMHP.GetValue());
        statMSP.SetValue(charachter.statMSP.GetValue());
        statATK.SetValue(charachter.statATK.GetValue());
        statDEF.SetValue(charachter.statDEF.GetValue());
        statMATK.SetValue(charachter.statMATK.GetValue());
        statMDEF.SetValue(charachter.statMDEF.GetValue());
        statAGI.SetValue(charachter.statAGI.GetValue());
        statLUK.SetValue(charachter.statLUK.GetValue());
        statHRG.SetValue(charachter.statHRG.GetValue());
        statSRG.SetValue(charachter.statSRG.GetValue());

        statHIT.SetValue(charachter.statHIT.GetValue());
        statCRI.SetValue(charachter.statCRI.GetValue());
        statEVA.SetValue(charachter.statEVA.GetValue());
        statHRR.SetValue(charachter.statHRR.GetValue());
        statSRR.SetValue(charachter.statSRR.GetValue());

        currentHP = Mathf.RoundToInt(statMHP.GetValue());
        currentSP = Mathf.RoundToInt(statMSP.GetValue());

        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        equipment= new Equipment[numSlot];

        //Setup Control Param
        actionSight = charachter.actionSight;
        actionCost = charachter.actionCost;
        // nameText.SetText(actorName);
        isAlive = true;

        abilities = charachter.abilities;
        projectile = charachter.projectile;
        projectileLife = charachter.projectileLife;
        attackRate = charachter.attackRate;
        attackTime = charachter.attackTime;

        nextLevelExp = new int[maxLevel + 1];
        nextLevelExp[1] = 100;
        for (int i = 2; i < maxLevel; i++)
        {
            nextLevelExp[i] = Mathf.RoundToInt(nextLevelExp[i - 1] * 1.2f);
        }
        currentLevel = 1;

        Quaternion rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
        directionFace.rotation = rotation;
    }

    public void OnItemEquiped(Equipment newItem)
    {
        if(newItem != null ){
            statMHP.AddModifier(newItem.modMHP);
            statMSP.AddModifier(newItem.modMSP);
            statATK.AddModifier(newItem.modATK);
            statDEF.AddModifier(newItem.modDEF);
            statMATK.AddModifier(newItem.modMATK);
            statMDEF.AddModifier(newItem.modMDEF);
            statAGI.AddModifier(newItem.modAGI);
            statLUK.AddModifier(newItem.modLUK);
            statHRG.AddModifier(newItem.modHRG);
            statSRG.AddModifier(newItem.modSRG);

            statHIT.AddModifier(newItem.modHIT);
            statCRI.AddModifier(newItem.modCRI);
            statEVA.AddModifier(newItem.modEVA);
            statHRR.AddModifier(newItem.modHRR);
            statSRR.AddModifier(newItem.modSRR);
        }
        if(onActorStatChanged != null) onActorStatChanged.Invoke();
    }

    public void OnItemUnequiped(Equipment oldItem){
        
        if(oldItem != null){
            statMHP.RemoveModifier(oldItem.modMHP);
            statMSP.RemoveModifier(oldItem.modMSP);
            statATK.RemoveModifier(oldItem.modATK);
            statDEF.RemoveModifier(oldItem.modDEF);
            statMATK.RemoveModifier(oldItem.modMATK);
            statMDEF.RemoveModifier(oldItem.modMDEF);
            statAGI.RemoveModifier(oldItem.modAGI);
            statLUK.RemoveModifier(oldItem.modLUK);
            statHRG.RemoveModifier(oldItem.modHRG);
            statSRG.RemoveModifier(oldItem.modSRG);

            statHIT.RemoveModifier(oldItem.modHIT);
            statCRI.RemoveModifier(oldItem.modCRI);
            statEVA.RemoveModifier(oldItem.modEVA);
            statHRR.RemoveModifier(oldItem.modHRR);
            statSRR.RemoveModifier(oldItem.modSRR);
        }
        if(onActorStatChanged != null) onActorStatChanged.Invoke();
    }

    public override void ApplyDamage(Charachter user)
    {
        base.ApplyDamage(user);
        if(onActorHPChanged != null) onActorHPChanged.Invoke();
        if(gameObject.activeSelf) StartCoroutine(HitEffect());
        CameraManager.Instance.Shake(0.75f,2.5f);
    }

    public void ApplyAction(float energy)
    {
        StopRegen();
        if(currentSP >= energy){
            currentSP -= energy;
            if(currentSP <= 0) currentSP = 0;
            if(onActorHPChanged != null) onActorHPChanged.Invoke();
        }
    }

    public virtual void ActionMove(Vector2 force){
        Quaternion rotation = Quaternion.LookRotation(force, Vector3.up);
        directionFace.rotation = rotation;
        rb.velocity = statAGI.GetValue() * force;
        // rb.velocity = Vector2.zero;
        // rb.AddForce(force * statAGI.GetValue(), ForceMode2D.Impulse);
        // ApplyAction(actionCost);
    }

    public virtual void ActionDash(float dashDuration, float dashMultiplier){
        Debug.Log("Dash My Boy");
        if(Time.time >= nextDashTime)
        {
            SoundManager.Instance.Play("Dash");
            if(dash != null) StopCoroutine(dash);
            dash = StartCoroutine(Dash(dashDuration, dashMultiplier));
            nextDashTime = Time.time + dashTime / dashRate;
        }
           
        // if(isDash && gameObject.activeSelf){
            // Transform closestEnemy = EnemyManager.Instance.EnemyNearbyTransform(transform.position);
            // if (closestEnemy != null){
            //     float distance = Vector3.Distance(closestEnemy.position, transform.position); 
            //     float maxDistance = 3f;  
            //     if(distance < maxDistance){
            //         rb.velocity = Vector2.zero;         
            //         Vector3 direction = (closestEnemy.position - transform.position).normalized;
            //         rb.AddForce(direction * 20f, ForceMode2D.Impulse);
            //         // ApplyAction(actionCost);
            //         TimeManager.Instance.StartImpactMotion();
            //         isDash = false;
            //     }
                
            // }
        // }
    }

    public virtual void ActionAttack(GameObject target){
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Projectile projClone = Instantiate(projectile, transform.position, Quaternion.Euler(0,0,angle -90)) as Projectile;
        projClone.Initialize(this, 0, 10, 5, false, false, Quaternion.Euler(0,0,angle -90));
        Destroy(projClone.gameObject, projectileLife);

        // Mob targetAtk =  target.GetComponent<Mob>();
        // if(targetAtk != null) {
        //     // TimeManager.Instance.StartImpactMotion();
        //     Instantiate(hitEffect,target.transform.position,Quaternion.identity);
        //     targetAtk.ApplyDamage(this);
        // }
    }

    void CheckAttackDistance(){
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(transform.position, actionSight, enemyLayers);
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Enemy")){
                if(Time.time >= nextAttackTime)
                {
                    ActionAttack(hitObj.gameObject);
                    nextAttackTime = Time.time + attackTime / attackRate;
                }
            }
        }
    }

    private void Update() {
        CheckAttackDistance();
        DisplayHPBar();
    }

    private void FixedUpdate() {
        lastVel = rb.velocity;
    }

    public void Init(int index)
    {
        this.index = index;
    }

    public void MoveOnPath(Path path, float dist)
    {
        Vector2 prev = path.Head(-index);
        Vector2 next = path.Head(-index + 1);

        // Interpolate the position of the minion between the previous and the next point within the path. 'dist' is the distance between the 'head' of the path and the leader
        if(transform != null)
        transform.position = Vector2.Lerp(prev, next, dist);
    }

    public void Push(Vector2 dir)
    {
        transform.position += (Vector3)dir;
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

    public void GainExp(int experience){
        currentExp += experience;
        if(currentExp >= nextLevelExp[currentLevel] && currentLevel < maxLevel){
            LevelUp();
        }

        if(currentLevel >= maxLevel) currentExp = 0;
        if(onActorStatChanged != null) onActorStatChanged.Invoke();
        Debug.Log(actorName+" gainExp!");
    }

    private void LevelUp()
    {
        currentExp -= nextLevelExp[currentLevel];
        currentLevel++;
        IncreaseActorStat(curveStat);
    }

    private void IncreaseActorStat(float curve)
    {
        statMHP.SetValue(Mathf.RoundToInt(statMHP.GetValue() * curve));
        statMSP.SetValue(Mathf.RoundToInt(statMSP.GetValue() * curve));
        statATK.SetValue(Mathf.RoundToInt(statATK.GetValue() * curve));
        statDEF.SetValue(Mathf.RoundToInt(statDEF.GetValue() * curve));
        statMATK.SetValue(Mathf.RoundToInt(statMATK.GetValue() * curve));
        statMDEF.SetValue(Mathf.RoundToInt(statMDEF.GetValue() * curve));
        statAGI.SetValue(Mathf.RoundToInt(statAGI.GetValue() * curve));
        statLUK.SetValue(Mathf.RoundToInt(statLUK.GetValue() * curve));
        statHIT.SetValue(Mathf.RoundToInt(statHIT.GetValue() * curve));
        statCRI.SetValue(Mathf.RoundToInt(statCRI.GetValue() * curve));
        statEVA.SetValue(Mathf.RoundToInt(statEVA.GetValue() * curve));

        currentHP = Mathf.RoundToInt(statMHP.GetValue());
        currentSP = Mathf.RoundToInt(statMSP.GetValue());
    }

    public void StartRegen(){
        if(gameObject.activeSelf){
            if(regen != null) StopCoroutine(regen);
            regen = StartCoroutine(RegenerateStamina());
        }
    }

    public void StopRegen(){
        if(regen != null) StopCoroutine(regen);
    }

    private IEnumerator RegenerateStamina(){
        yield return new WaitForSeconds(statSRR.GetValue());

        while (currentSP < statMSP.GetValue())
        {
            currentSP += Mathf.RoundToInt(statSRG.GetValue());
            if(onActorHPChanged != null) onActorHPChanged.Invoke();
            yield return new WaitForSeconds(0.25f);
        }
        regen = null;
    }

    private IEnumerator Dash(float duration, float multiplier){
        // Vector2 normalSpeed = statAGI.GetValue() * (rb.velocity.normalized);
        float originalSpeed = statAGI.GetValue();
        float ellapsedTime = 0;
        while (ellapsedTime < duration)
        {
            statAGI.SetValue(originalSpeed * multiplier);
            // rb.velocity = normalSpeed * 2;
            ellapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        statAGI.SetValue(originalSpeed);
        dash = null;
    }

    public void Revive(){
        isAlive = true;
        currentHP = Mathf.RoundToInt(statMHP.GetValue());
        currentSP = Mathf.RoundToInt(statMSP.GetValue());

        Shader shaderSpritesDefault = Shader.Find("Sprites/Default");
        charSprite.material.shader = shaderSpritesDefault;
        charSprite.color = Color.white;

        gameObject.SetActive(true);
        if(onActorHPChanged != null) onActorHPChanged.Invoke();
    }

    public override void Die()
    {
        if(onActorHPChanged != null) onActorHPChanged.Invoke();

        isAlive = false;
        gameObject.SetActive(false);

        Party.Instance.partyOnActorChanged();
        Party.Instance.actors.Remove(this);

        if(Party.Instance.actors.Count > 0) Party.Instance.Refresh();
        
        //     // Party.Instance.SetLeader();
        // }

        Debug.Log(actorName + " Died");
        // Destroy(gameObject);
        // rb.velocity = Vector2.zero;
        // GetComponent<CircleCollider2D>().enabled = false;
    }

    private float calculateImpulse(Collision2D col){
        float impact = 0f;
        foreach (ContactPoint2D cp in col.contacts) {
                impact += cp.normalImpulse;
        }
        return impact;
    }

    IEnumerator HitEffect() {
        Shader shaderGUItext = Shader.Find("GUI/Text Shader");
        charSprite.material.shader = shaderGUItext;
        charSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        Shader shaderSpritesDefault = Shader.Find("Sprites/Default");
        charSprite.material.shader = shaderSpritesDefault;
        charSprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        SoundManager.Instance.Play("Hit");

        if(other.collider.tag == "Props"){
            Vector3 hitPoint = other.contacts[0].point;
            Instantiate(bounceEffect,hitPoint,Quaternion.identity);
            other.collider.GetComponent<Props>().ApplyDamage(1);
        }

        if(other.collider.tag == "Wall"){
            Vector3 hitPoint = other.contacts[0].point;
            Instantiate(bounceEffect,hitPoint,Quaternion.identity);
        }

        Vector2 inNormal = other.contacts[0].normal;
        Vector2 force = Vector3.Reflect(lastVel, inNormal);
        
        Quaternion rotation = Quaternion.LookRotation(force, Vector3.up);
        directionFace.rotation = rotation;

        rb.velocity = force;
        rb.velocity += inNormal * 2.0f;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, actionSight);
    }
}
