using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow1 : MonoBehaviour
{
    public GameObject arrowPrefabs;
    public float shootDelay;

    private Player player;
    private int baseAttack;
    private int shootTimes = 0;

    void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(StartShoot(shootTimes,shootDelay));
    }

    public void ShootBow(int attack)
    {
        Start();
        baseAttack = attack;
        shootTimes ++;
    }

    IEnumerator StartShoot(int shootTime, float shootDelay){
        yield return new WaitForSeconds(shootDelay);
        Debug.Log("shoot : "+ shootTime);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        for (int i = 0; i < shootTime; i++)
        {
            GameObject arrow = Instantiate(arrowPrefabs, player.transform.position, Quaternion.identity) as GameObject;
            arrow.GetComponent<Arrow>().Initialize(baseAttack);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForEndOfFrame();
        shootTimes = 0;
        Destroy(gameObject);
    }
}
