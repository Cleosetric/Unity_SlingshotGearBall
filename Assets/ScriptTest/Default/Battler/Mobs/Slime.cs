using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobState {
    Idle,
    Move,
    Attack,
}

public class Slime : Mob
{
    [Space]
    [Header("Mob Attack")]
    public Transform initialPost;
    private float attackTime;
    private float attackRate;
    private float nextAttackTime = 0;
    private MobState state;
    private LineRenderer attackLine;
    private Coroutine attackCharge;
    private bool isAttacking;
    private bool onAttack;
    private Transform target;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackTime = mob.attackTime;
        attackRate = mob.attackRate;

        attackLine = GetComponent<LineRenderer>();
        initialPost.position = transform.position;
        
        state = MobState.Idle;
    }

    public override void Move()
    {
       base.Move();

        switch (state)
        {
            case MobState.Idle:
                target = CheckTargetsInSight(radiusSight);
                if(target != null){
                    state = MobState.Move;
                }else{
                    MoveToward(initialPost);
                }
            break;
            case MobState.Move:
                // target = CheckTargetsInSight(radiusSight);
                if(target != null){
                    Vector2 direction = target.position - transform.position;
                    Vector2 dir_c =  new Vector2(transform.position.x,transform.position.y) + (direction.normalized * attackSight);
                    Debug.DrawLine(transform.position, dir_c, Color.green);

                    if(Vector2.Distance(parent.position, target.position) < attackSight){
                        state = MobState.Attack;
                    }else if(Vector2.Distance(parent.position, target.position) > (radiusSight + 1f)){
                        state = MobState.Idle;
                    }else{
                        MoveToward(target);
                    }

                    
                }else{
                    if(Vector2.Distance(parent.position, initialPost.position) > 0){
                        MoveToward(initialPost);
                        if(Vector2.Distance(parent.position, initialPost.position) < 0.1f){
                            state = MobState.Idle;
                        }
                    }
                }
            break;
            case MobState.Attack:
                if(Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackTime / attackRate;
                }
            break;
        }

    }

    private void ShowAttackLine()
    {
        Vector2 direction = target.position - transform.position;
        Vector2 targetDirection =  new Vector2(transform.position.x,transform.position.y) + (direction.normalized * attackSight);
        // Debug.DrawLine(transform.position, dir_c, Color.green);
        attackLine.positionCount = 2;
        attackLine.useWorldSpace = true;
        attackLine.numCapVertices = 0;
        Vector3 [] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = Vector3.MoveTowards(transform.position, targetDirection, attackSight);
        attackLine.SetPositions(points);
    }

    private void Attack()
    {
        if(!isAttacking){
            rb.bodyType = RigidbodyType2D.Static;
            charAnim.SetBool("isAttacking", true);
            ShowAttackLine();

            // Invoke("FakeAttack",1);
            if(attackCharge != null) StopCoroutine(ChargeAttack());
            attackCharge = StartCoroutine(ChargeAttack());

            isAttacking = true;
        }
    }

    IEnumerator ChargeAttack(){
        Vector2 direction = target.position - transform.position;
        Vector2 targetDirection =  new Vector2(transform.position.x,transform.position.y) + (direction.normalized * attackSight);
        
        yield return new WaitForSeconds(1f);
        attackLine.positionCount = 0;

        float elapsedTime = 0;
        onAttack = true;
        while (elapsedTime < 0.5f)
        {
            
            transform.position = Vector3.Lerp(transform.position, targetDirection, (elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onAttack = false;

        yield return new WaitForSeconds(1f);
        state = MobState.Move;
        charAnim.SetBool("isAttacking", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        isAttacking = false;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if(onAttack){
            if(other != null && other.gameObject.CompareTag("Actors")){
                Actor targetAtk = other.gameObject.GetComponentInChildren<Actor>();
                if(targetAtk != null)
                targetAtk.ApplyDamage(this);
            }
        }
    }
}
