using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword1 : AttackPatterns
{
    private Player player;
    private float tempRange;

    public void InitializeSkill(int attack, float range){
        tempRange = range;
        Start();
        DrawCircleHitbox(transform.position, attack, range);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void LateUpdate()
    {
        transform.position = player.transform.position;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tempRange);
    }
}
