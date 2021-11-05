using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
	#region Singleton
	private static TimeManager _instance;
	public static TimeManager Instance { get { return _instance; } }

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

	[Header("Timer")]
	public float timeRemaining = 300;
    public bool timerIsRunning = false;
	public TextMeshProUGUI timerText;

	[Space]
	[Header("Slowmotion")]
    public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;

	private void Start() {
		timerIsRunning = true;
	}

	void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.unscaledDeltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

	public string GetDisplayTime(float givenTime){
		float clearedTime = givenTime - timeRemaining;
		float minutes = Mathf.FloorToInt(clearedTime / 60); 
        float seconds = Mathf.FloorToInt(clearedTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	public float GetRemainingMinutes(){
		return Mathf.FloorToInt(timeRemaining / 60); 
	}

	public void EnterSlowmotion()
	{
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}

    public void ReleaseSlowmotion(){
        Time.timeScale = 1;
    }

	public void StartImpactMotion(){
		EnterSlowmotion();
		StartCoroutine(ReleaseImpactMotion(0.3f));
	}

	public void VictoryMotion(){
		EnterSlowmotion();
		StartCoroutine(ReleaseImpactMotion(1f));
	}

	private IEnumerator ReleaseImpactMotion(float slowdownTime){
		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
		yield return new WaitForSecondsRealtime(slowdownTime);
		Time.timeScale = 1f;
		
	}
}
