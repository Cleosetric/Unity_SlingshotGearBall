﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword3 : AttackPatterns
{
    public float speed;

    private Player player;
    private Rigidbody2D rb;
    private int baseAttack;
    private float baseRange;

    public void InitializeSkill(int attack, float range, float duration){
        Start();
        baseRange = range;
        baseAttack = attack;

        DrawCircleHitbox(player.transform.position, attack, range);
        StartCoroutine(HyperSpeed(new Vector2(1,1), duration, speed, rb));
        DestroySelf(gameObject, duration);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // DrawCircleHitbox(player.transform.position, baseAttack, baseRange);
        transform.position = player.transform.position;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, baseRange);
    }
}
