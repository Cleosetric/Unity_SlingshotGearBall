using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Inventory/New Equip", order = 1)]
public class Equipment : Items
{
    public EquipmentSlot equipSlot;
    public int modMHP;
    public int modMSP;
    public int modATK;
    public int modDEF;
    public int modAGI;
    public int modHRG;
    public int modSRG;

    public float modHIT;
    public float modCRI;
    public float modHRR;
    public float modSRR;

    public override void Use()
    {
        base.Use();
        EquipManager.Instance.Equip(this);
        RemoveItem();
    }

}

public enum EquipmentSlot {Weapon,Armor,Boots,Necklace,Ring,Earing}