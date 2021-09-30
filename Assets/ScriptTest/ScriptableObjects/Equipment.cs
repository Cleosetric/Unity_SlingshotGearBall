using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Inventory/New Equip", order = 1)]
public class Equipment : Items
{
    public EquipmentSlot equipSlot;
    public ActorClass equipClass;
    public int modMHP;
    public int modMSP;
    public int modATK;
    public int modDEF;
    public int modAGI;
    public int modHRG;
    public int modSRG;

    public int modHIT;
    public int modCRI;
    public int modEVA;
    public int modHRR;
    public int modSRR;

    public override void Use()
    {
        base.Use();
        ItemHelpUI.Instance.ShowUI(this);
        // EquipManager.Instance.Equip(this);
        // RemoveItem();
    }

}

public enum ActorClass {None, Knight, Archer, Mage, Lance, Gunner, Summoner}

public enum EquipmentSlot {Weapon,Armor,Boots,Necklace,Ring,Earing}