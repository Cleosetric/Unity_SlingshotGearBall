using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charachter : MonoBehaviour, IDamagable
{
    [Space]
    [Header("Base Parameter")]
    [Tooltip("Max Health Point")] public Stat statMHP;
    [Tooltip("Max Stamina Point")] public Stat statMSP;
    [Tooltip("Base Attack")] public Stat statATK;
    [Tooltip("Base Defense")] public Stat statDEF;
    [Tooltip("Base Agility")] public Stat statAGI;
    [Tooltip("Health Point Regeneration")] public Stat statHRG;
    [Tooltip("Stamina Point Regeneration")] public Stat statSRG;

    [Space]
    [Header("Ex Parameter")]
    [Tooltip("Hit Rate")] public Stat statHIT;
    [Tooltip("Critical Rate")] public Stat statCRI;
    [Tooltip("Critical Rate")] public Stat statEVA;
    [Tooltip("Health Regeneration Rate")] public Stat statHRR;
    [Tooltip("Stamina Regeneration Rate")] public Stat statSRR;

    public virtual void ApplyDamage(int damage)
    {
        //Called When Damage Applied
    }

    public virtual void ApplyAction(int energy){
        //Called When Doing an Action
    }
}
