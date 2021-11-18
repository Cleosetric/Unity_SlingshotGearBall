using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    #region Singleton
	private static PartyManager _instance;
	public static PartyManager Instance { get { return _instance; } }

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

    public List<GameObject> party = new List<GameObject>();
    public Party partyObject;

    public void Revive(){
        GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
        if(respawn != null){
            foreach (GameObject obj in party)
            {
                GameObject actor = Instantiate(obj, respawn.transform.position, Quaternion.identity);
                actor.transform.SetParent(partyObject.transform);
                actor.transform.position = respawn.transform.position;
            }
        }else{
            foreach (GameObject obj in party)
            {
                GameObject actor = Instantiate(obj, partyObject.transform.position, Quaternion.identity);
                actor.transform.SetParent(partyObject.transform);
                actor.transform.position = partyObject.transform.position;
            }
        }
        partyObject.Init();
        GameManager.Instance.isGamePaused = false;
    }
}
