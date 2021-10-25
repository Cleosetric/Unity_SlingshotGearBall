using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Collider Ability", order = 3)]
public class ColliderAbility : Ability
{
    [Space]
    [Header("Collider Effect")]
    public float range;
    public float hitForce;

    private ColliderAbilityTrigger colliderAbility;
    
    public override void Initialize(GameObject parent){
        base.Initialize(parent);
        colliderAbility = parent.GetComponent<ColliderAbilityTrigger>();
        colliderAbility.Initialize(actor, parent.transform, range, hitForce);
    }

    public override void Activate(){
        base.Activate();
        colliderAbility.Active();
    }
    
    public override void Deactivate(){
        base.Deactivate();
    }
}
