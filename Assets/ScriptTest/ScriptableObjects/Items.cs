using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Inventory/New Item", order = 1)]
public class Items : ScriptableObject
{
    new public string name = "";
    public string description;
    public Sprite icon;
    public bool isDefaultItem = false;

    public virtual void Use(){
        Debug.Log("user Item "+ name);
    }

    public void RemoveItem(){
        InventoryManager.Instance.Remove(this);
    }
}
