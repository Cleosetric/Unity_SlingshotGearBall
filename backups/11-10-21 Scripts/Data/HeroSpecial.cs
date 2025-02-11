﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Special", order = 0)]
public class HeroSpecial : HeroBase
{
    [Space]
    [Header("Skills Prefab")]
    public GameObject baseAttack;
    public GameObject baseDashAttack;
    public GameObject baseSkill;
    public GameObject baseUltimate;

    public override void Attack(GameObject obj){
        Vector3 pos = obj.transform.position;
        GameObject attackPrefab = Instantiate(baseAttack, pos, Quaternion.identity);
        attackPrefab.transform.SetParent(obj.transform);
        attackPrefab.GetComponent<Magic1>().InitializeSkill(attack);
    }
    
    public override void DashAttack(GameObject obj){
        
    }
    
    public override void Skill(GameObject obj){
    //    base.Skill();
    }
    
    public override void Ultimate(GameObject obj){
    //    base.Ultimate();
    }
}
