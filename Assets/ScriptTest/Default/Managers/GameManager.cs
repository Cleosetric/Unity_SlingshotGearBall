using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

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

    [Header("Currency Info")]
    public int stageCoin = 0;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textStage;

    [Space]
    [Header("Battle Info")]
    public int maxScore = 1000;
    public int totalExpGained = 0;

    [Space]
    public GameObject animPrefab;
    public GameObject stageClearText;
    public GameObject victoryPanel;
    public GameObject GameoverPanel;

    [Space]

    public bool isGamePaused = false;
    public bool isStageClear = false;

    private int stageCount = 0;
    private bool isVictory = false;

    private void Start() {
        textCoin.SetText(stageCoin.ToString());
        textStage.SetText(MapManager.Instance.GetStage());

        stageClearText.SetActive(false);
        victoryPanel.SetActive(false);
        GameoverPanel.SetActive(false);
        EnterNewStage();
        // MapManager.Instance.SpawnNewStage();
    }

    public void IncreaseCoin(int number){
        stageCoin += number;
        textCoin.SetText(stageCoin.ToString());
    }

    public void IncreaseTotalExp(int exp){
        totalExpGained += exp;
    }

    // Update is called once per frame
    private void Update()
    {
        // if(!MapManager.Instance.IsMapHasGoal()){
        //     if(EnemyManager.Instance.IsAllEnemyDead()){
        //         if(stageCount <= MapManager.Instance.stage.Count){
        //             if(!isStageClear){
        //                 StageCleared();
        //             }
        //         }else{
        //             if(!isVictory){
        //                 Victory();
        //             }
        //         }
        //     }
        // }

        if(stageCount > MapManager.Instance.stage.Count){
            if(!isVictory){
                Victory();
            }
        }
    }

    private void Victory()
    {
        victoryPanel.SetActive(true);
        VictoryUI victory = victoryPanel.GetComponent<VictoryUI>();
        victory.ShowBattleResult();

        isGamePaused = true;
        isVictory = true;
        Debug.Log("Victory yey!");
    }

    public void GameOver(){
        GameoverPanel.SetActive(true);
        GameOverUI gameOverUI = GameoverPanel.GetComponent<GameOverUI>();
        gameOverUI.ShowBattleResult();

        isGamePaused = true;
        Debug.Log("Gameover yey!");
    }

    public void StageCleared(){
        isGamePaused = true;
        StartCoroutine(StageClear());
        isStageClear = true;
    }

    IEnumerator StageClear(){
        TimeManager.Instance.EnterSlowmotion();
        yield return new WaitForSeconds(0.1f);
        stageClearText.SetActive(true);
        stageClearText.GetComponent<Animator>().SetBool("isClearing",true);
        yield return new WaitForSeconds(0.1f);
        stageClearText.GetComponent<Animator>().SetBool("isClearing",false);
        yield return new WaitForSeconds(0.1f);
        stageClearText.SetActive(false);
        TimeManager.Instance.ReleaseSlowmotion();
        PlayTeleportAnimation();
        yield return new WaitForSeconds(0.5f);
        textStage.SetText(MapManager.Instance.GetStage());
        EnterNewStage();
    }

    void EnterNewStage(){
        if(stageCount >= MapManager.Instance.stage.Count){
            if(!isVictory){
                Victory();
            }
        }else{
            MapManager.Instance.SpawnNewStage();
            stageCount ++;
            isGamePaused = false;
        }
    }

    public void PlayTeleportAnimation(){
        foreach (Actor actor in Party.Instance.actors)
        {
            if(actor != null && actor.parent != null && actor.parent.gameObject.activeSelf && actor.isAlive){
                GameObject effect = Instantiate(animPrefab, actor.parent.position, Quaternion.identity);
                effect.transform.SetParent(actor.parent);
            }
        }
    }
}
