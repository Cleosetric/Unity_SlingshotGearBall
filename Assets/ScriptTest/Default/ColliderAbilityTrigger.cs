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
        lastVel = actor.GetComponent<Rigidbody2D>().velocity;
        if((int)ability.chantType == 1 || (int)ability.chantType == 2) 
        actor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        Debug.Log((int)ability.skillHitType + "Skill Hit Type");
        if((int)ability.skillHitType == 0){
            Debug.Log("Once");
            StartCoroutine(BeginAttack());
        }else{
            Debug.Log("Update");
            StartCoroutine(BeginContinousAttack());
        }

        if((int)ability.chantType == 2){
            actor.GetComponent<Rigidbody2D>().velocity = lastVel;
        }
    }

    private IEnumerator BeginAttack(){
        TimeManager.Instance.StartImpactMotion();
        
        Vector3 pos = actor.transform.position + new Vector3(0,0.1f,0);
        GameObject chantAnim = Instantiate(ability.chantAnimPrefab, pos, Quaternion.identity);
        chantAnim.transform.SetParent(actor.transform);
        
        yield return new WaitForSeconds(0.15f);
        Destroy(chantAnim);
        GameObject anim = Instantiate(ability.abilityEffect, origin.position + ability.positionOffset, Quaternion.identity);
        anim.transform.SetParent(actor.transform);

        for (int i = 0; i < ability.repeatTime; i++)
        {
            CircleHitBox();
            yield return new WaitForSeconds(ability.repeatDelay);
        }

        yield return new WaitForSeconds(0.15f);

        Destroy(anim);
        ability.Deactivate();
        ability.isFinished = true;
    }

    private IEnumerator BeginContinousAttack(){
        TimeManager.Instance.StartImpactMotion();
        
        Vector3 pos = actor.transform.position + new Vector3(0,0.1f,0);
        GameObject chantAnim = Instantiate(ability.chantAnimPrefab, pos, Quaternion.identity);
        chantAnim.transform.SetParent(actor.transform);

        yield return new WaitForSeconds(0.15f);

        Destroy(chantAnim);
        GameObject anim = Instantiate(ability.abilityEffect, origin.position + ability.positionOffset, Quaternion.identity);
        anim.transform.SetParent(actor.transform);

        float ellapsedTime = 0;
        float nextDashTime = 0;
        while (ellapsedTime < ability.skillDuration)
        {
            if(ellapsedTime >= nextDashTime)
            {
                CircleHitBox();
                nextDashTime = ellapsedTime + 1 / ability.repeatDelay;
            }
            ellapsedTime += Time.deltaTime;
            Debug.Log("Skill Active! : " + ellapsedTime);
            yield return null;
        }
        Debug.Log("Skill's Done!");

        yield return new WaitForSeconds(0.15f);

        Destroy(anim);
        ability.Deactivate();
        ability.isFinished = true;
    }

    private void CircleHitBox(){

        Collider2D[] hitBox = Physics2D.OverlapCircleAll(origin.position + ability.positionOffset, ability.range, actor.enemyLayers);
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Enemy")){
                Mob targetAtk =  hitObj.GetComponent<Mob>();
                if(targetAtk != null){
                    targetAtk.ApplyDamage(actor);
                    Rigidbody2D otherRb = targetAtk.GetComponent<Rigidbody2D>();
                    Vector3 collisionPoint = hitObj.ClosestPoint(origin.position);
                    Vector3 collisionNormal = origin.position - collisionPoint;
                    otherRb.AddForce(-collisionNormal * ability.hitForce, ForceMode2D.Impulse);
                }
            }

            if(hitObj.CompareTag("Props")){
                hitObj.GetComponent<Props>().ApplyDamage(Mathf.RoundToInt(actor.statATK.GetValue()));
            }
        }
       
    }

}
