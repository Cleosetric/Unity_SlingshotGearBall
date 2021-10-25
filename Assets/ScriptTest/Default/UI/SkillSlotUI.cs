using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillSlotUI : MonoBehaviour
{
    private Ability ability;
    private Actor actor;
    private GameObject holder;
    public GameObject casting;
    public Image backgroundSprite;
    public Image skillSprite;
    public Image skillMask;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI staminaCost;
    public Button abilityTrigger;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;
    public bool isAbilityReady = true;
    public bool isAbilityFinished = true;

    public void Initialize(GameObject holder, Ability ability){
        this.holder = holder;
        this.ability = ability;
        this.actor = holder.GetComponent<Actor>();
        RefreshUI();
    }

    void RefreshUI(){
        backgroundSprite.color = new Color32(36,36,36,255);
        skillSprite.sprite = ability.abilitySprite;
        cooldownText.SetText(ability.cooldownTime.ToString());
        staminaCost.SetText("SP:"+ability.staminaCost.ToString());
        casting.SetActive(false);

        coolDownDuration = ability.cooldownTime;
        ability.Initialize(this.holder);
        AbilityReady();
    }

    // Update is called once per frame
    void Update()
    {
        bool coolDownComplete = (Time.unscaledTime > nextReadyTime);
        if (coolDownComplete) 
        {
            AbilityReady();    
        } else 
        {
            AbilityCoolDown();  
        }

        if(ability.isFinished){
            casting.SetActive(false);
            backgroundSprite.color = new Color32(36,36,36,255);
            isAbilityFinished = true;
            ability.isFinished = false;
        }
    }

    public void AbilityTrigger(){
        if(SkillUI.Instance.CheckAbilityIsAllFinished()){
            if(isAbilityReady && actor.currentSP > ability.staminaCost){
                ability.Activate();
                actor.ApplyAction(ability.staminaCost);
                isAbilityReady = false;
                isAbilityFinished = false;
                casting.SetActive(true);
                nextReadyTime = coolDownDuration + Time.unscaledTime;
                coolDownTimeLeft = coolDownDuration;
                backgroundSprite.color = new Color32(100,10,0,255);
            }
        }
    }

    private void AbilityCoolDown()
    {
        if(ability != null){
            coolDownTimeLeft -= Time.unscaledDeltaTime;
            if(coolDownTimeLeft <= 0.0f){
                coolDownTimeLeft = 0;
            }
            // float roundedCd =  Mathf.Round(coolDownTimeLeft);
            cooldownText.enabled = true;
            cooldownText.text = String.Format("{0:0.0}",coolDownTimeLeft)+"s";
            skillMask.enabled = true;
            skillMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
            abilityTrigger.interactable = false;
        }
    }

    private void AbilityReady()
    {
        isAbilityReady = true;
        isAbilityFinished = true;

        abilityTrigger.interactable = true;
        cooldownText.enabled = false;
        skillMask.enabled = false;
        casting.SetActive(false);
        backgroundSprite.color = new Color32(36,36,36,255);
    }

    // private void AbilityActive(){
    //     isAbilityReady = true;
    //     isAbilityFinished = false;
    //     abilityTrigger.interactable = true;
    // }
}
