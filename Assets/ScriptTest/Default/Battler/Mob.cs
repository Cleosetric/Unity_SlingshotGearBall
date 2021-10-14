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

    [Space]
    [Header("Mob Movement")]
    public LayerMask actorLayer;
    public float radiusSight;
    public float attackSight;
    public float moveSpeed;
    private Vector2 lastVel;

    [Space]
    [Header("Mob Attack")]
    public float attackTime = 3f;
    public float attackRate = 1f;
    private float nextAttackTime = 0;
    private Transform target;
    private LineRenderer attackLine;
    
    //shake
    private float time = 0.2f;
    private float delayShake = 0f;

    //Movement
    private Vector2 baseStartPoint;
    private Vector2 destinationPoint;
    private Vector2 startPoint;
    private float moveProgress = 0.0f;

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

        attackLine = GetComponent<LineRenderer>();
        initialPost.position = transform.position;

        startPoint = transform.localPosition;
        baseStartPoint = transform.localPosition;
        moveProgress = 0.0f;
        PickNewRandomDestination();
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

    public virtual void Move(){
        CheckTargetsInSight(radiusSight);
        if(target != null){
            CheckAttackDistance(attackSight);
        }else{
            float distance = Vector2.Distance(initialPost.position, transform.position);
            if(distance >= 0){
                MoveToward(initialPost);
            }else{
                // MoveRandom();
            }
        }
    }

    private void CheckAttackDistance(float attackSight)
    {
        Vector2 direction = (target.position - transform.position).normalized * attackSight;
        Debug.DrawRay(transform.position, direction, Color.green);

        float distance = Vector2.Distance(target.position, transform.position);
        if(distance <= attackSight){
            if(Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackTime / attackRate;
            }
        }else{
            MoveToward(target);
        }
    }

    private void CheckTargetsInSight(float range)
    {
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(transform.position, range, actorLayer);
        List<Transform> targets = new List<Transform>();
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Actors")){
                targets.Add(hitObj.transform);
            }
        }

        if(targets.Count > 0){
            targets.Sort(delegate(Transform t1, Transform t2){ 
                return Vector2.Distance(t1.position,transform.position).CompareTo(Vector2.Distance(t2.position, transform.position));
            });
            target = targets[0];
        }else{
            target = null;
        }
    }

    public virtual void Attack(){
        Debug.Log(name + " Attack "+target.name);
        rb.velocity = Vector2.zero;
        ShowAttackLine();
        Invoke("BasicAttack",0.5f);
    }

    private void ShowAttackLine()
    {
        attackLine.positionCount = 2;
        attackLine.useWorldSpace = true;
        attackLine.numCapVertices = 10;
        Vector3 [] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = Vector3.MoveTowards(transform.position, target.transform.position, attackSight);
        attackLine.SetPositions(points);
    }

    void BasicAttack(){
        if(target != null){
            Vector2 dir = (target.position - transform.position).normalized;
            rb.AddForce(dir * 20f, ForceMode2D.Impulse);
            Actor targetAtk = target.GetComponent<Actor>();
            targetAtk.ApplyDamage(this);
        }
        attackLine.positionCount = 0;
    }

    private void MoveToward(Transform selectedTarget)
    {
        Vector2 targetPos = new Vector2(selectedTarget.position.x, selectedTarget.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void MoveRandom()
    {
        bool reached = false;

        moveProgress += moveSpeed * Time.deltaTime;
        if (moveProgress >= 2.0f)
        {
            moveProgress = 2.0f;
            reached = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, destinationPoint, moveSpeed * Time.deltaTime);

        if (reached)
        {
            startPoint = destinationPoint;
            PickNewRandomDestination();
            moveProgress = 0.0f;
        }
    }

    void PickNewRandomDestination()
    {
        destinationPoint = Random.insideUnitCircle * radiusSight + baseStartPoint;
    }

    public override void Die()
    {
        Debug.Log(mobName + " Died");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Vector2 inNormal = other.contacts[0].normal;
        rb.velocity = Vector3.Reflect(lastVel, inNormal);
        rb.velocity += inNormal * 2.0f;
        PickNewRandomDestination();
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
