using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Collectibles/DEF Collectible", order = 3)]
public class CollectibleDEF : Collectibles
{
    public float buffDef;
    public float effectiveTime;

    public override void ApplyEffect(Transform parent)
    {
        string bonusText = "+"+buffDef+" DEF";
        HitCounter.Instance.AddDamagePopup(parent, 4, bonusText, "Toughness");

        CollectiblesManager.Instance.AddBuffDEF((int)target, effectiveTime, buffDef);
    }
    
}
