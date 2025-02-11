using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType{
    flat,
    percent,
}

[System.Serializable]
public class StatModifier 
{
    public float value;
    public StatModType type;
    public readonly int order;

    public StatModifier(float value, StatModType type){
        this.value = value;
        this.type = type;
    }
}
