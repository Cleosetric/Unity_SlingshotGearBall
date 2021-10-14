using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Inventory/New Equip", order = 1)]
public class Equipment : Items
{
    public EquipmentSlot equipSlot;
    public ActorClass equipClass;
    public StatModifier modMHP;
    public StatModifier modMSP;
    public StatModifier modATK;
    public StatModifier modDEF;
    public StatModifier modMATK;
    public StatModifier modMDEF;
    public StatModifier modAGI;
    public StatModifier modLUK;
    public StatModifier modHRG;
    public StatModifier modSRG;

    public StatModifier modHIT;
    public StatModifier modCRI;
    public StatModifier modEVA;
    public StatModifier modHRR;
    public StatModifier modSRR;

    public override void Use()
    {
        base.Use();
        ItemHelpUI.Instance.ShowUI(this);
        // EquipManager.Instance.Equip(this);
        // RemoveItem();
    }

}

public enum ActorClass {None, Knight, Archer, Mage, Lance, Gunner, Summoner}

public enum EquipmentSlot {Weapon,Armor,Boots,Helmet,Necklace,Ring}