using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitCounter : MonoBehaviour
{
    #region Singleton
	private static HitCounter _instance;
	public static HitCounter Instance { get { return _instance; } }

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

    public float delayTimeHits = 1;
    [Space]
    [Header("Combo Counter")]
    public TextMeshProUGUI hitText;
    public TextMeshProUGUI totalDamageText;
    private Animator hitAnim;
    private Animator hitAnimDamage;

    [Space]
    [Header("Damage Popup")]
    public GameObject popUI;

    private int hitCounter;
    private int damageTotal;
    private float lastHitTime;

    private void Start() {
        hitText.text = "";
        totalDamageText.text = "";
        hitAnim = hitText.GetComponent<Animator>();
        hitAnimDamage = totalDamageText.GetComponent<Animator>();
    }

    private void Update() {
        if((Time.time - lastHitTime) > delayTimeHits)
        {
            hitText.text = "";
            totalDamageText.text = "";
            lastHitTime = 0;
        }
    }

    public void AddHitCounter(int combo, int damage)
    {
        if((Time.time - lastHitTime) < delayTimeHits)
        {
            hitCounter += combo;
            damageTotal += damage;
            hitText.text = hitCounter+" Hits!";
        }
        else
        {
            hitCounter = 1;
            damageTotal = damage;
            hitText.text = hitCounter+" Hit!";
        }
        hitAnim.SetTrigger("comboHit");
        hitAnimDamage.SetTrigger("comboHit");
        totalDamageText.text = damageTotal.ToString();
        lastHitTime = Time.time;
    }

    public void AddDamagePopup(Transform pos, int type, string damage, string hit){
        GameObject popup = Instantiate(popUI, pos.position, Quaternion.identity);
        popup.transform.SetParent(pos);
        popup.transform.position = pos.position + new Vector3(0,1.25f,0);
        popup.GetComponent<PopupUI>().Setup(type,damage,hit);
    }
}
