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
    public Animator uiAnim;
    public GameObject actorPanel;
    public Button buttonReward;
    public List<VictoryActorUI> battleMembers = new List<VictoryActorUI>();

    // Start is called before the first frame update
    void Start()
    {
        battleMembers.AddRange(actorPanel.GetComponentsInChildren<VictoryActorUI>());
        Invoke("SetupBattleMembers",1f);
    }

    public void OnButtonDoubleReward(){
        SoundManager.Instance.Play("ButtonClick");
        GameManager.Instance.stageCoin *= 2;
        textCoin.SetText(GameManager.Instance.stageCoin.ToString());
        Debug.Log("SHow Ads then double the rewards");
        buttonReward.interactable = false;
    }

    public void OnButtonHome(){
        SoundManager.Instance.Play("ButtonClick");
        SoundManager.Instance.Stop("Battle");
        LevelLoader.Instance.LoadScene("WorldScene");
    }

    public void OnButtonNextLevel(){
        SoundManager.Instance.Play("ButtonCancel");
        Debug.Log("Next Level Yo!");
    }

    public void ShowBattleResult(){
        uiAnim.SetBool("ShowUp", true);
        TimeManager.Instance.timerIsRunning = false;

        textTime.SetText(TimeManager.Instance.GetDisplayTime());
        textScore.SetText(ScoreCounter.Instance.score.ToString());
        textCoin.SetText(GameManager.Instance.stageCoin.ToString());
        textExp.SetText(GameManager.Instance.totalExpGained.ToString());

        CalculateStar();
        StartCoroutine(SetupGainedExp());
    }

    private void CalculateStar()
    {
        int maxScore = GameManager.Instance.maxScore;
        int totalScore = ScoreCounter.Instance.score + TimeManager.Instance.GetRemainingMinutes();
        int scorePercentage = Mathf.FloorToInt((totalScore * 10) / maxScore);

        Debug.Log(scorePercentage +" | "+ totalScore +" | "+ maxScore);

        if(scorePercentage <= 4){
            SetActiveStar(1);
            //Star1
        }else if(scorePercentage <= 7){
            SetActiveStar(2);
            //Star2
        }else{
            SetActiveStar(3);
            //Star3
        }

        //check overallscore
    }

    private void SetActiveStar(int star){
        switch (star)
        {
            case 1:
                {
                    star1.enabled = true;
                    star2.enabled = false;
                    star3.enabled = false;
                    break;
                }

            case 2:
                {
                    star1.enabled = true;
                    star2.enabled = true;
                    star3.enabled = false;
                    break;
                }

            case 3:
                {
                    star1.enabled = true;
                    star2.enabled = true;
                    star3.enabled = true;
                    break;
                }
        }
    }

    IEnumerator SetupGainedExp()
    {
        Actor[] members = Party.Instance.actors.ToArray();
        int liveCount = 0;
        foreach (Actor member in members)
        {
            if(member != null && member.isAlive){
                liveCount++;
            }
        }
        
        int totalExpGained = GameManager.Instance.totalExpGained / liveCount;

        yield return new WaitForSeconds(2f);
        foreach (Actor actor in Party.Instance.actors)
        {
            if(actor != null || actor != null){
                actor.GainExp(totalExpGained);
                SetupBattleMembers();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void SetupBattleMembers(){
        for (int i = 0; i < battleMembers.Count; i++)
        {
            if(i < PartyManager.Instance.party.Count){
                if(Party.Instance.actors[i] != null)
                {
                    battleMembers[i].Initialize(PartyManager.Instance.party[i]);
                    battleMembers[i].gameObject.SetActive(true);
                }else{
                    battleMembers[i].gameObject.SetActive(false);
                }
            }else{
                battleMembers[i].gameObject.SetActive(false);
            }
        }
    }

}
