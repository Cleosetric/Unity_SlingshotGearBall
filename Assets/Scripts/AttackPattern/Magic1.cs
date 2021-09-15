using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic1 : AttackPatterns
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] float maxDistance;
    [SerializeField] float maxRayLength;
    [SerializeField] float timeDuration;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    private Player player;
    private GameObject projectile;
    private int baseAttack;

    public void InitializeSkill(int attack){
        Start();
        baseAttack = attack;
        Invoke("Shoot", 1f);
    }

    void Shoot(){
        projectile = Instantiate(firePrefab, player.transform.position, Quaternion.identity);
        projectile.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(projectile, timeDuration);

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        enemyList.Add(player.gameObject);
        CheckDistanceEachObject();
    }
    
     void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void CheckDistanceEachObject()
    {
        GameObject[] enemyGO = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyGO)
        {
            enemyList.Add(enemy);
        }

        enemyList.Sort(delegate(GameObject t1, GameObject t2){ 
            return Vector3.Distance(t1.transform.position,player.transform.position).CompareTo(Vector3.Distance(t2.transform.position, player.transform.position));
        });

        List<GameObject> enemyInRange = new List<GameObject>();
        foreach (GameObject enemy in enemyList)
        {
            if(Vector3.Distance(enemy.transform.position, player.transform.position) < maxDistance){
                enemyInRange.Add(enemy);
            }
        }
        StartCoroutine(ShootProjectile(enemyInRange));
    }

    void DrawRaycastHit(Vector2 origin, Vector2 direction, float length){
        RaycastHit2D ray = Physics2D.Raycast(origin, direction, length);
        Debug.DrawRay(origin, direction * length, Color.red);

        if(ray.collider != null && ray.collider.CompareTag("Enemy")){
            ray.collider.GetComponent<Enemy>().ApplyDamage(baseAttack);
            OnComboAdd(1);
        }
    }

    IEnumerator ShootProjectile(List<GameObject> enemyInRange){
        projectile.GetComponent<SpriteRenderer>().enabled = true;

        if(enemyInRange.Count > 1){
            float segmentDuration = timeDuration / enemyInRange.Count;
            for (int i = 0; i < enemyInRange.Count; i++)
            {
                float startTime = Time.time;
                Vector3 startPos, endPos;
                if(i == (enemyInRange.Count - 1)){
                    startPos = enemyInRange[i].transform.position;
                    endPos = player.transform.position;
                    projectile.GetComponent<SpriteRenderer>().enabled = false;
                    Destroy(projectile);
                    DrawRaycastHit(startPos, Vector2.zero, maxRayLength);
                }else{
                    startPos = enemyInRange[i].transform.position;
                    endPos = enemyInRange[i+1].transform.position;
                    Vector3 dir = (endPos - startPos).normalized;
                    DrawRaycastHit(startPos, dir, maxRayLength);
                }
            
                Vector3 pos = startPos;
                while(pos != endPos){
                    float t = (Time.time - startTime) / segmentDuration;
                    pos = Vector3.Lerp(startPos, endPos, t);
                    projectile.transform.position = pos;
                    float angle = Mathf.Atan2(endPos.y, endPos.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle -90);
                    yield return null;
                }
            }
           
            Destroy(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
}
