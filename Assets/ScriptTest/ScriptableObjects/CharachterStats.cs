using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterStats : ScriptableObject
{
    [Space]
    [Header("Base Parameter")]
    [Tooltip("Max Health Point")] public Stat MHP;
    [Tooltip("Max Stamina Point")] public Stat MSP;
    [Tooltip("Base Attack")] public Stat ATK;
    [Tooltip("Base Defense")] public Stat DEF;
    [Tooltip("Base Agility")] public Stat AGI;
    [Tooltip("Health Point Regeneration")] public Stat HRG;
    [Tooltip("Stamina Point Regeneration")] public Stat SRG;

    [Space]
    [Header("Ex Parameter")]
    [Tooltip("Hit Rate")] public Stat HIT;
    [Tooltip("Critical Rate")] public Stat CRI;
    [Tooltip("Critical Rate")] public Stat EVA;
    [Tooltip("Health Regeneration Rate")] public Stat HRR;
    [Tooltip("Stamina Regeneration Rate")] public Stat SRR;

}
