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
        switch (type)
        {
            case 1:
                hitResultText.enabled = false;
                spriteHit.enabled = false;
                damageText.enabled = true;
                damageText.SetText(dmgText);
                break;
            case 2:
                damageText.enabled = false;
                spriteHit.enabled = false;
                hitResultText.enabled = true;
                hitResultText.SetText(hitResult);
                break;
            case 3:
                damageText.enabled = true;
                hitResultText.enabled = true;
                spriteHit.enabled = true;
                damageText.SetText(dmgText);
                hitResultText.SetText(hitResult);
                break;
            case 4:
                damageText.enabled = true;
                hitResultText.enabled = true;
                spriteHit.enabled = false;
                damageText.SetText("<color=#0cc92f>"+dmgText+"</color>");
                hitResultText.SetText("<color=#ffffff>"+hitResult+"</color>");
                break;
        }
    }
}
