using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : AttackPatterns
{
    public float speed;
    public float lifeTime;
    private SpriteRenderer sprite;
    private Vector2 direction;

    private int baseAttack;
    private bool knockback;

    public void Initialize(Player player, int atk, float offset, bool knock){
        Start();
        baseAttack = atk;
        knockback = knock;
        SetAngleToEnemy(player, offset);
    }

    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        Destroy(gameObject, lifeTime);
    }

    void Update(){
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        DrawRayHitBox(transform.position, transform.up, 0.3f, baseAttack, knockback);
    }

    void SetAngleToEnemy(Player player, float offset)
    {
        if(player != null && player.gameObject.activeSelf){
            Transform closestEnemy = EnemyNearbyTransform(player.transform.position);
            if (closestEnemy != null)
            {
                direction = closestEnemy.position - player.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + offset);
            }else{
                direction = Vector2.up;
            }
        }
    }

}
