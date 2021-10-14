using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    InventoryManager inventory;
    EquipManager equipment;
    Party party;

    public Transform slotParentInventory;
    public Transform slotParentEquipment;

    public GameObject inventoryUI;

    InventroySlotUI[] inventorySlotUI;
    EquipmentSlotUI[] equipmentSlotUI;

    // Start is called before the first frame update
    void Start()
    {
        inventory = InventoryManager.Instance;    
        equipment = EquipManager.Instance;    
        party = Party.Instance;    

        inventory.onItemChangedCallback += UpdateUI;
        Party.Instance.partyOnActorChanged += setEquipUI;
        equipment.onEquipmentChanged += UpdateEquipUI;

        inventorySlotUI = slotParentInventory.GetComponentsInChildren<InventroySlotUI>();
        equipmentSlotUI = slotParentEquipment.GetComponentsInChildren<EquipmentSlotUI>();
    }

    void UpdateUI()
    {
        for (int i = 0; i < inventorySlotUI.Length; i++)
        {
            if(i < inventory.items.Count){
                inventorySlotUI[i].AddItem(inventory.items[i]);
            }else{
                inventorySlotUI[i].ClearSlot();
            }
        }
    }

    void setEquipUI(){
        Equipment[] equips = party.GetActiveActor().equipment;
        for (int i = 0; i < equips.Length; i++)
        {
            if(equips[i] != null){
                int slotIndex = (int)equips[i].equipSlot;
                equipmentSlotUI[slotIndex].AddItem(equips[i]);
            }else{
                equipmentSlotUI[i].ClearSlot();
            }
        }
    }

    void UpdateEquipUI(Equipment newItem, Equipment oldItem){
        if(newItem != null){
            int slotIndex = (int)newItem.equipSlot;
            equipmentSlotUI[slotIndex].AddItem(newItem);
        }

        if(newItem == null && oldItem != null){
            int slotIndex = (int)oldItem.equipSlot;
            equipmentSlotUI[slotIndex].ClearSlot();
        }
    }

    public void ShowMenu(){
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
}
