using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : Balls
{
    public int healthPoint = 3;
    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;

    public override void behaviour(){
        Debug.Log("Enemy init");
        PlayerBall.OnDoDamage += DoDamage;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        lastFrameVelocity = rb.velocity;
    }
    
    private void OnCollisionEnter2D(Collision2D hit) {
        if(hit != null && hit.gameObject.tag != "Player"){
            Vector2 inNormal = hit.contacts[0].normal;		
            var lastSpeed = lastFrameVelocity.magnitude;
            var direction = Vector2.Reflect(lastFrameVelocity.normalized, inNormal);
            rb.velocity = direction * Mathf.Max(lastSpeed, normalSpeed);
        }
    }
    public void DoDamage(int damage){
        healthPoint -= damage;
        CheckHp();
    }

    private void CheckHp(){
        if(healthPoint <= 0){
            Destroy(this.gameObject);
        }
    }
}
