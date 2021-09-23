using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    Equipment item;
    public Image icon;

    public void AddItem(Equipment newItem){
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot(){
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void RemoveItem(){
        if(item != null){
            int slotIndex = (int)item.equipSlot;
            EquipManager.Instance.Unequip(slotIndex);
        }
    }
}
