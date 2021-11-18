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
    [Header("Drop Info")] 
    public int score = 50;
    public int experience = 20;
    [Range(1, 100)] public int coin;
    public List<EquipmentHolder> equip = new List<EquipmentHolder>();
    [Range(1, 100)] public int equipChance;

    [Space]
    [Header("Movement")]
    public float radiusSight;
    public float attackSight;
    public float attackTime = 1f;
    public float attackRate = 2f;
}
