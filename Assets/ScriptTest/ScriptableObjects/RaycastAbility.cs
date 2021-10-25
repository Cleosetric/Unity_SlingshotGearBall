using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Raycast Ability", order = 1)]
public class RaycastAbility : Ability
{
    [Space]
    [Header("Raycast Effect")]
    public TargetAttack target;
    public float range;
    public float hitForce;

    private RaycastAbilityTrigger raycastAbility;

    public override void Initialize(GameObject parent)
    {
        base.Initialize(parent);
        raycastAbility = parent.GetComponent<RaycastAbilityTrigger>();
        raycastAbility.Initialize(actor,range,hitForce, target);
    }

    public override void Activate()
    {
        base.Activate();
        raycastAbility.Active();
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
    }
}
