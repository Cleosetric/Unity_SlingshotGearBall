using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
    public static event Action<int> OnComboCounter = delegate { };

    public LayerMask enemyLayers;

    protected void DrawCircleHitbox(Vector3 position, int attack, float range){
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(position, range, enemyLayers);
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Enemy")){
                hitObj.GetComponent<Enemy>().ApplyDamage(attack);
                OnComboCounter(1);
            }

            if(hitObj.CompareTag("Props")){
               hitObj.GetComponent<Box>().ApplyDamage(attack);
            }
        }
    }

    protected Transform EnemyNearbyTransform(Vector3 position){
        List<Transform> enemyList = new List<Transform>();
        GameObject[] enemyGO = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemyGO.Length > 0){
            foreach (GameObject enemy in enemyGO)
            {
                enemyList.Add(enemy.transform);
            }

            enemyList.Sort(delegate(Transform t1, Transform t2){ 
                return Vector3.Distance(t1.position,position).CompareTo(Vector3.Distance(t2.position, position));
            });

            return enemyList[0];
        }else{
            return null;
        }
    }

    protected IEnumerator HyperSpeed(Vector2 direction, float time, float speed, Rigidbody2D rb){
        yield return new WaitForEndOfFrame();
        float timeLength = 0;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
        
        while(timeLength <= time){
            rb.velocity = speed * (rb.velocity.normalized);
            timeLength += Time.deltaTime;
            yield return null;
        }
    }

    protected void DestroySelf(GameObject obj, float time){
        Destroy(obj, time);
    }
}
