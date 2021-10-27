using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitAreaType { circle, box}

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Collider Ability", order = 3)]
public class ColliderAbility : Ability
{
    [Space]
    [Header("Collider Effect")]
    public float repeatTime;
    public float repeatDelay;
    [Space]
    public float range;
    public float hitForce;
    public Vector3 positionOffset;
    public GameObject abilityEffect;

    private ColliderAbilityTrigger colliderAbility;
    private GameObject origin;
    
    public override void Initialize(GameObject parent){
        base.Initialize(parent);
        this.origin = parent;
        colliderAbility = parent.GetComponent<ColliderAbilityTrigger>();
    }

    public override void Activate(){
        base.Activate();
        colliderAbility.Initialize(this, actor, origin.transform);
        colliderAbility.Active();
    }
    
    public override void Deactivate(){
        base.Deactivate();
    }
}
