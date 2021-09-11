using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword4 : AttackPatterns
{
    public float speed;
    public float maxDistance;
    private Player player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public void InitializeSkill(){
        Start();
        MoveTowardClosestEnemy();
    }

    void MoveTowardClosestEnemy()
    {
        if(player != null || player.gameObject.activeSelf){
            Transform closestEnemy = EnemyNearbyTransform(player.transform.position);
            if (closestEnemy != null)
            {
                float distance = Vector3.Distance(closestEnemy.position, player.transform.position);                
                if(distance < maxDistance){
                    sprite.enabled = true;
                    Vector3 direction = closestEnemy.position - player.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle + -90);
                    rb.AddForce(direction * speed, ForceMode2D.Impulse);
                }else{
                    sprite.enabled = true;
                    rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
                }
            }else{
                sprite.enabled = false;
            }
        }
        Destroy(gameObject, 0.25f);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = player.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update(){
        this.transform.position = player.transform.position;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.up,0.35f, enemyLayers);
        Debug.DrawRay(transform.position, Vector2.up, Color.red, 0.35f);

        if(ray.collider != null && ray.collider.CompareTag("Enemy")){
            sprite.enabled = false;
            Destroy(gameObject);
        }

    }
}
