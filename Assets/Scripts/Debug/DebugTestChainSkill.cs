using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTestChainSkill : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] float maxDistance;
    [SerializeField] float maxRayLength;
    [SerializeField] float timeDuration;
    [SerializeField] List<GameObject> allObj = new List<GameObject>();
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        projectile.transform.position = player.transform.position;
        projectile.GetComponent<SpriteRenderer>().enabled = false;
        CheckDistanceEachObject();
    }

    private void CheckDistanceEachObject()
    {
        allObj.Sort(delegate(GameObject t1, GameObject t2){ 
            return Vector3.Distance(t1.transform.position,player.transform.position).CompareTo(Vector3.Distance(t2.transform.position, player.transform.position));
        });

        List<GameObject> enemyInRange = new List<GameObject>();
        foreach (GameObject enemy in allObj)
        {
            if(Vector3.Distance(enemy.transform.position, player.transform.position) < maxDistance){
                enemyInRange.Add(enemy);
            }
        }
        StartCoroutine(ShootProjectile(enemyInRange));
    }

    void DrawRaycastHit(Vector2 origin, Vector2 direction, float length){
        // Vector3 direction = (pos2 - pos1).normalized;
        RaycastHit2D ray = Physics2D.Raycast(origin, direction, length);
        Debug.DrawRay(origin, direction * length, Color.red);

        if(ray.collider != null){
            Debug.Log(ray.collider.name);
            ray.collider.GetComponent<SpriteRenderer>().color = Color.red;
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
                    yield return null;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            CheckDistanceEachObject();
        }

        if(Input.GetMouseButtonUp(0)){
            foreach (GameObject item in allObj)
            {
                item.GetComponent<SpriteRenderer>().color = Color.white;
                projectile.transform.position = player.transform.position;
            }
        }
    }
}
