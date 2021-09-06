using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Special", order = 0)]
public class HeroSpecial : HeroBase
{
    public override void Attack(GameObject obj){
        Debug.Log("Special Attack");
    }
    
    public override void Skill(GameObject obj){
    //    base.Skill();
    }
    
    public override void Ultimate(GameObject obj){
    //    base.Ultimate();
    }
}
