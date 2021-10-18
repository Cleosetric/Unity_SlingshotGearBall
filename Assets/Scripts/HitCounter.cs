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
    public Animator hitAnim;

    [Space]
    [Header("Damage Popup")]
    public GameObject popUI;

    private int hitCounter;
    private float lastHitTime;

    private void Start() {
        hitText.text = "";
        hitAnim = hitText.GetComponent<Animator>();
    }

    private void Update() {
        if((Time.time - lastHitTime) > delayTimeHits)
        {
            hitText.text = "";
            lastHitTime = 0;
        }
    }

    public void AddHitCounter(int combo)
    {
        if((Time.time - lastHitTime) < delayTimeHits)
        {
            hitCounter += combo;
        }
        else
        {
            hitCounter = 1;
        }
        hitAnim.SetTrigger("comboHit");
        hitText.text = hitCounter+" Hits!";
        lastHitTime = Time.time;
    }

    public void AddDamagePopup(Transform pos, int type, string damage, string hit){
        GameObject popup = Instantiate(popUI, pos.position, Quaternion.identity);
        popup.transform.SetParent(pos);
        popup.transform.position = pos.position + Vector3.up;
        popup.GetComponent<PopupUI>().Setup(type,damage,hit);
    }
}
