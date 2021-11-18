using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleScreenUI : MonoBehaviour
{
    public GameObject creditUI;

    public void StartGame(){
        SoundManager.Instance.Stop("Title");
        SoundManager.Instance.Play("ButtonClick");
        LevelLoader.Instance.LoadScene("WorldScene");
    }

    public void Credits(){
        SoundManager.Instance.Play("ButtonClick");
        creditUI.SetActive(true);
    }

    public void HideCredits(){
        SoundManager.Instance.Play("ButtonClick");
        creditUI.SetActive(false);
    }

    public void ExitGame(){
        SoundManager.Instance.Stop("Title");
        SoundManager.Instance.Play("ButtonClick");
        Application.Quit();
    }


}
