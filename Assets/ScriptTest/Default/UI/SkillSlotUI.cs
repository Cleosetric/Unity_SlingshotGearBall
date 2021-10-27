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
        } else {
            AbilityCoolDown();  
        }

        if(ability.isFinished){
            Debug.Log("abilityfinish");

            casting.SetActive(false);
            isAbilityFinished = true;
            ability.isFinished = false;
            
            backgroundSprite.color = new Color32(36,36,36,255);
        }
    }

    public void AbilityTrigger(){
        if(SkillUI.Instance.CheckAbilityIsAllFinished()){
            if(isAbilityReady && actor.currentSP > ability.staminaCost){
                ability.Activate();
                actor.ApplyAction(ability.staminaCost);

                isAbilityReady = false;
                isAbilityFinished = false;
                abilityTrigger.interactable = false;
                casting.SetActive(true);
               
                backgroundSprite.color = new Color32(100,10,0,255);

                nextReadyTime = coolDownDuration + Time.unscaledTime;
                coolDownTimeLeft = coolDownDuration;
            }
        }
    }

    private void AbilityCoolDown()
    {
        if(ability != null){
            coolDownTimeLeft -= Time.unscaledDeltaTime;

            abilityTrigger.interactable = false;
            cooldownText.enabled = true;
            skillMask.enabled = true;

            cooldownText.text = Math.Round((decimal)coolDownTimeLeft, 1).ToString() + "s";
            skillMask.fillAmount = (coolDownTimeLeft / coolDownDuration);

            if(Mathf.Round(coolDownTimeLeft) <= 0){
                Debug.Log("Interactrue");
                coolDownTimeLeft = 0;
            }
        }
    }

    private void AbilityReady()
    {
        isAbilityReady = true;
        isAbilityFinished = true;

        cooldownText.enabled = false;
        skillMask.enabled = false;
        casting.SetActive(false);
        abilityTrigger.interactable = true;

        backgroundSprite.color = new Color32(36,36,36,255);
    }

    // private void AbilityActive(){
    //     isAbilityReady = true;
    //     isAbilityFinished = false;
    //     abilityTrigger.interactable = true;
    // }
}
