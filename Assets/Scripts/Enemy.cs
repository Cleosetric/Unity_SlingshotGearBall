using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public int currentHealth = 5;
    public SpriteRenderer sprite;

    public void ApplyDamage(int damage){
        // Debug.Log(gameObject.name + " Get Damaged!!");
        currentHealth -= damage;
        Blink();
        if(currentHealth <= 0){
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die(){
        Destroy(gameObject);
    }

    private IEnumerator Blink() {
        Color defaultColor = sprite.color;
        sprite.color = new Color(1, 0, 0,1);
        yield return new WaitForSeconds(0.05f);
        sprite.color = defaultColor ;
    }
}
