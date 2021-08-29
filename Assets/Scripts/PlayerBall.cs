using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : Balls
{
    public static event Action<int> OnComboCounter = delegate { };
    public static event Action<int> OnDoDamage = delegate { };

    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;

    public override void behaviour(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D hit) {
        float impulse = calculateImpulse(hit);

        if(hit.collider.tag == "Enemy" && impulse > 30f){ 
            TimeManager.Instance.StartImpactMotion();
            EnemyBall enemy = hit.gameObject.GetComponent<EnemyBall>();
            enemy.DoDamage(1);
            OnComboCounter(1);
        }

        Vector2 inNormal = hit.contacts[0].normal;		
        var lastSpeed = lastFrameVelocity.magnitude;
        var direction = Vector2.Reflect(lastFrameVelocity.normalized, inNormal);
        rb.velocity = direction * Mathf.Max(lastSpeed, normalSpeed);
    }

    private float calculateImpulse(Collision2D col){
        float impact = 0f;
        foreach (ContactPoint2D cp in col.contacts) {
                impact += cp.normalImpulse;
        }
        return impact;
    }
}
