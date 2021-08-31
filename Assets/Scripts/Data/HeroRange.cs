using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Range", order = 0)]
public class HeroRange : HeroBase
{
    public override void Attack(){
        Debug.Log("Range Attack");
    }
    
    public override void Skill(){
       base.Skill();
    }
    
    public override void Ultimate(){
       base.Ultimate();
    }
}
