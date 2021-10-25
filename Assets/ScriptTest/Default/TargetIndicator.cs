using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public SpriteRenderer sprite;
    
    private void Awake() {
        sprite.enabled = true;
    }

    public void ShowIndicator(){
        sprite.enabled = true;
    }

    public void HideIndicator(){
        sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyTarget();
    }

    private void CheckEnemyTarget()
    {
        Transform closestEnemy = EnemyManager.Instance.EnemyNearbyTransform(transform.position);
        if (closestEnemy != null){
            Vector3 direction = (closestEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + -90); 
        }else{
            sprite.enabled = false;
        }
    }
}
