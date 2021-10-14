using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charachter", menuName = "Charachter/New Mob", order = 1)]
public class MobStats : CharachterStats
{
    [Space]
    [Header("Mob Info")]
    public new string name;
    public string description;
    public Sprite mobSprite;

    [Space]
    [Header("Movement")]
    public float radiusSight;
    public float attackSight;
}
