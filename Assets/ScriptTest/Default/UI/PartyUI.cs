using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    List<ActorUI> actorUI = new List<ActorUI>();
    Party party;

    // Start is called before the first frame update
    void Start()
    {
        party = Party.Instance;
        party.partyOnActorChanged += InitActorEvent;
        actorUI.AddRange(GetComponentsInChildren<ActorUI>());
        InitActorEvent();
        Refresh();
    }

    private void InitActorEvent()
    {
        Debug.Log("PartyUI Don");
        foreach (Actor actor in party.actors)
        {
            actor.onActorHPChanged += Refresh;
        }
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < actorUI.Count; i++)
        {
            if(i < PartyManager.Instance.party.Count){
                if( i < party.actors.Count){
                    actorUI[i].gameObject.SetActive(true);
                    actorUI[i].SetupActor(party.actors[i], i);
                    actorUI[i].AliveHUD();
                }else{
                    actorUI[i].gameObject.SetActive(true);
                    actorUI[i].DeadHUD();
                }
            }else{
                actorUI[i].gameObject.SetActive(false);
            }
        }
    }
}
