using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float upForce = 1f;
    public float sideForce = 0.045f;
    private Rigidbody2D rb;
    private Transform actor;

    [SerializeField]
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        actor = Party.Instance.GetActiveActor().transform;
    }

    public void CoinInit(){
        rb = GetComponent<Rigidbody2D>();
        actor = Party.Instance.GetActiveActor().transform;
    }

    public void CoinSpawn(Vector2 spawnPos){
        gameObject.SetActive(true);
        transform.position = spawnPos;

        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce, upForce);
        Vector2 force = new Vector2(xForce *4, yForce * 4);

        rb.velocity = force;
        Invoke("SetSpawned", 0.75f);
    }

    void SetSpawned(){
        isSpawning = true;
    }

    private void Update() {
        if(isSpawning && CoinInDistance()){
            gameObject.SetActive(false);
            isSpawning = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isSpawning) Invoke("MoveCoinToPlayer", 3f);
    }

    void MoveCoinToPlayer()
    {
        Vector3 moveVector  = (actor.position - transform.position);
        moveVector.Normalize();
        rb.MovePosition(transform.position + moveVector * Time.unscaledDeltaTime * 10f);

        if(CoinInDistance()){
            gameObject.SetActive(false);
            isSpawning = false;
        }
    }
    
    private bool CoinInDistance(){
        if (Vector3.Distance(actor.position, transform.position) < 0.5f)
        {
            return true;
        }
        return false;
    }
}
