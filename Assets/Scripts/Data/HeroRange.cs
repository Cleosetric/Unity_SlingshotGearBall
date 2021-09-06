using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Range", order = 0)]
public class HeroRange : HeroBase
{
    public override void Attack(GameObject obj){
        Debug.Log("Range Attack");
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    }
    
    public override void Skill(GameObject obj){
    //    base.Skill();
    }
    
    public override void Ultimate(GameObject obj){
    //    base.Ultimate();
    }

    
}
