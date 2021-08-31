using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Event
    public static event Action<int> OnComboCounter = delegate { };

    //Reference 
    private HeroBase heroData;
    private PlayerControl control;
    private PlayerUI playerUI;

    //Player Param
    public String name;
    public int mhp;
    public int hp;
    public float stamina;
    public float mstamina;
    public Sprite spriteFace;
    
    private int attack;
    private int defense;
    private int speed;
    private int controlRange;
    private int energyToSling;

    //Player Monobehaviour
    private SpriteRenderer spriteChar;
    private Rigidbody2D rb;
    private Vector2 lastFrameVelocity;

    
    private float timer;
    [SerializeField] private float regenStamina = 10f;
    [SerializeField] private float regenCooldown = 2f;


    // Start is called before the first frame update
    void Start()
    {
        heroData = PartyManager.Instance.GetActiveHero();
        control = FindObjectOfType<PlayerControl>();
        playerUI = FindObjectOfType<PlayerUI>();
        spriteChar = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        RefreshHero();
    }

    public void LoadData(HeroBase hero){
        name = hero.name;
        hp = hero.hp;
        stamina = hero.stamina;
        mhp = hero.maxHp;
        mstamina = hero.maxStamina;

        attack = hero.attack;
        defense = hero.defense;
        speed = hero.speed;

        controlRange = hero.range;
        energyToSling = hero.actionCost;

        spriteChar.sprite = hero.heroSprite;
        spriteFace = hero.heroFace;

        control.powerShoot = hero.speed;
        control.maxDistance = hero.range;

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
        stamina -= energyToSling;
        if(stamina < 0){
            stamina = 0;
        }
        timer = regenCooldown;
        updateHero();
    }

    public void OnDamage(int damage){
        hp -= damage;
        if(hp < 0){
            hp = 0;
        }
        updateHero();
    }

    public bool IsSlingable(){
        if(stamina >= energyToSling){
            return true;
        }
        return false;
    }

    public void DoAttack(){
        if(heroData != null){
            heroData.Attack();
        }
    }

    public void DoSkill(){
        if(heroData != null){
            heroData.Skill();
        }
    }

    public void DoUltimate(){
        if(heroData != null){
            heroData.Ultimate();
        }
    }

    void RegenStamina(float energy){
        stamina = stamina + (Mathf.Round(energy * Time.deltaTime));
        if(stamina >= mstamina){
            stamina = mstamina;
        }
        updateHero();
    }

    // Update is called once per frame
    void Update()
    {
        lastFrameVelocity = rb.velocity;

        if(stamina < mstamina){
            timer -= Time.deltaTime;
            if(timer <= 0f){
                RegenStamina(regenStamina);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D hit) {
        float impulse = calculateImpulse(hit);

        if(hit.collider.tag == "Enemy" && impulse > 30f){ 
            TimeManager.Instance.StartImpactMotion();
            EnemyBall enemy = hit.gameObject.GetComponent<EnemyBall>();
            enemy.DoDamage(1);
            OnComboCounter(1);
            DoAttack();
        }

        Vector2 inNormal = hit.contacts[0].normal;		
        var lastSpeed = lastFrameVelocity.magnitude;
        var direction = Vector2.Reflect(lastFrameVelocity.normalized, inNormal);
        rb.velocity = direction * Mathf.Max(lastSpeed, 5f);
    }

    private float calculateImpulse(Collision2D col){
        float impact = 0f;
        foreach (ContactPoint2D cp in col.contacts) {
                impact += cp.normalImpulse;
        }
        return impact;
    }

    
}
