using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow4 : AttackPatterns
{
    public GameObject arrowPrefabs;
    public float speed;

    private Player player;
    private Rigidbody2D rb;
    private int baseAttack;

     public void InitializeSkill(int attack, float duration){
        Start();
        baseAttack = attack;

        StartCoroutine(HyperSpeed(new Vector2(1,0), duration, speed, rb));
        Invoke("UltimateOff", duration - 0.5f);
        InvokeRepeating("ShootBullet",0.25f, 0.1f);
        DestroySelf(gameObject, duration);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    void ShootBullet(){
        GameObject arrow2 = Instantiate(arrowPrefabs, player.transform.position, Quaternion.identity) as GameObject;
        arrow2.GetComponent<Arrow>().Initialize(player, baseAttack, -90, true);
        arrow2.transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    void UltimateOff(){
        PlayerControl.Instance.SetOffUltimate();
    }
}
