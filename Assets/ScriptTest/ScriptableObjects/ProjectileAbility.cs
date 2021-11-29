using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType {destroyAll,destroyOnlyEnemy,pierceAll,pierceOnlyEnemy}
public enum Direction {one, two, three, radial}

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Projectile Ability", order = 2)]
public class ProjectileAbility : Ability
{
    [Space]
    [Header("Projectile Effect")]
    public TargetAttack target;
    public float projectileSpeed;
    public float projectileLife;
    public float projectileImpact;
    public int projectileCount;
    public float projectileDelay;
    public HitType hitType;
    public Direction direction;
    public Projectile projectile;

    private ProjectileAbilityTrigger projectileAbility;
    private GameObject origin;

    public override void Initialize(GameObject parent){
        this.origin = parent;
        base.Initialize(parent);
        projectileAbility = parent.GetComponent<ProjectileAbilityTrigger>();
    }

    public override void Activate(){
        base.Activate();
        // string bonusText = "+"+bonusValue.ToString() +" "+abilityBonus.ToString().ToUpper();
        projectileAbility.Initialize(this, projectile, actor.transform, projectileSpeed, 
        projectileLife, projectileImpact, projectileCount, projectileDelay, target, chantType, hitType,
        direction, chantAnimPrefab);

        projectileAbility.Active();
    }

    public override void Deactivate(){
        base.Deactivate();
    }  
}
