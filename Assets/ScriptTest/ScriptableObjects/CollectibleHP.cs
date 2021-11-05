using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Collectibles/HP Collectible", order = 0)]
public class CollectibleHP : Collectibles
{
    public int restoreHP;

    public override void ApplyEffect(Transform parent){
        string bonusText = "+"+restoreHP+" HP";
        HitCounter.Instance.AddDamagePopup(parent, 4, bonusText, "Healing");

        if((int)target == 0){
            Party.Instance.GetLeader().ApplyHeal(restoreHP);
        }else{
            foreach (Actor actor in Party.Instance.actors)
            {
                if(actor != null && actor.isAlive){
                    actor.ApplyHeal(restoreHP);
                }
            }
        }
    }
}
