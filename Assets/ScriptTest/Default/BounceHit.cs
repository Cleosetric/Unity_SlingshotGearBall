using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceHit : MonoBehaviour
{
    public Transform actorLook;
    public GameObject bounceEffect;
    Rigidbody2D rb;
    Vector2 lastVel;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Props"){
                Vector3 hitPoint = other.contacts[0].point;
                Instantiate(bounceEffect,hitPoint,Quaternion.identity);
                other.collider.GetComponent<Props>().ApplyDamage(1);
            }

            if(other.collider.tag == "Wall"){
                Vector3 hitPoint = other.contacts[0].point;
                Instantiate(bounceEffect,hitPoint,Quaternion.identity);
            }

            Vector2 inNormal = other.contacts[0].normal;
            Vector2 force = Vector3.Reflect(lastVel, inNormal);
            
            Quaternion rotation = Quaternion.LookRotation(force, Vector3.up);
            actorLook.rotation = rotation;

            rb.velocity = force;
            rb.velocity += inNormal * 2.0f;
    }

    private void FixedUpdate() {
        lastVel = rb.velocity;
    }
}
