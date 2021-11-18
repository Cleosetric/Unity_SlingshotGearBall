using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyroslime : Mob
{
    [Space]
    [Header("Mob Attack")]
    public GameObject touchEffect;

    [Space]
    [Header("Mob Movement")]
    public Transform startPoint;
    public Transform secondPoint;
    public Transform wayPoint;

    protected override void Start() {
        base.Start();
        wayPoint = startPoint;
    }

    public override void Move(){
        base.Move();
        if(Vector2.Distance(transform.position, startPoint.position) < 0.2f){
            wayPoint = secondPoint;
        }

        if(Vector2.Distance(transform.position, secondPoint.position) < 0.2f){
            wayPoint = startPoint;
        }

        MoveToward(wayPoint);
    }

    public override void OnCollisionEnter2D(Collision2D other) {
        base.OnCollisionEnter2D(other);

        if(other.collider.CompareTag("Actors")){
            Actor targetAtk = other.gameObject.GetComponentInChildren<Actor>();
            if(targetAtk != null){
                targetAtk.ApplyDamage(this);
                Vector3 hitPoint = other.contacts[0].point;
                Instantiate(touchEffect,hitPoint,Quaternion.identity);
            }
        }
    }
}
