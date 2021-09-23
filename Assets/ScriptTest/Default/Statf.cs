using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statf
{
    [SerializeField] float baseValue;
    private List<float> modifiers = new List<float>();

    public float getValue(){
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void SetValue(float value){
        baseValue = value;
    }

    public void AddModifier(float modifier){
        if(modifier != 0) modifiers.Add(modifier);
    }

    public void RemoveModifier(float modifier){
        if(modifier != 0) modifiers.Remove(modifier);
    }
}
