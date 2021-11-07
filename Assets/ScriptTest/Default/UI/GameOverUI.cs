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

    public void Revive(){
        Debug.Log("Show Ads then revive");
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home(){
        Debug.Log("Next Level Yo!");
    }

    public void ShowBattleResult(){
        uiAnim.SetBool("ShowUI", true);
        TimeManager.Instance.timerIsRunning = false;
        textCoin.SetText(GameManager.Instance.stageCoin.ToString());
        textExp.SetText(GameManager.Instance.totalExpGained.ToString());
    }



}
