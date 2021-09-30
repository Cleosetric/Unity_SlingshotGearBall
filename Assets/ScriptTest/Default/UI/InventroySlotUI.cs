using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventroySlotUI : MonoBehaviour
{
    Items item;
    public Image icon;
    public Button remove;

    public void AddItem(Items newItem){
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        remove.gameObject.SetActive(false);
        remove.interactable = true;
    }

    public void ClearSlot(){
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        remove.gameObject.SetActive(false);
        remove.interactable = false;
    }

    public void RemoveItem(){
        InventoryManager.Instance.Remove(item);
    }

    public void UseItem(){
        if(item != null){
            item.Use();
        }
    }
}
