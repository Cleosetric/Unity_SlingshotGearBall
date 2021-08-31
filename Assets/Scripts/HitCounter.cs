using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitCounter : MonoBehaviour
{
   
    [SerializeField] private float delayTimeHits = 1;
    [SerializeField] private GameObject hitPanel;
    [SerializeField] private TextMeshProUGUI hitText;

    private int hitCounter;
    private float lastHitTime;

    private void Awake() {
        Player.OnComboCounter += AddHitCounter;
    }

    private void Start() {
        hitPanel.SetActive(false);
    }

    private void Update() {
        if((Time.time - lastHitTime) > delayTimeHits)
        {
            hitPanel.SetActive(false);
            hitText.text = "";
            lastHitTime = 0;
        }
    }

    // Call when you hit an enemy
    public void AddHitCounter(int combo)
    {
        hitPanel.SetActive(true);
        // Debug.Log((Time.time - lastHitTime));
        if((Time.time - lastHitTime) < delayTimeHits)
        {
            // then add to the hit counter
            hitCounter += combo;
        }
        else
        {
            // => Reset the counter and start over with this hit as the first one
            hitCounter = 1;
        }
        hitText.text = "Combo : "+hitCounter;
        
        // update the lastHitTime
        lastHitTime = Time.time;
    }
}
