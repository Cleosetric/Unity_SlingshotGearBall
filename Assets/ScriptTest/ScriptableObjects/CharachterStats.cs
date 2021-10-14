using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterStats : ScriptableObject
{
    [Space]
    [Header("Base Parameter")]
    [Tooltip("Max Health Point")] public Statf statMHP;
    [Tooltip("Max Stamina Point")] public Statf statMSP;
    [Tooltip("Base Attack")] public Statf statATK;
    [Tooltip("Base Defense")] public Statf statDEF;
    [Tooltip("Base Magic Attack")] public Statf statMATK;
    [Tooltip("Base Magic Defense")] public Statf statMDEF;
    
    [Tooltip("Base Agility")] public Statf statAGI;
    [Tooltip("Base Luck")] public Statf statLUK;
    [Tooltip("Health Point Regeneration")] public Statf statHRG;
    [Tooltip("Stamina Point Regeneration")] public Statf statSRG;

    [Space]
    [Header("Ex Parameter")]
    [Tooltip("Hit Rate")] public Statf statHIT;
    [Tooltip("Critical Rate")] public Statf statCRI;
    [Tooltip("Critical Rate")] public Statf statEVA;
    [Tooltip("Health Regeneration Rate")] public Statf statHRR;
    [Tooltip("Stamina Regeneration Rate")] public Statf statSRR;

}
