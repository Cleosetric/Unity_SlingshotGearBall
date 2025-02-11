﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
    public static event Action<int> OnComboCounter = delegate { };

    public LayerMask enemyLayers;

    protected void OnComboAdd(int value){
        OnComboCounter(value);
    }

    protected void DrawCircleHitbox(Vector3 position, int attack, float range, bool destroyInContact){
        Collider2D[] hitBox = Physics2D.OverlapCircleAll(position, range, enemyLayers);
        foreach (Collider2D hitObj in hitBox)
        {
            if(hitObj.CompareTag("Enemy")){
                hitObj.GetComponent<Enemy>().ApplyDamage(attack);
                OnComboCounter(1);
                if(destroyInContact) Destroy(gameObject);
            }

            if(hitObj.CompareTag("Props")){
               hitObj.GetComponent<Props>().ApplyDamage(attack);
            }
        }
    }

    protected void DrawRayHitBox(Vector2 pos, Vector2 dir, float length, int attack, bool knockback){
        RaycastHit2D ray = Physics2D.Raycast(pos, dir, length, enemyLayers);
        Debug.DrawRay(pos, dir, Color.red, length);

        if(ray.collider != null && ray.collider.CompareTag("Enemy")){
            ray.collider.GetComponent<Enemy>().ApplyDamage(attack);
            OnComboCounter(1);
            if(knockback){
                Vector2 enemyPos = new Vector2(ray.collider.transform.position.x, ray.collider.transform.position.y);
                Vector2 difference = (enemyPos - pos).normalized;
                Vector2 force = difference * 5f;
                Rigidbody2D rb = ray.collider.GetComponent<Rigidbody2D>();
                rb.AddForce(force, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
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
