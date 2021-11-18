using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{
    public Items dropableItem;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = dropableItem.icon;
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
                SoundManager.Instance.Play("EquipDrop");
                InventoryManager.Instance.Add(dropableItem);
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
