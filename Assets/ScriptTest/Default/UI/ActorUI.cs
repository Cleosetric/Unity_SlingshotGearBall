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
    public TextMeshProUGUI textSP;
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
        DisplayHPBar();
        DisplaySPBar();
        if(actor.isAlive){
            actorFace.sprite = actor.actorSprite;
        }else{
            actorFace.sprite = death;
            textHP.SetText("-");
            textSP.SetText("-");
            hpBar.fillAmount = 0;
            spBar.fillAmount = 0;
        }
    }

    private void DisplayHPBar()
    {
        textHP.SetText(actor.currentHP + "/"+ actor.statMHP.GetValue());
        textSP.SetText(Mathf.Round(actor.currentSP).ToString());
        hpBar.fillAmount = (float)actor.currentHP / (float)actor.statMHP.GetValue();
    }
    
    private void DisplaySPBar()
    {
        spBar.fillAmount = (float)actor.currentSP / (float)actor.statMSP.GetValue();
    }

    public void ShowActorStatus(){
        SoundManager.Instance.Play("ButtonClick");
        Party.Instance.SetActor(actorIndex);
        actorStatusUI.SetActive(true);
    }

}
