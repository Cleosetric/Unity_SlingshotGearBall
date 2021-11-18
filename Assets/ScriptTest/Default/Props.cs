using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Props : MonoBehaviour
{

    [SerializeField] private int durability;
    [SerializeField] private int score = 10;
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

    public virtual void ApplyDamage(int value){
        durability -= value;
        slider.value = durability;
        anim.SetTrigger("GetHit");
        CheckDurability();
    }

    void CheckDurability(){
        if(durability <= 0){
            StopCoroutine("ShowDurabilityUI");
            OnDestroyedProps();
        }else{
            StartCoroutine(ShowDurabilityUI(2f));
        }
    }

    protected virtual void OnDestroyedProps(){
        SoundManager.Instance.Play("Break");
        HideBox();
        ScoreCounter.Instance.IncreaseScore(score);
    }
    
    void HideBox(){
        slideObject.SetActive(false);
        col.enabled = false;
        sprite.enabled = false;
        ps.Play();
        Destroy(this.gameObject, ps.main.duration);
    }

    private IEnumerator ShowDurabilityUI(float slowdownTime){
		slideObject.SetActive(true);
		yield return new WaitForSecondsRealtime(slowdownTime);
		slideObject.SetActive(false);
	}
}
