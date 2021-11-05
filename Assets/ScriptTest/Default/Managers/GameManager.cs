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
    public int stageEmerald = 0;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textEmerald;

    [Space]
    [Header("Battle Info")]
    public int totalExpGained = 0;

    [Space]
    public GameObject animPrefab;
    public GameObject stageClearText;
    public GameObject victoryPanel;
    public bool isGamePaused = false;

    private int stageCount = 1;
    private bool isStageClear = false;
    private bool isVictory = false;

    private void Start() {
        textCoin.SetText(stageCoin.ToString());
        textEmerald.SetText(stageEmerald.ToString());

        stageClearText.SetActive(false);
    }

    public void IncreaseCoin(int number){
        stageCoin += number;
        textCoin.SetText(stageCoin.ToString());
    }

    public void IncreaseEmerald(int number){
        stageEmerald += number;
        textEmerald.SetText(stageEmerald.ToString());
    }

    public void IncreaseTotalExp(int exp){
        totalExpGained += exp;
    }


    // Update is called once per frame
    private void Update()
    {
        if(EnemyManager.Instance.IsAllEnemyDead()){
            if(stageCount < MapManager.Instance.stage.Count){
                if(!isStageClear){
                    StartCoroutine(StageClear());
                    isStageClear = true;
                }
            }else{
                if(!isVictory){
                    Victory();
                    isVictory = true;
                }
            }
        }
    }

    private void Victory()
    {
        victoryPanel.SetActive(true);
        VictoryUI victory = victoryPanel.GetComponent<VictoryUI>();
        victory.ShowBattleResult();
        isGamePaused = true;
        Debug.Log("Victory yey!");
    }

    public void GameOver(){
        isGamePaused = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        EnterNewStage();
    }

    void EnterNewStage(){
        MapManager.Instance.SpawnNewStage();
        stageCount ++;
        isStageClear = false;
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
