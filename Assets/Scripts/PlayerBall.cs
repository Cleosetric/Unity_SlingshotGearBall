using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public float normalSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;


    private void Start() {
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
