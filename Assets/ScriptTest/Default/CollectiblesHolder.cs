using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesHolder : MonoBehaviour
{
    public Collectibles collectibles;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool isSpawning = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Refresh();
    }

    // public void Initialize(Collectibles collectibles){
    //     this.collectibles = collectibles;
    //     Refresh();
    // }

    private void Refresh() {
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
            Vector3 moveVector  = (leader.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + moveVector * Time.unscaledDeltaTime * 15f);

            if (Vector3.Distance(leader.transform.position, transform.position) < 0.5f)
            {
                SoundManager.Instance.Play("Collectible");
                collectibles.ApplyEffect(leader.transform.transform);
                Destroy(gameObject);
            }
        }
    }
    
    private bool CoinInActorDistance(){
        if(Party.Instance.actors.Count > 0 ){
            Actor leader = Party.Instance.GetLeader();
            if(leader != null){
                if (Vector3.Distance(leader.transform.position, transform.position) < 2f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
