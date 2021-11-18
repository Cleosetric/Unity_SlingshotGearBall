using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow2 : MonoBehaviour
{
    public GameObject arrowPrefabs;

    private Player player;
    private int baseAttack;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void ShootBow(int attack)
    {
        Start();
        baseAttack = attack;
        StartShoot();
    }

    void StartShoot(){
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject arrow = Instantiate(arrowPrefabs, player.transform.position, Quaternion.identity) as GameObject;
        arrow.GetComponent<Arrow>().Initialize(player, baseAttack, -90, false);
        Destroy(gameObject);
    }

}
