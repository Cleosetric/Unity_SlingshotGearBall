using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}

public class ObjectPooling : MonoBehaviour
{
    #region Singleton
	private static ObjectPooling _instance;
	public static ObjectPooling Instance { get { return _instance; } }

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

    public List<ObjectPoolItem> itemsToPool;
    private List<GameObject> pooledObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.transform.SetParent(transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
