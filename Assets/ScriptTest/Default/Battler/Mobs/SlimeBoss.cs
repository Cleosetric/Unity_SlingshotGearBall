using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPhase{
    Phase1, Phase2, Phase3
}
public class SlimeBoss : Mob
{
    [Space]
    [Header("Mob Attack")]
    public Transform initialPost;
    public Projectile projectile;
    private float attackTime;
    private float attackRate;
    private float nextAttackTime = 0;
    private MobState state;
    private BossPhase phase;
    private LineRenderer attackLine;
    private Coroutine attackCharge;
    private bool isChargeAttack;
    private bool isShootAttack;
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
        phase = BossPhase.Phase1;
    }

    public override void Move(){
        base.Move();

        if(currentHP > statMHP.GetValue() * 0.6){
            phase = BossPhase.Phase1;
        }else if(currentHP > statMHP.GetValue() * 0.3){
            phase = BossPhase.Phase2;
        }else{
            phase = BossPhase.Phase3;
        }

        switch (phase)
        {
            case BossPhase.Phase1:

                switch (state)
                {
                    case MobState.Idle:
                        target = CheckTargetsInSight(radiusSight);
                        if(target != null){
                            state = MobState.Attack;
                        }
                    break;
                    case MobState.Move:
                        if(Vector2.Distance(parent.position, initialPost.position) > 0){
                            MoveToward(initialPost);
                        }

                        if(Vector2.Distance(parent.position, initialPost.position) < 0.2f){
                            state = MobState.Idle;
                        }
                    break;
                    case MobState.Attack:
                        PrepareChargeAttack();
                    break;
                }
            break;
            case BossPhase.Phase2:
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
                        if(target == null){
                            isShootAttack = false;
                            state = MobState.Idle;
                        } else {
                            if(Vector2.Distance(parent.position, target.position) < (attackSight/2)){
                                state = MobState.Attack;
                            }else if(Vector2.Distance(parent.position, target.position) > (radiusSight + 1f)){
                                state = MobState.Idle;
                            }else{
                                MoveToward(target);
                            }
                        }

                    break;
                    case MobState.Attack:
                        if(target == null){
                            isShootAttack = false;
                            state = MobState.Idle;
                        }else{
                            if(Time.time >= nextAttackTime)
                            {
                                float rng = UnityEngine.Random.Range(0f, 1f);
                                Debug.Log(rng);
                                PrepareShootProjectile(rng);
                                nextAttackTime = Time.time + attackTime / attackRate;
                            }
                        }
                    break;
                }
            break;
            case BossPhase.Phase3:
                onAttack = true;
                charAnim.SetBool("isRaging", true);
                switch (state)
                {
                    case MobState.Idle:
                        target = CheckTargetsInSight(radiusSight);
                        if(target != null){
                            state = MobState.Attack;
                        }
                    break;
                    case MobState.Move:
                        if(target == null) state = MobState.Idle;
                        if(Vector2.Distance(parent.position, initialPost.position) > 0){
                            MoveToward(initialPost);
                        }

                        if(Vector2.Distance(parent.position, initialPost.position) < 0.2f){
                            state = MobState.Idle;
                        }
                    break;
                    case MobState.Attack:
                        PrepareChargeAttackFinal();
                    break;
                }
            break;
        }
    }

    private void PrepareShootProjectile(float rand)
    {
        if(!isShootAttack){
            if(rand > 0.5f){
                StartCoroutine(ShootAttack());
            }else{
                StartCoroutine(ShootRadialAttack());
            }
            isShootAttack = true;
        }
    }

    private void PrepareChargeAttack()
    {
        if(!isChargeAttack){
            rb.bodyType = RigidbodyType2D.Static;
            charAnim.SetBool("isAttacking", true);
            ShowAttackLine();
            StartCoroutine(ChargeAttack());
            isChargeAttack = true;
        }
    }

    private void PrepareChargeAttackFinal()
    {
        if(!isChargeAttack){
            rb.bodyType = RigidbodyType2D.Static;
            charAnim.SetBool("isAttacking", true);
            ShowAttackLine();
            StartCoroutine(ChargeAttackFinal());
            isChargeAttack = true;
        }
    }

    IEnumerator ShootAttack(){
        rb.bodyType = RigidbodyType2D.Static;
        charAnim.SetBool("isAttacking", true);

        yield return new WaitForSeconds(1);
        for (int i = 0; i < 5; i++)
        {    
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject shoot = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
            shoot.GetComponent<Projectile>().Initialize(this, 0, 10f, 5f, true,false, Quaternion.Euler(0, 0, angle + -90f));
            Destroy(shoot, 1f);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1);

        charAnim.SetBool("isAttacking", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        state = MobState.Move;
        isShootAttack = false;
    }

    IEnumerator ShootRadialAttack(){
        rb.bodyType = RigidbodyType2D.Static;
        charAnim.SetBool("isAttacking", true);

        yield return new WaitForSeconds(1);

        Vector2 direction = target.position - transform.position;
        float angleStep = 360f / 10;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i <= 10 - 1; i++) {
            
            float projectileDirXposition = parent.position.x + Mathf.Sin ((angle * Mathf.PI) / 180) * 5f;
            float projectileDirYposition = parent.position.y + Mathf.Cos ((angle * Mathf.PI) / 180) * 5f;

            Vector3 projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - parent.position).normalized;

            Vector3 directionAngle = Quaternion.Euler(0, 0, 90) * projectileMoveDirection;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionAngle);

            GameObject shoot = Instantiate(projectile.gameObject, parent.position, Quaternion.identity);
            shoot.GetComponent<Projectile>().Initialize(this, 0, 10f, 5f, true,false, Quaternion.Euler(0, 0, angle + -90f));
            Destroy(shoot, 1f);

            angle += angleStep;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        charAnim.SetBool("isAttacking", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        state = MobState.Move;
        isShootAttack = false;
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
        charAnim.SetBool("isAttacking", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        state = MobState.Move;
        isChargeAttack = false;
    }

    IEnumerator ChargeAttackFinal(){
        Vector2 direction = target.position - transform.position;
        Vector2 targetDirection =  new Vector2(transform.position.x,transform.position.y) + (direction.normalized * attackSight);
        
        yield return new WaitForSeconds(1f);
        attackLine.positionCount = 0;

        float elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, targetDirection, (elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        
        float rng = UnityEngine.Random.Range(0f, 1f);
        PrepareShootProjectile(rng);

        yield return new WaitForSeconds(1f);
        charAnim.SetBool("isAttacking", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        state = MobState.Move;
        isChargeAttack = false;
    }

    private void ShowAttackLine()
    {
        Vector2 direction = target.position - transform.position;
        Vector2 targetDirection =  new Vector2(transform.position.x,transform.position.y) + (direction.normalized * attackSight);
        Debug.DrawLine(transform.position, targetDirection, Color.green);

        attackLine.positionCount = 2;
        attackLine.useWorldSpace = true;
        attackLine.numCapVertices = 0;
        Vector3 [] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = Vector3.MoveTowards(transform.position, targetDirection, attackSight);
        attackLine.SetPositions(points);
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
