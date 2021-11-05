using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Collectibles/ATK Collectible", order = 2)]
public class CollectibleATK : Collectibles
{
    public float buffATK;
    public float effectiveTime;

    public override void ApplyEffect(Transform parent)
    {
        string bonusText = "+"+buffATK+" ATK";
        HitCounter.Instance.AddDamagePopup(parent, 4, bonusText, "Strength");

        CollectiblesManager.Instance.AddBuffATK((int)target, effectiveTime, buffATK);
    }
    
}
