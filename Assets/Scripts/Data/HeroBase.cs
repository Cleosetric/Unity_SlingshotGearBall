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

    public new string name;
    public string description;
    public HeroType type;
    public Sprite heroSprite;
    public Sprite heroFace;
    public int maxHp = 5;
    public int hp;
    public float maxStamina = 100;
    public float stamina;
    public int attack = 1;
    public int defense = 1;
    public int speed = 10;
    public int level = 1;
    public int exp = 1;
    public int range = 10;
    public int actionCost = 10;

    public void OnEnable() {
        Debug.Log("Awaken My Master! " + name);
        hp = maxHp;
        stamina = maxStamina;
    }

    public virtual void Attack(){
        Debug.Log("Attack");
    }

    public virtual void Skill(){
        Debug.Log("Skill");
    }

    public virtual void Ultimate(){
        Debug.Log("Ultimate");
    }

}
