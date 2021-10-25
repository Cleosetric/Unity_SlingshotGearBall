using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAbilityTrigger : MonoBehaviour
{
    TargetAttack target;
    private float range;
    private float force;
    private Actor skillCaster;

    public void Initialize(Actor actor, float range, float force, TargetAttack target){
        this.skillCaster = actor;
        this.range = range;
        this.force = force;
        this.target = target;
    }

    public void Active(){
        Vector3 originPos = transform.position;
        Vector3 directionRay;
        if((int)target == 0){
            Transform closestEnemy = EnemyManager.Instance.EnemyNearbyTransform(originPos);
            if(closestEnemy != null){
                Vector3 targetPos = closestEnemy.position;
                directionRay = (targetPos - originPos).normalized;
            }else{
                directionRay = Vector2.up;
            }
        }else{
            directionRay = Vector2.up;
        }

        RaycastHit2D ray = Physics2D.Raycast(originPos, directionRay, range, skillCaster.enemyLayers);
        if(ray.collider != null && ray.collider.CompareTag("Enemy")){
            Mob targetAtk =  ray.collider.GetComponent<Mob>();
            if(targetAtk != null) targetAtk.ApplyDamage(skillCaster);
            ray.collider.GetComponent<Rigidbody2D>().AddForce(-ray.normal * force);
        }
        Debug.DrawRay (originPos, directionRay * range, Color.green);
    }
}
