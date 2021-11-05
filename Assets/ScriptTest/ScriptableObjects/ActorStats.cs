using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charachter", menuName = "Charachter/New Actor", order = 0)]
public class ActorStats : CharachterStats
{
    [Space]
    [Header("Charachter")]
    public new string name;
    public string description;
    public ActorClass actorClass;
    public Sprite actorSprite;
    public Sprite actorFace;
    public float growthCurve;

    [Space]
    [Header("Abilities")]
    public List<Ability> abilities = new List<Ability>();
    public Projectile projectile;
    public float projectileLife;
    public float attackTime;
    public float attackRate;

    [Space]
    [Header("Controller")]
    public float actionSight;
    public int actionCost;
}
