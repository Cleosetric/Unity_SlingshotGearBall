using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class VictoryUI : MonoBehaviour
{
    [Header("Stars")]
    public Image star1;
    public Image star2;
    public Image star3;

    [Space]
    [Header("Texts")]
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textExp;

    [Space]
    public GameObject actorPanel;
    public List<VictoryActorUI> battleMembers = new List<VictoryActorUI>();

    private float initialTime;
    private Animator uiAnim;

    // Start is called before the first frame update
    void Start()
    {
        initialTime = TimeManager.Instance.timeRemaining;
        uiAnim = GetComponent<Animator>();
        battleMembers.AddRange(actorPanel.GetComponentsInChildren<VictoryActorUI>());
        Invoke("SetupBattleMembers",1f);
    }

    public void OnButtonDoubleReward(){
        Debug.Log("SHow Ads then double the rewards");
    }

    public void OnButtonHome(){
        GameManager.Instance.GameOver();
    }

    public void OnButtonNextLevel(){
        Debug.Log("Next Level Yo!");
    }

    public void ShowBattleResult(){
        uiAnim.SetBool("ShowUp", true);
        textTime.SetText(TimeManager.Instance.GetDisplayTime(initialTime));
        textScore.SetText(ScoreCounter.Instance.score.ToString());
        textCoin.SetText(GameManager.Instance.stageCoin.ToString());
        textExp.SetText(GameManager.Instance.totalExpGained.ToString());
        CalculateStar();
        SetupGainedExp();
    }

    private void CalculateStar()
    {
        //check overallscore
    }

    private void SetupGainedExp()
    {
        foreach (Actor actor in Party.Instance.actors)
        {
            actor.GainExp(GameManager.Instance.totalExpGained / Party.Instance.actors.Count);
        }

        Invoke("SetupBattleMembers",2f);
    }

    private void SetupBattleMembers(){
        for (int i = 0; i < battleMembers.Count; i++)
        {
            if(i < Party.Instance.actors.Count){
                battleMembers[i].Initialize(Party.Instance.actors.ToArray()[i]);
                battleMembers[i].gameObject.SetActive(true);
            }else{
                battleMembers[i].gameObject.SetActive(false);
            }
        }
    }

}
