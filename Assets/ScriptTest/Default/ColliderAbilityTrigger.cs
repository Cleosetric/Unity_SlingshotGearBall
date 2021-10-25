using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAbilityTrigger : MonoBehaviour
{
    private float range;
    private float force;
    private Actor actor;
    private Transform origin;

    public void Initialize(Actor actor, Transform origin, float range, float force){
        this.actor = actor;
        this.range = range;
        this.force = force;
        this.origin = origin;
    }

    public void Active(){
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(origin.position, range, actor.enemyLayers);
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Enemy")){
                Mob targetAtk =  hitObj.GetComponent<Mob>();
                if(targetAtk != null) targetAtk.ApplyDamage(actor);
                Vector3 collisionPoint = hitObj.ClosestPoint(origin.position);
                Vector3 collisionNormal = origin.position - collisionPoint;
                hitObj.GetComponent<Rigidbody2D>().AddForce(-collisionNormal * force);
            }
        }
    }
}
