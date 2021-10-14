using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statf
{
    [SerializeField] float baseValue;
    private List<StatModifier> modifiers = new List<StatModifier>();

    public float GetValue(){
        return CalculateFinalValue();
    }

    public void SetValue(float value){
        baseValue = value;
    }

    public void AddModifier(StatModifier modifier){
        if(modifier.value != 0){
            modifiers.Add(modifier);
            // modifiers.Sort(CompareModifierorder);
        }
    }

    public void RemoveModifier(StatModifier modifier){
        if(modifier.value != 0) 
            modifiers.Remove(modifier);
    }

    private float CalculateFinalValue()
    {
        float finalValue = baseValue;

        for (int i = 0; i < modifiers.Count; i++)
        {
            StatModifier mod = modifiers[i];

            if (mod.type == StatModType.flat)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModType.percent)
            {
                finalValue *= 1 + mod.value;
            }
        }
        // Rounding gets around float calculation errors (like getting 12.0001f, instead of 12f)
        // 4 digits is usually precise enough, but feel free to change this to fit your needs
        return (float)Mathf.Round(finalValue);
    }

    private int CompareModifierorder(StatModifier a, StatModifier b)
    {
        if (a.order < b.order)
            return -1;
        else if (a.order > b.order)
            return 1;
        return 0; // if (a.order == b.order)
    }
}
