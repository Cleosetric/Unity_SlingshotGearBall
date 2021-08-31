using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefabs;
    private Camera camera;
    private int indexEnemySpawn = 0;

    private void Start() {
        camera = Camera.main;
        SpawnEnemy();
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
