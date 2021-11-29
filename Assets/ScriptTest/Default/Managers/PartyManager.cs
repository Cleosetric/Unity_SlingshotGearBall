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

    public List<Actor> party = new List<Actor>();
    public Party partyObject;

    private void Start() {
        FloodParty();
    }

    private void FloodParty(){
        // Debug.LogWarning("flood Party!");
        party.AddRange(partyObject.actors);
    }

    public void Revive(){
        StartCoroutine(GetRespawn());
        // GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
        // if(respawn != null){
        //     foreach (Actor actor in partyObject.actors)
        //     {
        //         actor.Revive();
        //         actor.transform.position = respawn.transform.position;
        //     }
        // }else{
        //     foreach (Actor actor in partyObject.actors)
        //     {
        //         actor.Revive();
        //         actor.transform.position = partyObject.transform.position;
        //     }
        // }
        // if(respawn != null){
        //     foreach (GameObject obj in party)
        //     {
        //         GameObject actor = Instantiate(obj, respawn.transform.position, Quaternion.identity);
        //         actor.transform.SetParent(partyObject.transform);
        //         actor.transform.position = respawn.transform.position;
        //     }
        // }else{
        //     foreach (GameObject obj in party)
        //     {
        //         GameObject actor = Instantiate(obj, partyObject.transform.position, Quaternion.identity);
        //         actor.transform.SetParent(partyObject.transform);
        //         actor.transform.position = partyObject.transform.position;
        //     }
        // }

    }

    IEnumerator GetRespawn(){
        foreach (Actor actor in party)
        {
            partyObject.actors.Add(actor);
        }

        // partyObject.actors = party;
        GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");

        // while(respawn == null){
        //     respawn = GameObject.FindGameObjectWithTag("Respawn");
        //     yield return null;
        // }

        if(respawn != null){
            Debug.Log(respawn.name + " | "+respawn.transform.position);

            yield return new WaitForSeconds(2f);
            GameManager.Instance.PlayTeleportAnimation();

            foreach (Actor actor in partyObject.actors)
            {
                actor.Revive();
                // actor.transform.position = respawn.transform.position;
            }
            partyObject.transform.position =  respawn.transform.position;
            // partyObject.SetLeader();
            partyObject.Refresh();
            
            foreach (Actor actor in partyObject.actors)
            {
                // actor.Revive();
                actor.transform.position = respawn.transform.position;
            }
            //partyObject.GetLeader().transform.position;
        }
        
        
        partyObject.isPartyDefeated = false;
        GameManager.Instance.isGamePaused = false;
    }
}
