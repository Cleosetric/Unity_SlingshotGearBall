using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCutinUI : MonoBehaviour
{
    public TextMeshProUGUI textSkill;
    public Image spriteCaster;
    public Animator anim;
    private Ability ability;

    public void Initialize(Ability ability){
        this.ability = ability;
        Refresh();
    }

    // Update is called once per frame
    void Refresh()
    {
        TimeManager.Instance.EnterSlowmotion();
        anim.SetBool("Show", true);
        textSkill.SetText(ability.abilityName);
        spriteCaster.sprite = ability.actor.charSprite.sprite;
        Invoke("HideAnim", 0.15f);
    }

    void HideAnim(){
        anim.SetBool("Show", false);
        TimeManager.Instance.ReleaseSlowmotion();
    }
}
