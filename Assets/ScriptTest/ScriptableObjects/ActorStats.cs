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
    public Sprite actorSprite;
    public Sprite actorFace;
    public int level;

    [Space]
    [Header("Controller")]
    public int actionSight;
    public int actionCost;
}
