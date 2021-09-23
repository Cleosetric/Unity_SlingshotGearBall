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
    public Equipment[] currentEquipment;

    // Start is called before the first frame update
    void Start()
    {
        inventory = InventoryManager.Instance;
        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlot];
    }

    public void Equip(Equipment newItem){

        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        if(currentEquipment[slotIndex] != null){
            if(inventory.items.Count < inventory.maxSlot){ 
                oldItem = currentEquipment[slotIndex];
                inventory.Add(oldItem);
                currentEquipment[slotIndex] = newItem;
            }
        }else{
            currentEquipment[slotIndex] = newItem;
        }

        if(onEquipmentChanged != null) onEquipmentChanged.Invoke(newItem, oldItem);
    }

    public void Unequip(int slotIndex){
        if(inventory.items.Count < inventory.maxSlot){
            if(currentEquipment[slotIndex] != null){
                Equipment oldItem = currentEquipment[slotIndex];
                inventory.Add(oldItem);
                currentEquipment[slotIndex] = null;
                if(onEquipmentChanged != null) onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll(){
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}
