using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    InventoryManager inventory;
    EquipManager equipment;

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

        inventory.onItemChangedCallback += UpdateUI;
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
