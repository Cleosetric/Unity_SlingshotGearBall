using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    #region Singleton
	private static CollectiblesManager _instance;
	public static CollectiblesManager Instance { get { return _instance; } }

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
	Party party;

    private void Start() {
        party = Party.Instance;
    }

    public void AddBuffATK(int type, float time, float buff){
        if(type == 0){
            StartCoroutine(BuffATK(time,buff));
        }else{
            StartCoroutine(BuffATKParty(time,buff));
        }
    }

    public void AddBuffDEF(int type, float time, float buff){
        if(type == 0){
            StartCoroutine(BuffDEF(time,buff));
        }else{
            StartCoroutine(BuffDEFParty(time,buff));
        }
    }

    public void AddBuffAGI(int type, float time, float buff){
        if(type == 0){
            StartCoroutine(BuffAGI(time,buff));
        }else{
            StartCoroutine(BuffAGIParty(time,buff));
        }
    }

    IEnumerator BuffATK(float effectiveTime, float buffValue){
        StatModifier bonus = new StatModifier(buffValue, StatModType.flat);
        party.GetLeader().statATK.AddModifier(bonus);
        yield return new WaitForSeconds(effectiveTime);
        party.GetLeader().statATK.RemoveModifier(bonus);
    }

    IEnumerator BuffATKParty(float effectiveTime, float buffValue){
        StatModifier bonus = new StatModifier(buffValue, StatModType.flat);
        foreach (Actor actor in party.actors)
        {
            actor.statATK.AddModifier(bonus);
        }

        yield return new WaitForSeconds(effectiveTime);

        foreach (Actor actor in party.actors)
        {
            actor.statATK.RemoveModifier(bonus);
        }
    }

    IEnumerator BuffDEF(float effectiveTime, float buffValue){
        StatModifier bonus = new StatModifier(buffValue, StatModType.flat);
        party.GetLeader().statDEF.AddModifier(bonus);
        yield return new WaitForSeconds(effectiveTime);
        party.GetLeader().statDEF.RemoveModifier(bonus);
    }

    IEnumerator BuffDEFParty(float effectiveTime, float buffValue){
        StatModifier bonus = new StatModifier(buffValue, StatModType.flat);
        foreach (Actor actor in party.actors)
        {
            actor.statDEF.AddModifier(bonus);
        }

        yield return new WaitForSeconds(effectiveTime);

        foreach (Actor actor in party.actors)
        {
            actor.statDEF.RemoveModifier(bonus);
        }
    }

    IEnumerator BuffAGI(float effectiveTime, float buffValue){
        StatModifier bonus = new StatModifier(buffValue, StatModType.flat);
        party.GetLeader().statAGI.AddModifier(bonus);
        yield return new WaitForSeconds(effectiveTime);
        party.GetLeader().statAGI.RemoveModifier(bonus);
    }

    IEnumerator BuffAGIParty(float effectiveTime, float buffValue){
        StatModifier bonus = new StatModifier(buffValue, StatModType.flat);
        foreach (Actor actor in party.actors)
        {
            actor.statAGI.AddModifier(bonus);
        }

        yield return new WaitForSeconds(effectiveTime);

        foreach (Actor actor in party.actors)
        {
            actor.statAGI.RemoveModifier(bonus);
        }
    }
}
