using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    [SerializeField] private int durability;
    [Range(1, 10)]
    [SerializeField] private int coinLoot = 3;
    
    private Slider slider;
    private GameObject slideObject;
    private Animator anim;
    private ParticleSystem ps;
    private SpriteRenderer sprite;
    private BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        slider = GetComponentInChildren<Slider>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        col = GetComponent<BoxCollider2D>();

        slideObject = slider.gameObject;
        slider.maxValue = durability;
        slider.value = durability;
        slideObject.SetActive(false);
    }

    // private void Update() {
    //     SpawnPoolCoin(10);
    // }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     if(other.collider.tag == "Player"){
    //         OnDamage(1);
    //     }
    // }

    public void ApplyDamage(int value){
        Debug.Log("Box Damaged!");
        durability -= value;
        slider.value = durability;
        anim.SetTrigger("GetHit");
        CheckDurability();
    }

    void CheckDurability(){
        if(durability <= 0){
            StopCoroutine("ShowDurabilityUI");
            SpawnPoolCoin(Random.Range(Mathf.Max(coinLoot), 1));
            HideBox();
        }else{
            StartCoroutine(ShowDurabilityUI(2f));
        }
    }

    void HideBox(){
        slideObject.SetActive(false);
        col.enabled = false;
        sprite.enabled = false;
        ps.Play();
        Destroy(this.gameObject, 2f);
    }

    void SpawnPoolCoin(int total)
    {
        for (int i = 0; i < total; i++)
        {
            GameObject coin = ObjectPooling.Instance.GetPooledObject("Coin");
            if(coin != null){
                coin.GetComponent<Coin>().CoinInit();
                coin.GetComponent<Coin>().CoinSpawn(transform.position);
            }
        }
    }

    private IEnumerator ShowDurabilityUI(float slowdownTime){
		slideObject.SetActive(true);
		yield return new WaitForSecondsRealtime(slowdownTime);
		slideObject.SetActive(false);
	}
}
