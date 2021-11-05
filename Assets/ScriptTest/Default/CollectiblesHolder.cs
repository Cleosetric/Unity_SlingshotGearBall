using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesHolder : MonoBehaviour
{
    private Collectibles collectibles;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool isSpawning = false;

    public void Initialize(Collectibles collectibles){
        this.collectibles = collectibles;
        Refresh();
    }

    private void Refresh() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = collectibles.sprite;
        Invoke("SetSpawned", 0.75f);
    }

    void SetSpawned(){
        isSpawning = true;
    }

    private void Update() {
        if(isSpawning && CoinInActorDistance()){
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        Actor leader = Party.Instance.GetLeader();
        if(leader != null){
            Vector3 moveVector  = (leader.parent.position - transform.position).normalized;
            rb.MovePosition(transform.position + moveVector * Time.unscaledDeltaTime * 15f);

            if (Vector3.Distance(leader.parent.position, transform.position) < 0.5f)
            {
                collectibles.ApplyEffect(leader.parent.transform);
                Destroy(gameObject);
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
