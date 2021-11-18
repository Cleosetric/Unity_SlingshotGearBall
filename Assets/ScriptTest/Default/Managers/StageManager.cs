using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Singleton
	private static StageManager _instance;
	public static StageManager Instance { get { return _instance; } }

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
    

    public void FightStage(){
        SoundManager.Instance.Stop("Home");
        SoundManager.Instance.Play("ButtonClick");
        LevelLoader.Instance.LoadScene("BattleScene");
    }
    
    public void BackTitle(){
        SoundManager.Instance.Stop("Home");
        SoundManager.Instance.Play("ButtonClick");
        LevelLoader.Instance.LoadScene("TitleScene");
    }

    public void ButtonUnit(){
        SoundManager.Instance.Play("ButtonCancel");
    }

    public void ButtonPrepare(){
        SoundManager.Instance.Play("ButtonCancel");
    }

    public void ButtonShop(){
        SoundManager.Instance.Play("ButtonCancel");
    }

    public void ButtonWorld(){
        SoundManager.Instance.Play("ButtonClick");
    }

    public void ChangeStage(){
        SoundManager.Instance.Play("ButtonClick");
    }
}
