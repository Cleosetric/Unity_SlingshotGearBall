using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : Balls
{
    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;

    public override void behaviour(){
        Debug.Log("Player init");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D hit) {
        Vector2 inNormal = hit.contacts[0].normal;		
        var lastSpeed = lastFrameVelocity.magnitude;
        var direction = Vector2.Reflect(lastFrameVelocity.normalized, inNormal);
        rb.velocity = direction * Mathf.Max(lastSpeed, normalSpeed);
    }
}
