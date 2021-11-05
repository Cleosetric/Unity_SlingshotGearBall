using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryActorUI : MonoBehaviour
{
    private Actor actor;
    public Image actorSprite;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textPercentage;
    public Slider expSlider;

    private Coroutine expSlideAnim;

    public void Initialize(Actor actor){
        this.actor = actor;
        Refresh();
    }

    void Refresh(){
        actorSprite.sprite = actor.charSprite.sprite;
        if(actor.currentLevel < actor.maxLevel){
            float currentExp = ((float)actor.currentExp / (float)actor.nextLevelExp[actor.currentLevel]);
            textPercentage.SetText(Mathf.RoundToInt(currentExp*100).ToString() + "%");
            textLevel.SetText("LV: "+actor.currentLevel.ToString());
            if(expSlideAnim != null) StopCoroutine(expSlideAnim);
            expSlideAnim =  StartCoroutine(AnimateSliderOverTime(1f));
        }else{
            textLevel.SetText("LV: 10");
            textPercentage.SetText("100%");
            expSlider.maxValue = 1;
            expSlider.value = 1;
        }
    }

    IEnumerator AnimateSliderOverTime(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            expSlider.maxValue = actor.nextLevelExp[actor.currentLevel];
            expSlider.value = Mathf.Lerp(expSlider.value, actor.currentExp, lerpValue);
            yield return null;
        }
        expSlideAnim = null;
    }
}
