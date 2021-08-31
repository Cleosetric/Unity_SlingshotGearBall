using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Melee", order = 0)]
public class HeroMelee : HeroBase
{
   
    public override void Attack(){
        Debug.Log("MeleeAttack");
    }
    
    public override void Skill(){
       base.Skill();
    }
    
    public override void Ultimate(){
       base.Ultimate();
    }
}
