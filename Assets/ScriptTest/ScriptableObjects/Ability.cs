using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType { self, party }

public enum AbilityMod {
    none, mhp, msp, atk, def, matk, mdef, agi, luk, hit, cri, eva
}

public enum TargetAttack{ closestEnemy, directionForward, directionUp, directionDown }

public enum ChantType{ move, stop, continous }

public enum ChantAnimation {none, show}

public class Ability : ScriptableObject
{
    [Header("Ability Info")]
    public string abilityName = "New Ability";
    [TextArea] public string abilityDesc;
    public Sprite abilitySprite;
    public bool rotateProjectile;


    [Space]
    [Header("Ability Effect")]
    public float bonusValue;
    public AbilityMod abilityBonus;
    public StatModType bonusType;
    public TargetType targetType;
    public ChantType chantType;

    [Space]
    [Header("Ability Animation")]
    public ChantAnimation chantAnim;
    public GameObject chantAnimPrefab;

    [Space]
    [Header("Ability Time")]
    public float staminaCost;
    public float cooldownTime;
    public bool isFinished = false;
    public Actor actor;
    protected StatModifier bonus;

    public virtual void Initialize(GameObject parent){
        actor = parent.GetComponent<Actor>();
        bonus = new StatModifier(bonusValue, bonusType);
        isFinished = false;
        //wtf
    }

    public virtual void Activate(){
        switch ((int)targetType)
        {
            case 0:
                AddAbilityBonus(actor);
            break;
            case 1:
                foreach (Actor member in Party.Instance.actors)
                {
                    if(member.isAlive) AddAbilityBonus(member);
                }
            break;
        }

        string bonusText = "+"+abilityBonus.ToString().ToUpper()+" UP";
        HitCounter.Instance.AddDamagePopup(actor.transform, 4, bonusText, abilityName);
    }

    public virtual void Deactivate(){
        switch ((int)targetType)
        {
            case 0:
                RemoveAbilityBonus(actor);
            break;
            case 1:
                foreach (Actor member in Party.Instance.actors)
                {
                    if(member.isAlive) RemoveAbilityBonus(member);
                }
            break;
        }
    }

    private void AddAbilityBonus(Actor chara){
        if((int)abilityBonus == 0) return;
        switch ((int)abilityBonus)
        {
            case 1:
                chara.statMHP.AddModifier(bonus);
            break;
            case 2:
                chara.statMSP.AddModifier(bonus);
            break;
            case 3:
                chara.statATK.AddModifier(bonus);
            break;
            case 4:
                chara.statDEF.AddModifier(bonus);
            break;
            case 5:
                chara.statMATK.AddModifier(bonus);
            break;
            case 6:
                chara.statMDEF.AddModifier(bonus);
            break;
            case 7:
                chara.statAGI.AddModifier(bonus);
            break;
            case 8:
                chara.statLUK.AddModifier(bonus);
            break;
            case 9:
                chara.statHIT.AddModifier(bonus);
            break;
            case 10:
                chara.statCRI.AddModifier(bonus);
            break;
            case 11:
                chara.statEVA.AddModifier(bonus);
            break;
        }
    }

    private void RemoveAbilityBonus(Actor chara){
        if((int)abilityBonus == 0) return;
        switch ((int)abilityBonus)
        {
            case 1:
                chara.statMHP.RemoveModifier(bonus);
            break;
            case 2:
                chara.statMSP.RemoveModifier(bonus);
            break;
            case 3:
                chara.statATK.RemoveModifier(bonus);
            break;
            case 4:
                chara.statDEF.RemoveModifier(bonus);
            break;
            case 5:
                chara.statMATK.RemoveModifier(bonus);
            break;
            case 6:
                chara.statMDEF.RemoveModifier(bonus);
            break;
            case 7:
                chara.statAGI.RemoveModifier(bonus);
            break;
            case 8:
                chara.statLUK.RemoveModifier(bonus);
            break;
            case 9:
                chara.statHIT.RemoveModifier(bonus);
            break;
            case 10:
                chara.statCRI.RemoveModifier(bonus);
            break;
            case 11:
                chara.statEVA.RemoveModifier(bonus);
            break;
        }
        Debug.Log("Remove Bonus : "+abilityBonus.ToString()+" | "+bonus.value);
    }

}
