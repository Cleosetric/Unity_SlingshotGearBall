using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryoslime : Mob
{
    [Space]
    [Header("Mob Attack")]
    public Projectile projectile;
    public GameObject touchEffect;
    public float projectileLife;
    private float attackTime;
    private float attackRate;
    private float nextAttackTime = 0;

    [Space]
    [Header("Mob Movement")]
    public Transform startPoint;
    public Transform secondPoint;
    public Transform wayPoint;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackTime = mob.attackTime;
        attackRate = mob.attackRate;
        wayPoint = startPoint;
    }

    // Update is called once per frame
    public override void Move()
    {
        base.Move();
        
        if(Vector2.Distance(transform.position, startPoint.position) < 0.05f){
            wayPoint = secondPoint;
        }

        if(Vector2.Distance(transform.position, secondPoint.position) < 0.05f){
            wayPoint = startPoint;
        }

        MoveToward(wayPoint);
        
        if(Time.time >= nextAttackTime)
        {
            Shoot();
            nextAttackTime = Time.time + attackTime / attackRate;
        }

    }

    void Shoot()
    {
        Vector2 direction = Vector2.down;
        Vector2 dir_c =  new Vector2(transform.position.x,transform.position.y) + (direction.normalized * attackSight);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.DrawLine(transform.position, dir_c, Color.green);

        GameObject shoot = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
        shoot.GetComponent<Projectile>().Initialize(this, 0, 10f, 5f, true, false, Quaternion.Euler(0, 0, angle + -90f));
        Destroy(shoot, projectileLife);
    }



    public override void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);

        if(other.collider.CompareTag("Actors")){
            Actor targetAtk = other.gameObject.GetComponent<Actor>();
            if(targetAtk != null){
                targetAtk.ApplyDamage(this);
                Vector3 hitPoint = other.contacts[0].point;
                Instantiate(touchEffect,hitPoint,Quaternion.identity);
            }
        }
    }
}
