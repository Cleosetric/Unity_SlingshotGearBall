using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow3 : MonoBehaviour
{
    public GameObject arrowPrefabs;
    private Player player;
    private int baseAttack;

    public void ShootBow(int attack)
    {
        Start();
        baseAttack = attack;
        StartShoot();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void StartShoot(){
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject arrow1 = Instantiate(arrowPrefabs, player.transform.position, Quaternion.identity) as GameObject;
        arrow1.GetComponent<Arrow>().Initialize(player, baseAttack, -75, true);
        GameObject arrow2 = Instantiate(arrowPrefabs, player.transform.position, Quaternion.identity) as GameObject;
        arrow2.GetComponent<Arrow>().Initialize(player, baseAttack, -90, true);
        GameObject arrow3 = Instantiate(arrowPrefabs, player.transform.position, Quaternion.identity) as GameObject;
        arrow3.GetComponent<Arrow>().Initialize(player, baseAttack, -105, true);
        Destroy(gameObject);
    }
}