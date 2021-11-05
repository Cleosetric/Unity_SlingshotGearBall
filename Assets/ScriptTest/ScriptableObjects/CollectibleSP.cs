using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Collectibles/SP Collectible", order = 1)]
public class CollectibleSP : Collectibles
{
    public int restoreSP;

    public override void ApplyEffect(Transform parent){
        string bonusText = "+"+restoreSP+" SP";
        HitCounter.Instance.AddDamagePopup(parent, 4, bonusText, "Energized");

        if((int)target == 0){
            Party.Instance.GetLeader().ApplyEnergy(restoreSP);
        }else{
            foreach (Actor actor in Party.Instance.actors)
            {
                if(actor != null && actor.isAlive){
                    actor.ApplyEnergy(restoreSP);
                }
            }
        }
    }
}
