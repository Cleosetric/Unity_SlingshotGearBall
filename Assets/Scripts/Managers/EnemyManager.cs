using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
   
    public List<GameObject> enemiesAlive = new List<GameObject>();

    private void Start() {
        FloodEnemy();
    }

    public void FloodEnemy()
    {
        GameObject[] enemyGO = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesAlive.AddRange(enemyGO);
    }

    public Transform EnemyNearbyTransform(Vector3 position){
        enemiesAlive.RemoveAll(enemy => enemy == null);
        if(enemiesAlive.Count == 1){
            return enemiesAlive[0].transform;
        }else if(enemiesAlive.Count > 1){
            enemiesAlive.Sort(delegate(GameObject t1, GameObject t2){ 
                return Vector3.Distance(t1.transform.position,position).CompareTo(Vector3.Distance(t2.transform.position, position));
            });
            return enemiesAlive[0].transform;
        }else{
            return null;
        }
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
