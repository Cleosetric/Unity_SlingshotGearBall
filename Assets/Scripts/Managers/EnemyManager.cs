using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
	private static EnemyManager _instance;
	public static EnemyManager Instance { get { return _instance; } }

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
   
    public GameObject enemyPrefabs;
    private Camera camera;
    private int indexEnemySpawn = 0;

    private void Start() {
        camera = Camera.main;
        // SpawnEnemy();
    }

    public void SpawnEnemy(){
        GameObject enemy;
        enemy = Instantiate(enemyPrefabs) as GameObject;
        enemy.transform.SetParent(transform);
        enemy.transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y + 4);
        enemy.gameObject.name = "Enemy "+indexEnemySpawn;
        indexEnemySpawn++;
    }
}
