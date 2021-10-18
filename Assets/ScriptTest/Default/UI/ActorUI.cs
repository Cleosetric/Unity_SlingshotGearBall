using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActorUI : MonoBehaviour
{
    public Sprite death;
    public Image actorFace;
    public Image hpBar;
    public Image spBar;
    public TextMeshProUGUI textHP;
    public GameObject actorStatusUI;

    private Actor actor;
    private int actorIndex;

    public void SetupActor(Actor actor, int index){ 
        this.actor = actor;
        this.actorIndex = index;
        RefreshHud();
    }

    public void RefreshHud()
    {
        if(actor.isAlive){
            actorFace.sprite = actor.actorSprite;
        }else{
            actorFace.sprite = death;
        }
        DisplayHPBar();
        DisplaySPBar();
    }

    private void DisplayHPBar()
    {
        textHP.SetText(actor.currentHP + "/"+ actor.statMHP.GetValue());
        hpBar.fillAmount = (float)actor.currentHP / (float)actor.statMHP.GetValue();
    }
    
    private void DisplaySPBar()
    {
        spBar.fillAmount = (float)actor.currentSP / (float)actor.statMSP.GetValue();
    }

    public void ShowActorStatus(){
        Party.Instance.SetActor(actorIndex);
        actorStatusUI.SetActive(true);
    }

}
