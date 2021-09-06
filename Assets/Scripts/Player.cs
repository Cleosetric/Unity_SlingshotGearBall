using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Event

    //Reference 
    private HeroBase heroData;
    private PlayerControl control;
    private PlayerUI playerUI;

    //Player Param
    public Sprite spriteFace;
    public String name;
    public int maxHP;
    public float maxStamina;
    public int hp;
    public float stamina;
    private int attack;
    private int defense;
    private float speed;
    private int actionCost;

    //Player Monobehaviour
    private SpriteRenderer spriteChar;
    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;
    private Animator animator;

    //regen stamina
    private WaitForSeconds regenTick = new WaitForSeconds(0.25f);
    private Coroutine regen;
    [SerializeField] private float regenStamina = 10f;
    [SerializeField] private float regenCooldown = 2f;


    // Start is called before the first frame update
    void Start()
    {
        heroData = PartyManager.Instance.GetActiveHero();
        control = FindObjectOfType<PlayerControl>();
        playerUI = FindObjectOfType<PlayerUI>();
        spriteChar = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        RefreshHero();
    }

    public void LoadData(HeroBase hero){
        name = hero.name;
        hp = hero.hp;
        stamina = hero.stamina;
        maxHP = hero.maxHp;
        maxStamina = hero.maxStamina;
        attack = hero.attack;
        defense = hero.defense;
        speed = hero.speed;
        actionCost = hero.actionCost;

        spriteChar.sprite = hero.heroSprite;
        spriteFace = hero.heroFace;

        control.powerShoot = hero.speed;
        control.maxDistance = hero.sight;
        
        animator.runtimeAnimatorController = hero.animCon;

        playerUI.DisplayUI(this);
    }
    
    public void RefreshHero(){
        heroData = PartyManager.Instance.GetActiveHero();
        if(heroData != null){
         LoadData(heroData);
        }
    }

    public void updateHero(){
        heroData = PartyManager.Instance.GetActiveHero();
        heroData.hp = hp;
        heroData.stamina = stamina;
        playerUI.DisplayUI(this);
    }

    public void OnSlingshot(){
        stamina -= actionCost;
        if(stamina < 0){
            stamina = 0;
        }

        StartRegen();
        updateHero();
    }

    public void StartRegen(){
        if(regen != null) StopCoroutine(regen);
        regen = StartCoroutine(RegenerateStamina());
    }

    public void OnDamage(int damage){
        hp -= damage;
        if(hp < 0){
            hp = 0;
        }
        updateHero();
    }

    public bool IsSlingable(){
        if(stamina >= actionCost){
            return true;
        }
        return false;
    }

    public void DoAttack(){
        if(heroData != null){
            animator.SetTrigger("Attack");
            heroData.Attack(this.gameObject);
        }
    }

    public void DoSkill(){
        if(heroData != null && stamina >= actionCost){
            animator.SetTrigger("Attack");
            heroData.Skill(this.gameObject);
            OnSlingshot();
        }
    }

    public void DoUltimate(){
        if(heroData != null &&  stamina >= actionCost * 4){
            stamina -= (actionCost * 4);
            heroData.Ultimate(this.gameObject);
            updateHero();
            if(regen != null) StopCoroutine(regen);
            Invoke("StartRegen", 5f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D hit) {
        float impulse = calculateImpulse(hit);

        if(hit.collider.tag == "Enemy" && impulse > 10f){ 
            TimeManager.Instance.StartImpactMotion();
            DoAttack();
        }
        
        if(hit.collider.tag == "Props"){
            hit.collider.GetComponent<Box>().ApplyDamage(attack);
        }

        Vector2 inNormal = hit.contacts[0].normal;
        var direction = Vector2.Reflect(lastFrameVelocity.normalized, inNormal);
        rb.velocity = direction * Mathf.Max(lastFrameVelocity.magnitude, 5f);
    }

    private float calculateImpulse(Collision2D col){
        float impact = 0f;
        foreach (ContactPoint2D cp in col.contacts) {
                impact += cp.normalImpulse;
        }
        return impact;
    }

    private IEnumerator RegenerateStamina(){
        yield return new WaitForSeconds(regenCooldown);

        while (stamina < maxStamina)
        {
            stamina += regenStamina;
            updateHero();
            yield return regenTick;
        }
        regen = null;
    }

    
}
