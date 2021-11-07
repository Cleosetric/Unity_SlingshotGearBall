using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{
    [Space]
    [Header("Mob Attack")]
    public Transform initialPost;
    private float attackTime;
    private float attackRate;
    private float nextAttackTime = 0;
    private float moveProgress = 0.0f;
    private LineRenderer attackLine;
    private bool onAttack = false;
    private Transform target;

    //Movement
    private Vector2 baseStartPoint;
    private Vector2 destinationPoint;
    private Vector2 startPoint;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        attackTime = mob.attackTime;
        attackRate = mob.attackRate;

        attackLine = GetComponent<LineRenderer>();

        initialPost.position = transform.position;
        startPoint = transform.localPosition;
        baseStartPoint = transform.localPosition;
        moveProgress = 0.0f;

        PickNewRandomDestination();
    }

    public override void Move()
    {
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

    public override void Attack()
    {
        if(target != null && target.gameObject.activeSelf){
            rb.velocity = Vector2.zero;
            charAnim.SetBool("isAttacking", true);
            // ShowAttackLine();
            Invoke("BasicAttack",1f);
        }
    }

    private void ShowAttackLine()
    {
        targetPosition = target.transform.position;
        attackLine.positionCount = 2;
        attackLine.useWorldSpace = true;
        attackLine.numCapVertices = 10;
        Vector3 [] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = Vector3.MoveTowards(transform.position, targetPosition, attackSight);
        attackLine.SetPositions(points);
    }

    void BasicAttack(){
        if(target != null && target.gameObject.activeSelf){
            onAttack = true;
            Vector2 dir = (targetPosition - transform.position).normalized;
            rb.AddForce(dir * 20f, ForceMode2D.Impulse);

            Collider2D[] hitBox = Physics2D.OverlapCircleAll(transform.position, 1f, actorLayer);
            foreach (Collider2D hitObj in hitBox)
            {
                if(hitObj != null && hitObj.CompareTag("Actors")){
                    Actor targetAtk = target.GetComponentInChildren<Actor>();
                    if(targetAtk != null)
                    targetAtk.ApplyDamage(this);
                }
            }
        }
        attackLine.positionCount = 0;
        onAttack = false;
        charAnim.SetBool("isAttacking", false);
    }

    private void CheckAttackDistance(float attackSight)
    {
        Vector2 direction = (target.position - transform.position).normalized * attackSight;
        Debug.DrawRay(transform.position, direction, Color.green);

        float distance = Vector2.Distance(target.position, transform.position);
        if(distance <= attackSight){
            if(Time.fixedTime >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.fixedTime + attackTime / attackRate;
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
    
    private void MoveToward(Transform selectedTarget)
    {
        if(!onAttack){
            // Vector2 targetPos = new Vector2(selectedTarget.position.x, selectedTarget.position.y);
            // rb.velocity = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);;
            // transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            Vector2 newPosition = Vector2.MoveTowards(transform.position, selectedTarget.position, Time.fixedDeltaTime * moveSpeed);
            rb.MovePosition(newPosition);
        }
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

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        PickNewRandomDestination();
    }
}
