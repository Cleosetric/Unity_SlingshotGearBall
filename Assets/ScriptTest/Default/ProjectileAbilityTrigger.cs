using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityTrigger : MonoBehaviour
{
    private Ability ability;
    private Projectile projectile;
    private TargetAttack target;
    private ChantType chantType;
    private HitType hitType;
    private Direction projectileDir;
    private Transform origin;
    private float projectileForce;
    private float projectileLife;
    private float projectileImpact;
    private int projectileCount;
    private float projectileDelay;
    private GameObject animationCast;
    private Vector2 lastVel;

    public void Initialize(Ability ability, Projectile projectile, Transform origin, float force, float life, 
    float impact, int count, float delay, TargetAttack target, ChantType chant, HitType hit, Direction direction,
    GameObject anim){
        this.ability = ability;
        this.projectile = projectile;
        this.origin = origin;
        this.projectileForce = force;
        this.projectileLife = life;
        this.projectileImpact = impact;
        this.projectileCount = count;
        this.projectileDelay = delay;
        this.target = target;
        this.chantType = chant;
        this.hitType = hit;
        this.projectileDir = direction;
        this.animationCast = anim;
    }

    public void Active()
    {
        StopCoroutine(ShowChantAnimation());
        StartCoroutine(ShowChantAnimation());
    }

    private Vector3 GetDirection()
    {
        Vector3 direction;
        if((int)target == 0){
            Transform closestEnemy = EnemyManager.Instance.EnemyNearbyTransform(origin.position);
            if(closestEnemy != null){
                Vector3 targetPos = closestEnemy.position;
                direction = (targetPos - origin.position).normalized;
            }else{
                direction = origin.GetComponent<Actor>().directionFace.forward;
            }
        }else if((int)target == 1){
            direction = origin.GetComponent<Actor>().directionFace.forward;
        }else if((int)target == 2){
            direction = Vector2.up;
        }else if((int)target == 3){
            direction = Vector2.down;
        }else{
            direction = Vector2.zero;
        }
        return direction;
    }

    void RadialProjectile(float angle)
	{
        float projectileDirXposition = origin.position.x + Mathf.Sin ((angle * Mathf.PI) / 180) * 5f;
        float projectileDirYposition = origin.position.y + Mathf.Cos ((angle * Mathf.PI) / 180) * 5f;

        Vector3 projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
        Vector2 projectileMoveDirection = (projectileVector - origin.position).normalized;

        Vector3 directionAngle = Quaternion.Euler(0, 0, 90) * projectileMoveDirection;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionAngle);

        GameObject clonedBullet = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact,true,ability.rotateProjectile, targetRotation);
        DestroyProjectile(clonedBullet.gameObject, projectileLife);
	}

    void SingleProjectile(float offset){
        Vector3 direction = GetDirection();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject clonedBullet = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact,true,ability.rotateProjectile, Quaternion.Euler(0, 0, angle + offset));
        DestroyProjectile(clonedBullet.gameObject, projectileLife);
    }

    void DoubleProjectile(float offset1, float offset2){
        Vector3 direction = GetDirection();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject clonedBullet = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact, true, ability.rotateProjectile, Quaternion.Euler(0, 0, angle + offset1));
        DestroyProjectile(clonedBullet.gameObject, projectileLife);

        GameObject clonedBullet2 = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet2.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact,true, ability.rotateProjectile, Quaternion.Euler(0, 0, angle + offset2));
        DestroyProjectile(clonedBullet2.gameObject, projectileLife);
    }

    void TrippleProjectile(float offset1, float offset2, float offset3){
        Vector3 direction = GetDirection();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject clonedBullet = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact,true, ability.rotateProjectile, Quaternion.Euler(0, 0, angle + offset1));
        DestroyProjectile(clonedBullet.gameObject, projectileLife);

        GameObject clonedBullet2 = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet2.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact,true, ability.rotateProjectile, Quaternion.Euler(0, 0, angle + offset2));
        DestroyProjectile(clonedBullet2.gameObject, projectileLife);

        GameObject clonedBullet3 = Instantiate(projectile.gameObject, origin.position, Quaternion.identity);
        clonedBullet3.GetComponent<Projectile>().Initialize(origin.GetComponent<Actor>(), hitType, projectileForce, 
        projectileImpact,true, ability.rotateProjectile, Quaternion.Euler(0, 0, angle + offset3));
        DestroyProjectile(clonedBullet3.gameObject, projectileLife);
    }

    IEnumerator ShowChantAnimation(){
        TimeManager.Instance.StartImpactMotion();

        lastVel = origin.GetComponentInParent<Rigidbody2D>().velocity;
        if((int)chantType == 1 || (int)chantType == 2) 
        origin.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;

        Vector3 pos = origin.position + new Vector3(0,0.1f,0);
        GameObject anim = Instantiate(animationCast, pos, Quaternion.identity);
        anim.transform.SetParent(origin.transform);

        yield return new WaitForSeconds(0.15f);

        Destroy(anim);

        switch((int)projectileDir){
            case 0:
                for (int i = 0; i < projectileCount; i++)
                {
                    SingleProjectile(-90);
                    yield return new WaitForSeconds(projectileDelay);
                }
            break;
            case 1:
                for (int i = 0; i < projectileCount; i++)
                {
                    DoubleProjectile(-75,-105);
                    yield return new WaitForSeconds(projectileDelay);
                }
            break;
            case 2:
                for (int i = 0; i < projectileCount; i++)
                {
                    TrippleProjectile(-75,-90,-105);
                    yield return new WaitForSeconds(projectileDelay);
                }
            break;
            case 3:
                Vector3 direction = GetDirection();
                float angleStep = 360f / projectileCount;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                for (int i = 0; i <= projectileCount - 1; i++) {
                    RadialProjectile(angle);
                    angle += angleStep;
                    yield return new WaitForSeconds(projectileDelay);
                }
            break;
        }

        // yield return new WaitForSeconds(0.25f);
        
        if((int)chantType == 2){
            origin.GetComponentInParent<Rigidbody2D>().velocity = lastVel;
        }
        ability.Deactivate();
        ability.isFinished = true;
    }

    void DestroyProjectile(GameObject obj, float life){
        Destroy(obj, life);
    }
}
