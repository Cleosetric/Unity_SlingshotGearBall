using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Special", order = 0)]
public class HeroSpecial : HeroBase
{
    public override void Attack(){
        Debug.Log("Special Attack");
    }
    
    public override void Skill(){
       base.Skill();
    }
    
    public override void Ultimate(){
       base.Ultimate();
    }
}
