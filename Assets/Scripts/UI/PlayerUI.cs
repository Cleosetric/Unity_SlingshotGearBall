using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    // private HeroBase heroData;
    // private Player player;

    public TextMeshProUGUI heroName;
    public Image heroFace;
    public Slider heroHp;
    public Slider heroStamina;
    private TextMeshProUGUI textHP;
    private TextMeshProUGUI textStamina;

    // Start is called before the first frame update
    void Start()
    {
        // heroData = PartyManager.Instance.GetActiveHero();
        // player = FindObjectOfType<Player>();

        textHP = heroHp.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textStamina = heroStamina.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        // DisplayUI(player);
    }

    public void DisplayUI(Player player){
        if(player != null){

            heroName.text = player.name;
            heroFace.sprite = player.spriteFace;

            heroHp.maxValue = player.mhp;
            heroHp.value = player.hp;

            heroStamina.maxValue = player.mstamina;
            heroStamina.value = player.stamina;

            textHP.text = player.hp.ToString();
            textStamina.text = player.stamina.ToString();
        }
    }

}
