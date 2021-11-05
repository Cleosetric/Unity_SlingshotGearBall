using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Collectibles/AGI Collectible", order = 4)]
public class CollectibleAGI : Collectibles
{
    public float buffAgi;
    public float effectiveTime;

    public override void ApplyEffect(Transform parent)
    {
        string bonusText = "+"+buffAgi+" AGI";
        HitCounter.Instance.AddDamagePopup(parent, 4, bonusText, "Swiftness");

        CollectiblesManager.Instance.AddBuffAGI((int)target, effectiveTime, buffAgi);
    }
    
}
