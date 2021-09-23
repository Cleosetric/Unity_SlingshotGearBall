using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
	private static InventoryManager _instance;
	public static InventoryManager Instance { get { return _instance; } }

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

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public List<Items> randItem = new List<Items>();
    public int maxSlot = 20;

    public List<Items> items = new List<Items>();
    
    public bool Add(Items item){
        if(!item.isDefaultItem){
            if(items.Count >= maxSlot){
                Debug.Log("Not Enough Space");
                return false;
            }
            items.Add(item);
            if(onItemChangedCallback != null) onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Remove(Items item){
        items.Remove(item);
        if(onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }

    public void AddRandomItem(){
        int rand = Random.Range (0, randItem.Count);
        Add(randItem[rand]);
    }


}
