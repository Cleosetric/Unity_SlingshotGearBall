using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 10;
    [Space]
    public float upForce = 1f;
    public float sideForce = 0.045f;
    private Rigidbody2D rb;
    private Transform actor;
    private GameManager gm;

    [SerializeField]
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        Invoke("SetSpawned", 0.75f);
    }

    public void CoinSpawn(Vector2 spawnPos){
        rb = GetComponent<Rigidbody2D>();
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
        if(isSpawning && CoinInActorDistance()){
            MoveCoinToPlayer();
        }
    }

    void MoveCoinToPlayer()
    {
        Actor leader = Party.Instance.GetLeader();
        if(leader != null){
            Vector3 moveVector  = (leader.parent.position - transform.position).normalized;
            rb.MovePosition(transform.position + moveVector * Time.unscaledDeltaTime * 10f);

            if (Vector3.Distance(leader.parent.position, transform.position) < 0.5f)
            {
                gm.IncreaseCoin(coinValue);
                gameObject.SetActive(false);
                isSpawning = false;
            }
        }
    }
    
    private bool CoinInActorDistance(){
        Actor leader = Party.Instance.GetLeader();
        if(leader != null){
            if (Vector3.Distance(leader.parent.position, transform.position) < 2f)
            {
                return true;
            }
        }
        return false;
    }
}
