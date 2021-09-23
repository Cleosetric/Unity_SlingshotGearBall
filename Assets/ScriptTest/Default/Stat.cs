using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] int baseValue;
    private List<int> modifiers = new List<int>();
    
    public int getValue(){
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void SetValue(int value){
        baseValue = value;
    }

    public void AddModifier(int modifier){
        if(modifier != 0) modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier){
        if(modifier != 0) modifiers.Remove(modifier);
    }
}
