using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemHelpUI : MonoBehaviour
{

    #region Singleton
	private static ItemHelpUI _instance;
	public static ItemHelpUI Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
	#endregion
    
    Equipment equip;
    Actor actor;
    public Image icon;
    public GameObject frame;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI itemStat;
    public TextMeshProUGUI itemClass;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemRarity;
    public Button btnEquip;
    public Button btnSell;

    private void Start() {
        actor = Party.Instance.GetActiveActor();
        ResetUI();
    }

    public void ResetUI()
    {
        equip = null;
        icon.enabled = false;
        icon.sprite = null;
        itemName.SetText("");
        itemDesc.SetText("");
        itemStat.SetText("");
        itemClass.SetText("");
        itemPrice.SetText("");
        itemRarity.SetText("");
        btnEquip.gameObject.SetActive(false);
        btnSell.gameObject.SetActive(false);
        frame.SetActive(false);
    }

    public void ShowUI(Equipment item){
        equip = item;
        frame.SetActive(true);
        icon.enabled = true;
        icon.sprite = item.icon;
        switch ((int)item.rarity)
        {
            case 0:
                itemRarity.SetText("<color=#5C5C5C>["+item.rarity+"]</color>");
            break;
            case 1:
                itemRarity.SetText("<color=#3CF800>["+item.rarity+"]</color>");
            break;
            case 2:
                itemRarity.SetText("<color=#0c539d>["+item.rarity+"]</color>");
            break;
            case 3:
                itemRarity.SetText("<color=#64109f>["+item.rarity+"]</color>");
            break;
            case 4:
                itemRarity.SetText("<color=#937f11>["+item.rarity+"]</color>");
            break;
            case 5:
                itemRarity.SetText("<color=#c70b3f>["+item.rarity+"]</color>");
            break;

            default:
                itemRarity.SetText("<color=#727a7f>["+item.rarity+"]</color>");
            break;
        }

        itemName.SetText(item.name);
        itemDesc.SetText(item.description);
        itemPrice.SetText("Price : \n"+item.sellingPrice.ToString());
        
        if((int)item.equipClass != 0){
            itemClass.SetText("[ "+item.equipClass.ToString()+" ]");
        }else{
            itemClass.SetText("");
        }
        DrawItemStat();
        btnEquip.gameObject.SetActive(true);
        btnSell.gameObject.SetActive(true);
    }

    private void DrawItemStat()
    {
        StatModifier[] allStat = {
            equip.modMHP,
            equip.modMSP,
            equip.modATK,
            equip.modDEF,
            equip.modMATK,
            equip.modMDEF,
            equip.modAGI,
            equip.modLUK,
            equip.modHRG,
            equip.modSRG,
            equip.modHIT, 
            equip.modCRI,
            equip.modEVA,
            equip.modHRR,
            equip.modSRR
        };
        string[] statLabel = {"MHP","MSP","ATK","DEF","MATK","MDEF","AGI","LUK","HRG","SRG","HIT","CRI","EVA","HRR","SRR"};
        string allstring = "";
        int counter = 0;
        for (int i = 0; i < allStat.Length; i++)
        {
            if(allStat[i].value != 0){
                counter += 1;
                string value;
                if((int)allStat[i].type == 0){
                    value = Mathf.RoundToInt(allStat[i].value).ToString();
                }else{
                    value = Mathf.RoundToInt(allStat[i].value).ToString() + "%";
                }
                allstring += statLabel[i]+"\t: "+"<color=#3CF800>+"+value+"</color>\t";
                if(counter >= 2){
                    counter = 0;
                    allstring += "\n";
                }
            }
        }
        itemStat.SetText(allstring);
    }

    public void EquipItem(){
        if(equip != null){
            SoundManager.Instance.Play("ButtonClick");
            actor = Party.Instance.GetActiveActor();
            if((int)equip.equipClass == 0 || (int)equip.equipClass == (int)actor.actorClass){
                Debug.Log("Equip " + equip.name);
                EquipManager.Instance.Equip(equip);
                equip.RemoveItem();
                ResetUI();
            }
        }
    }

    public void SellItem(){
        if(equip != null){
            SoundManager.Instance.Play("ButtonClick");
            GameManager.Instance.IncreaseCoin(equip.sellingPrice);
            Debug.Log("Sell " + equip.name +" for "+ equip.sellingPrice);
            equip.RemoveItem();
            ResetUI();
        }
    }
}
