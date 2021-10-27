using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAbilityTrigger : MonoBehaviour
{
    private ColliderAbility ability;
    private Actor actor;
    private Transform origin;
    private Vector2 lastVel;

    public void Initialize(ColliderAbility ability, Actor actor, Transform origin){
        this.ability = ability;
        this.actor = actor;
        this.origin = origin;
    }

    public void Active(){
        lastVel = actor.parent.GetComponent<Rigidbody2D>().velocity;
        if((int)ability.chantType == 1 || (int)ability.chantType == 2) 
        actor.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        StartCoroutine(BeginAttack());

        if((int)ability.chantType == 2){
            actor.parent.GetComponent<Rigidbody2D>().velocity = lastVel;
        }
    }

    private IEnumerator BeginAttack(){
        TimeManager.Instance.StartImpactMotion();
        
        Vector3 pos = actor.parent.position + new Vector3(0,0.1f,0);
        GameObject chantAnim = Instantiate(ability.chantAnimPrefab, pos, Quaternion.identity);
        
        yield return new WaitForSeconds(0.15f);
        Destroy(chantAnim);
        GameObject anim = Instantiate(ability.abilityEffect, origin.position + ability.positionOffset, Quaternion.identity);
        anim.transform.SetParent(actor.parent);

        for (int i = 0; i < ability.repeatTime; i++)
        {
            CircleHitBox();
            yield return new WaitForSeconds(ability.repeatDelay);
        }
    }

    private void CircleHitBox(){

        Collider2D[] hitBox = Physics2D.OverlapCircleAll(origin.position + ability.positionOffset, ability.range, actor.enemyLayers);
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Enemy")){
                Mob targetAtk =  hitObj.GetComponent<Mob>();
                if(targetAtk != null) targetAtk.ApplyDamage(actor);
                Vector3 collisionPoint = hitObj.ClosestPoint(origin.position);
                Vector3 collisionNormal = origin.position - collisionPoint;
                hitObj.GetComponent<Rigidbody2D>().AddForce(-collisionNormal * ability.hitForce, ForceMode2D.Impulse);
            }

            if(hitObj.CompareTag("Props")){
                hitObj.GetComponent<Box>().ApplyDamage(Mathf.RoundToInt(actor.statATK.GetValue()));
            }
        }
        ability.Deactivate();
        ability.isFinished = true;
    }

}
