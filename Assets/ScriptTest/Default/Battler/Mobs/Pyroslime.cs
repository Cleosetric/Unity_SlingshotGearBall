using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyroslime : Mob
{
    [Space]
    [Header("Mob Attack")]
    public GameObject touchEffect;
    private float attackTime;
    private float attackRate;
    private float nextAttackTime = 0;

    [Space]
    [Header("Mob Movement")]
    public Transform startPoint;
    public Transform secondPoint;
    public Transform wayPoint;

    private Transform target;

    private void Start() {
        attackTime = mob.attackTime;
        attackRate = mob.attackRate;
        wayPoint = startPoint;
    }

    private Transform CheckTargetsInSight(float range)
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
            return targets[0];
        }

        return null;
    }

    public override void Move(){
        target = CheckTargetsInSight(radiusSight);
        if(target != null){
            if(Vector2.Distance(transform.position, target.position) > attackSight){
                if(Time.fixedTime >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.fixedTime + attackTime / attackRate;
                }
            }
        }

        if(Vector2.Distance(transform.position, startPoint.position) < 0.2f){
            wayPoint = secondPoint;
        }

        if(Vector2.Distance(transform.position, secondPoint.position) < 0.2f){
            wayPoint = startPoint;
        }

        MoveToward(wayPoint);
    }

    private void MoveToward(Transform selectedTarget)
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, selectedTarget.position, Time.fixedDeltaTime * moveSpeed);
        rb.MovePosition(newPosition);
    }

    public override void Attack()
    {
        // Vector2 direction = (target.transform.position - transform.position).normalized;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // GameObject clonedBullet = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
        // clonedBullet.GetComponent<Projectile>().Initialize(this, 0, 10, 
        // 5, true, Quaternion.Euler(0, 0, angle + -90));
        // Destroy(clonedBullet, projectileLife);
    }


    public override void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);

        if(other.collider.CompareTag("Actors")){
            Actor targetAtk = target.GetComponentInChildren<Actor>();
            if(targetAtk != null){
                targetAtk.ApplyDamage(this);
                Vector3 hitPoint = other.contacts[0].point;
                Instantiate(touchEffect,hitPoint,Quaternion.identity);
            }
        }
    }
}
