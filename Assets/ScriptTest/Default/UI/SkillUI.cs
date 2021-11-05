using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    #region Singleton
	private static SkillUI _instance;
	public static SkillUI Instance { get { return _instance; } }

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
    private Party party;
    private Actor actor;
    private List<SkillSlotUI> slot = new List<SkillSlotUI>();

    // Start is called before the first frame update
    void Start()
    {
        party = Party.Instance;
        actor = party.GetLeader();   
        party.partyOnActorChanged += Refresh;
        slot.AddRange(GetComponentsInChildren<SkillSlotUI>());
        Refresh();
    }

    void Refresh(){
        actor = party.GetLeader();
        if(actor != null){
            for (int i = 0; i < slot.Count; i++)
            {
                slot[i].isAbilityReady = true;
                slot[i].isAbilityFinished = true;
                if(i < actor.abilities.Count){
                    slot[i].Initialize(actor.gameObject,actor.abilities[i]);
                    slot[i].gameObject.SetActive(true);
                }else{
                    slot[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public bool CheckAbilityIsAllReady(){
        if(slot.TrueForAll(x => x.isAbilityReady == true)){
            return true;
        }
        return false;
    }

    public bool CheckAbilityIsAllFinished(){
        if(slot.TrueForAll(x => x.isAbilityFinished == true)){
            return true;
        }
        return false;
    }
}
