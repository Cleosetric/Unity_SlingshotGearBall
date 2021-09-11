using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float upForce = 1f;
    public float sideForce = 0.03f;
    private Rigidbody2D rb;
    private Transform player;

    [SerializeField]
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().transform;
    }

    public void CoinInit(){
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().transform;
    }

    public void CoinSpawn(Vector2 spawnPos){
        gameObject.SetActive(true);
        transform.position = spawnPos;

        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce, upForce);
        Vector2 force = new Vector2(xForce *4, yForce * 4);

        rb.velocity = force;
        isSpawning = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isSpawning) Invoke("MoveCoinToPlayer", 1.25f);

        // if(CoinInDistance()){
        //     gameObject.SetActive(false);
        //     isSpawning = false;
        // }
    }

    void MoveCoinToPlayer()
    {
        Vector3 moveVector  = (player.position - transform.position);
        moveVector.Normalize();
        rb.MovePosition(transform.position + moveVector * Time.unscaledDeltaTime * 10f);

        if(CoinInDistance()){
            gameObject.SetActive(false);
            isSpawning = false;
        }
    }
    
    private bool CoinInDistance(){
        if (Vector3.Distance(player.position, transform.position) < 1f)
        {
            return true;
        }
        return false;
    }
}
