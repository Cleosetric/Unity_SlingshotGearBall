using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
	private static PlayerManager _instance;
	public static PlayerManager Instance { get { return _instance; } }

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

    private PlayerControl control;

    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<PlayerControl>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
