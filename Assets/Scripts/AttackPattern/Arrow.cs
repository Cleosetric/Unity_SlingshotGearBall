using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : AttackPatterns
{
    public float speed;
    public float lifeTime;

    private Player player;
    private SpriteRenderer sprite;
    private Vector2 direction;

    private int baseAttack;

    public void Initialize(int atk){
        Start();
        baseAttack = atk;
        SetAngleToEnemy();
        Destroy(gameObject, lifeTime);
    }

    void Start(){
        player = FindObjectOfType<Player>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
    }

    void Update(){
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        DrawRayHitBox(transform.position, direction, 0.3f, baseAttack);
    }

    void SetAngleToEnemy()
    {
        if(player != null && player.gameObject.activeSelf){
            Transform closestEnemy = EnemyNearbyTransform(player.transform.position);
            if (closestEnemy != null)
            {
                direction = closestEnemy.position - player.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + -90);
            }else{
                direction = Vector2.up;
            }
        }
    }

}
