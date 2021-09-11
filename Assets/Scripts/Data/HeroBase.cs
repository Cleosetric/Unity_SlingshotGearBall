using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Melee,
    Range,
    Special
}

public class HeroBase : ScriptableObject {

    [Header("Charachter Info")]
    public new string name;
    public string description;
    public HeroType type;
    public Sprite heroSprite;
    public Sprite heroFace;

    [Space]
    [Header("Base Parameter")]
    public int maxHp = 5;
    public int hp;
    public float maxStamina = 100;
    public float stamina;
    [Space]
    public int attack = 1;
    public int defense = 1;
    public float speed = 10f;
    public float range = 0.5f;

    [Space]
    [Header("Charachter Progress")]
    public int level = 1;
    public int exp = 1;

    [Space]
    [Header("Control Parameter")]
    public int sight = 10;
    public int actionCost = 10;
    public RuntimeAnimatorController animCon;

    public void OnEnable() {
        // Debug.Log("Awaken My Master! " + name);
        hp = maxHp;
        stamina = maxStamina;
    }

    public virtual void Attack(GameObject obj){
        Debug.Log("Attack");
    }

    public virtual void DashAttack(GameObject obj){
        Debug.Log("Dash Attack");
    }

    public virtual void Skill(GameObject obj){
        Debug.Log("Skill");
    }

    public virtual void Ultimate(GameObject obj){
        Debug.Log("Ultimate");
    }

}
