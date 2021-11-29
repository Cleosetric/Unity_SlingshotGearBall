using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textExp;
    public Animator uiAnim;
    public Button buttonRevive;

    public void Revive(){
        if(TimeManager.Instance.timeRemaining <= 1) return;
        Debug.Log("Show Ads then revive");

        SoundManager.Instance.Play("ButtonClick");
        PartyManager.Instance.Revive();
        buttonRevive.interactable = false;
        SoundManager.Instance.Play("Battle");
        Reset();
    }

    public void Restart(){
        SoundManager.Instance.Play("ButtonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home(){
        Debug.Log("Back Home");
        SoundManager.Instance.Play("ButtonClick");
        SoundManager.Instance.Stop("Battle");

        LevelLoader.Instance.LoadScene("WorldScene");
    }

    public void ShowBattleResult(){
        uiAnim.SetBool("ShowUI", true);
        TimeManager.Instance.timerIsRunning = false;
        textCoin.SetText(GameManager.Instance.stageCoin.ToString());
        textExp.SetText(GameManager.Instance.totalExpGained.ToString());
    }

    void Reset(){
        uiAnim.SetBool("ShowUI", false);
        gameObject.SetActive(false);
    }
}
