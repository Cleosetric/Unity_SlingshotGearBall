using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IDamagable
{
    public int currentHealth {
        get;

        private set;
    }

    public void ApplyDamage(int damage){

        currentHealth -= damage;
        if(currentHealth <= 0){
            currentHealth = 0;
            Die();
        }
    }

    private void Die(){
        
    }
}
