using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Camera cam;
    private int indexMapSpawn = 0;
    private Transform player;
    
    private void Start() {
        cam = Camera.main;
        player = FindObjectOfType<Player>().transform;
        SpawnNewStage();
    }

    public void SpawnNewStage(){
        if(indexMapSpawn < stage.Count){
            GameObject map;
            map = Instantiate(stage[indexMapSpawn]) as GameObject;
            map.transform.SetParent(transform);
            map.transform.position = Vector2.up * (upOffset * indexMapSpawn);
            map.gameObject.name = "Stage "+indexMapSpawn;

            Vector3 endPos = new Vector3( map.transform.position.x, map.transform.position.y,-10);
            StartCoroutine(MoveCameraToNewMap(cam.transform.position, endPos, 1f));
            indexMapSpawn ++;
        }
    }

    IEnumerator MoveCameraToNewMap(Vector3 pos1, Vector3 pos2, float duration)
    {
        Vector2 playerPos = new Vector2(pos2.x,pos2.y -4);
        player.position = playerPos;
        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(duration);
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            cam.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        cam.transform.position = pos2;
        player.gameObject.SetActive(true);

    }
    
}
