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
        actorUI.AddRange(GetComponentsInChildren<ActorUI>());
        InitActorEvent();
        Refresh();
    }

    private void InitActorEvent()
    {
       foreach (Actor actor in party.actors)
       {
            actor.onActorHPChanged += Refresh;
       }
    }

    private void Refresh()
    {
        for (int i = 0; i < actorUI.Count; i++)
        {
            if(i < party.actors.Count){
                actorUI[i].SetupActor(party.actors[i], i);
                actorUI[i].RefreshHud();
            }else{
                actorUI[i].gameObject.SetActive(false);
            }
        }
    }
}
