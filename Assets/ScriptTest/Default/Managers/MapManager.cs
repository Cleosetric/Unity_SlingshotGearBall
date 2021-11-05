﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapManager : MonoBehaviour
{
    #region Singleton
	private static MapManager _instance;
	public static MapManager Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
	#endregion

    public List<GameObject> stage = new List<GameObject>();
    public float upOffset = 12;

    private int indexMapSpawn = 0;
    private Party party;
    private Actor actor;
    private CinemachineVirtualCamera cam;
    private CameraManager cameraMgr;
    private EnemyManager enemyMgr;
    
    private void Start() {
        party = Party.Instance;
        cameraMgr = CameraManager.Instance;
        enemyMgr = EnemyManager.Instance;

        cam = cameraMgr.GetCamera();
        actor = party.GetLeader();

        SpawnNewStage();
    }

    public void SpawnNewStage(){
        if(indexMapSpawn < stage.Count){
            GameObject map;
            map = Instantiate(stage[indexMapSpawn]) as GameObject;
            map.transform.SetParent(transform);
            map.transform.position = Vector2.up * (upOffset * indexMapSpawn);
            map.gameObject.name = "Stage "+indexMapSpawn;

            Transform spawnPos = FindGameObjectInChildWithTag(map, "Respawn").transform;
            Vector3 endPos = new Vector3(spawnPos.position.x, spawnPos.position.y, -10);
            StartCoroutine(MoveCameraToNewMap(endPos));

            enemyMgr.FloodEnemy();
            indexMapSpawn ++;
        }
    }

    IEnumerator MoveCameraToNewMap(Vector3 endPos)
    {
        cam.GetComponent<CinemachineFollow>().StartZoomOut();
        // cam.Follow = null;

        foreach (Actor actor in Party.Instance.actors)
        {
            if(actor != null && actor.isAlive)
            actor.parent.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        // yield return new WaitForSeconds(duration);
        // for (float t = 0f; t < duration; t += Time.deltaTime)
        // {
        //     Camera.main.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
        //     yield return 0;
        // }
        // Camera.main.transform.position = pos2;

        Vector2 spawnPos = new Vector2(endPos.x,endPos.y);
        party.transform.position = spawnPos;
        foreach (Actor actor in party.actors)
        {
            if(actor != null && actor.isAlive){
                actor.parent.position = spawnPos;
                actor.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach (Actor actor in party.actors)
        {
            if(actor != null && actor.isAlive)
            actor.parent.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.25f);
        GameManager.Instance.PlayTeleportAnimation();
        cam.GetComponent<CinemachineFollow>().StartZoomIn();
    }

    public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++) 
        {
            if(t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }
                
        }
            
        return null;
    }
    
}