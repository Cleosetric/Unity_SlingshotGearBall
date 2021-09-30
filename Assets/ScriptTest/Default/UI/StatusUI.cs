using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    Actor actor;
    InventoryManager inventory;

    public Image spriteFace;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textClass;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI sp;
    public TextMeshProUGUI xp;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI def;
    public TextMeshProUGUI agi;
    public TextMeshProUGUI hit;
    public TextMeshProUGUI cri;
    public TextMeshProUGUI eva;
    public TextMeshProUGUI srg;
    public TextMeshProUGUI hrg;
    public TextMeshProUGUI hrr;
    public TextMeshProUGUI srr;

    public Slider hpSlider;
    public Slider spSlider;
    public Slider xpSlider;

    // Start is called before the first frame update
    void Start()
    {
        actor = Party.Instance.getActiveActor();
        inventory = InventoryManager.Instance;

        actor.onActorStatChanged += UpdateUI;
        inventory.onItemChangedCallback += UpdateUI;
        Party.Instance.partyOnActorChanged += UpdateActor;
        RefreshData();
    }

    void UpdateActor(){
        actor = Party.Instance.getActiveActor();
        RefreshData();
    }

    // Update is called once per frame
    void UpdateUI()
    {
       RefreshData();
    }

    void RefreshData(){
        if(actor != null){
            spriteFace.sprite = actor.actorFace;
            textName.SetText(actor.actorName);
            textLevel.SetText("(Level "+actor.currentLevel+")");
            textClass.SetText("["+actor.actorClass.ToString()+"]");
            hp.SetText(actor.currentHP+"/"+actor.statMHP.getValue());
            sp.SetText(actor.currentSP+"/"+actor.statMSP.getValue());
            xp.SetText(actor.currentExp+"/"+actor.nextLevelExp[actor.currentLevel]);
            atk.SetText("ATK\t: "+actor.statATK.getValue());
            def.SetText("DEF\t: "+actor.statDEF.getValue());
            agi.SetText("AGI\t: "+actor.statAGI.getValue());
            hit.SetText("HIT\t: "+actor.statHIT.getValue());
            cri.SetText("CRI\t: "+actor.statCRI.getValue());
            eva.SetText("EVA\t: "+actor.statEVA.getValue());
            hrg.SetText("HRG\t: "+actor.statHRG.getValue());
            srg.SetText("SRG\t: "+actor.statSRG.getValue());
            hrr.SetText("HRR\t: "+actor.statHRR.getValue());
            srr.SetText("SRR\t: "+actor.statSRR.getValue());

            hpSlider.maxValue = actor.statMHP.getValue();
            hpSlider.value = actor.currentHP;
            spSlider.maxValue = actor.statMSP.getValue();
            spSlider.value = actor.currentSP;
            xpSlider.maxValue = actor.nextLevelExp[actor.currentLevel];
            xpSlider.value = actor.currentExp;
        }
    }
}
