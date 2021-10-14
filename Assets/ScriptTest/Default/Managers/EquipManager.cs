using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    #region Singleton
	private static EquipManager _instance;
	public static EquipManager Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
	#endregion

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    InventoryManager inventory;
    Party party;
    // public Equipment[] currentEquipment;

    // Start is called before the first frame update
    void Start()
    {
        inventory = InventoryManager.Instance;
        party = Party.Instance;
        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        // currentEquipment = new Equipment[numSlot];
    }

    public void Equip(Equipment newItem){

        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        if(party.GetActiveActor().equipment[slotIndex] != null){
            if(inventory.items.Count < inventory.maxSlot){ 
                oldItem = party.GetActiveActor().equipment[slotIndex];
                inventory.Add(oldItem);
                
                party.GetActiveActor().OnItemUnequiped(oldItem);
                party.GetActiveActor().equipment[slotIndex] = newItem;
                party.GetActiveActor().OnItemEquiped(newItem);
            }
        }else{
            party.GetActiveActor().equipment[slotIndex] = newItem;
            party.GetActiveActor().OnItemEquiped(newItem);
        }

        // if(party.GetActiveActor().equipment[slotIndex] == null && party.GetActiveActor().equipment[slotIndex] != newItem)
           

        if(onEquipmentChanged != null) 
            onEquipmentChanged.Invoke(newItem, oldItem);
    }

    public void Unequip(int slotIndex){
        if(inventory.items.Count < inventory.maxSlot){
            if(party.GetActiveActor().equipment[slotIndex] != null){
                Equipment oldItem = party.GetActiveActor().equipment[slotIndex];
                inventory.Add(oldItem);
                party.GetActiveActor().equipment[slotIndex] = null;
                party.GetActiveActor().OnItemUnequiped(oldItem);
                if(onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll(){
        for (int i = 0; i < party.GetActiveActor().equipment.Length; i++)
        {
            Unequip(i);
        }
    }

    public void RecomendedEquip(){
        for (int i = inventory.items.Count - 1; i >= 0; i--)
        {
            if(inventory.items[i] is Equipment){
                Equipment equip = inventory.items[i] as Equipment;
                // Debug.Log("Index "+ i + " | equip "+equip.name);
                if((int)equip.equipClass == 0 || (int)equip.equipClass == (int)party.GetActiveActor().actorClass){
                    Debug.Log("Index "+ i + " | "+equip.name + " Succesfuly Equiped");
                    Equip(equip);
                    equip.RemoveItem();
                    // continue;
                }
            }
        }
        ItemHelpUI.Instance.ResetUI();
    }
}
