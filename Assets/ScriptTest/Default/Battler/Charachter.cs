using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Charachter : MonoBehaviour
{
    [Space]
    [Header("Base Parameter")]
    [Tooltip("Max Health Point")] public Statf statMHP;
    [Tooltip("Max Stamina Point")] public Statf statMSP;
    [Tooltip("Base Attack")] public Statf statATK;
    [Tooltip("Base Defense")] public Statf statDEF;
    [Tooltip("Base Magic Attack")] public Statf statMATK;
    [Tooltip("Base Magic Defense")] public Statf statMDEF;
    
    [Tooltip("Base Agility")] public Statf statAGI;
    [Tooltip("Base Luck")] public Statf statLUK;
    [Tooltip("Health Point Regeneration")] public Statf statHRG;
    [Tooltip("Stamina Point Regeneration")] public Statf statSRG;

    [Space]
    [Header("Ex Parameter")]
    [Tooltip("Hit Rate")] public Statf statHIT;
    [Tooltip("Critical Rate")] public Statf statCRI;
    [Tooltip("Critical Rate")] public Statf statEVA;
    [Tooltip("Health Regeneration Rate")] public Statf statHRR;
    [Tooltip("Stamina Regeneration Rate")] public Statf statSRR;

    public int currentHP { get; set; }
    public int currentSP { get; set; }

    [Space]
    [Header("Base Component")]
    public SpriteRenderer charSprite;
    public TextMeshProUGUI nameText;
    public Image hpBar;
    public Image hpEffect;
    public Rigidbody2D rb;

    public virtual void ApplyDamage(Charachter user)
    {
        int damage = Mathf.RoundToInt(user.statATK.GetValue());
        float hitRate = (user.statHIT.GetValue()/ 100) - (this.statEVA.GetValue() / 100);
        float criRate = (user.statCRI.GetValue() / 100) + 1 - (user.currentHP/user.statMHP.GetValue());
        float rand = Random.Range(0.0f,1.0f);

        // Debug.Log("Hit - Rand:"+rand+" <= "+hitRate);
        if(rand <= hitRate){
            if(damage <= Mathf.RoundToInt(this.statDEF.GetValue())){
                HitCounter.Instance.AddDamagePopup(transform, 2, "0", "NEGATE");
            }else{
                damage -= Mathf.RoundToInt(this.statDEF.GetValue());
                damage = Mathf.Clamp(damage, 0, int.MaxValue);
                
                // Debug.Log(" Cri - Rand:"+rand+" > "+criRate);
                if(rand < criRate){
                    damage *= 2;
                    float damageVariance = Random.Range(0.0f,0.2f);
                    // Debug.Log("CRand:"+damage+" | "+damageVariance);
                    damage = Mathf.RoundToInt(damage + (damage * damageVariance));
                    currentHP -= damage;
                    HitCounter.Instance.AddDamagePopup(transform, 3, damage.ToString(), "CRITICAL");
                }else{
                    float damageVariance = Random.Range(0.0f,0.2f);
                    // Debug.Log("NRand:"+damage+" | "+damageVariance);
                    damage = Mathf.RoundToInt(damage + (damage * damageVariance));
                    currentHP -= damage;
                    HitCounter.Instance.AddDamagePopup(transform, 1, damage.ToString(), "");
                }

                if(currentHP <= 0){
                    currentHP = 0;
                    Die();
                }   
            }
        }else{
            HitCounter.Instance.AddDamagePopup(transform, 2, "0", "MISS");
        }
    }

    public virtual void Die(){

    }
}
