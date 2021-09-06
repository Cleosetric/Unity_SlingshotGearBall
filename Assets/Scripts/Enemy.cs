using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public int currentHealth = 5;

    public void ApplyDamage(int damage){
        Debug.Log(gameObject.name + " Get Damaged!!");
        currentHealth -= damage;
        if(currentHealth <= 0){
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }
}
