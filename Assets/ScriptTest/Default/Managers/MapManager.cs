using System.Collections;
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
    private GameObject map;
    private bool isGoalShow = false;
    
    private void Start() {
        party = Party.Instance;
        cameraMgr = CameraManager.Instance;
        enemyMgr = EnemyManager.Instance;
        party.partyOnActorChanged += RefreshParty;

        cam = cameraMgr.GetCamera();
        actor = party.GetLeader();
    }

    private void RefreshParty()
    {
        actor = party.GetLeader();
    }

    private void Update() {
        if(EnemyManager.Instance.IsAllEnemyDead()){
            if(!isGoalShow){
                GameObject mapGoal = FindGameObjectInChildWithTag(map, "Goal");
                if(mapGoal != null) StartCoroutine(ShowGoal(mapGoal));
                isGoalShow = true;
            }
        }
    }

    private IEnumerator ShowGoal(GameObject mapGoal){
        mapGoal.SetActive(false);
        yield return new WaitForSeconds(3f);
        mapGoal.SetActive(true);
    }

    public string GetStage(){
        return "Stage "+(indexMapSpawn+1)+"/"+stage.Count;
    }

    public bool IsMapHasGoal(){
        if(map != null){
            if(FindGameObjectInChildWithTag(map, "Goal") != null){
                return true;
            }
        }
        return false;
    }

    public void SpawnNewStage(){
        if(indexMapSpawn < stage.Count){
            map = Instantiate(stage[indexMapSpawn]) as GameObject;
            map.transform.SetParent(transform);
            map.transform.localPosition = Vector2.up * (upOffset * indexMapSpawn);
            map.gameObject.name = "Stage "+indexMapSpawn;
            GameObject mapGoal = FindGameObjectInChildWithTag(map, "Goal");
            if(mapGoal != null) mapGoal.SetActive(false);

            Transform spawnPos = FindGameObjectInChildWithTag(map, "Respawn").transform;
            Vector3 endPos = new Vector3(spawnPos.position.x, spawnPos.position.y, -10);
            StartCoroutine(MoveCameraToNewMap(endPos));

            enemyMgr.FloodEnemy();
            isGoalShow = false;
            GameManager.Instance.isStageClear = false;
            
            GameObject lastMap = GameObject.Find("Stage "+(indexMapSpawn-1));
            DestroyLastMap(lastMap);

            indexMapSpawn ++;
        }
    }

    void DestroyLastMap(GameObject map){
        if(indexMapSpawn > 0){
            Destroy(map, 5);
        }
    }
    IEnumerator MoveCameraToNewMap(Vector3 endPos)
    {
        cam.GetComponent<CinemachineFollow>().StartZoomOut();
        // cam.Follow = null;

        foreach (Actor actor in Party.Instance.actors)
        {
            if(actor != null && actor.isAlive)
            actor.transform.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        float duration = 2f;
        yield return new WaitForSeconds(duration);
        
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {   
            if(actor != null && actor.isAlive){
                actor.transform.position = Vector3.Lerp(actor.transform.position, endPos, t / duration);
            }
            yield return null;
        }
        actor.transform.position = endPos;

        Vector2 spawnPos = new Vector2(endPos.x,endPos.y);
        party.transform.position = spawnPos;
        foreach (Actor actor in party.actors)
        {
            if(actor != null && actor.isAlive){
                actor.transform.position = spawnPos;
                actor.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach (Actor actor in party.actors)
        {
            if(actor != null && actor.isAlive)
            actor.transform.gameObject.SetActive(true);
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
