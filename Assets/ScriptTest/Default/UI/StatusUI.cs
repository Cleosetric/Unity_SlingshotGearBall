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
    public TextMeshProUGUI matk;
    public TextMeshProUGUI mdef;
    public TextMeshProUGUI agi;
    public TextMeshProUGUI luk;
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
        actor = Party.Instance.GetActiveActor();
        inventory = InventoryManager.Instance;

        actor.onActorStatChanged += UpdateUI;
        inventory.onItemChangedCallback += UpdateUI;
        Party.Instance.partyOnActorChanged += UpdateActor;
        RefreshData();
    }

    void UpdateActor(){
        actor = Party.Instance.GetActiveActor();
        actor.onActorStatChanged += UpdateUI;
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
            hp.SetText(actor.currentHP+"/"+actor.statMHP.GetValue());
            sp.SetText(actor.currentSP+"/"+actor.statMSP.GetValue());
            xp.SetText(actor.currentExp+"/"+actor.nextLevelExp[actor.currentLevel]);
            atk.SetText("ATK\t: "+actor.statATK.GetValue());
            def.SetText("DEF\t: "+actor.statDEF.GetValue());
            matk.SetText("MATK\t: "+actor.statMATK.GetValue());
            mdef.SetText("MDEF\t: "+actor.statMDEF.GetValue());
            agi.SetText("AGI\t: "+actor.statAGI.GetValue());
            luk.SetText("LUK\t: "+actor.statLUK.GetValue());
            hit.SetText("HIT\t: "+actor.statHIT.GetValue());
            cri.SetText("CRI\t: "+actor.statCRI.GetValue());
            eva.SetText("EVA\t: "+actor.statEVA.GetValue());
            hrg.SetText("HRG\t: "+actor.statHRG.GetValue());
            srg.SetText("SRG\t: "+actor.statSRG.GetValue());
            hrr.SetText("HRR\t: "+actor.statHRR.GetValue());
            srr.SetText("SRR\t: "+actor.statSRR.GetValue());

            hpSlider.maxValue = actor.statMHP.GetValue();
            hpSlider.value = actor.currentHP;
            spSlider.maxValue = actor.statMSP.GetValue();
            spSlider.value = actor.currentSP;
            xpSlider.maxValue = actor.nextLevelExp[actor.currentLevel];
            xpSlider.value = actor.currentExp;
        }
    }
}
