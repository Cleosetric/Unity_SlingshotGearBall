using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupUI : MonoBehaviour
{
    public Image spriteHit;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI hitResultText;

    public void Setup(int type, string dmgText, string hitResult){
        //normal
        if(type == 1){
            hitResultText.enabled = false;
            spriteHit.enabled = false;
            damageText.enabled = true;
            damageText.SetText(dmgText);
        }else if(type == 2){ //miss
            damageText.enabled = false;
            spriteHit.enabled = false;
            hitResultText.enabled = true;
            hitResultText.SetText(hitResult);
        }else if(type == 3){ //critical
            damageText.enabled = true;
            hitResultText.enabled = true;
            spriteHit.enabled = true;
            damageText.SetText(dmgText);
            hitResultText.SetText(hitResult);
        }
    }
}
