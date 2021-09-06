using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword2 : AttackPatterns
{
    public float speed;
    public float lifeTime;

    private Player player;
    private int baseAttack;
    private float baseRange;
    
    public void InitializeSkill(int attack, float range){
        Start();
        baseAttack = attack;
        baseRange = range;

        SetAngleToEnemy();
        DestroySelf(gameObject, lifeTime);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        DrawCircleHitbox(transform.position, baseAttack, baseRange);
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void SetAngleToEnemy()
    {
        Transform closestEnemy = EnemyNearbyTransform(player.transform.position);
        Vector3 direction;

        if (closestEnemy != null)
        {
            direction = closestEnemy.position - player.transform.position;
        }
        else
        {
            direction = Vector2.up;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + -90);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, baseRange);
    }
}
