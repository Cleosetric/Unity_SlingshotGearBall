using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;

	// void Update ()
	// {
	// 	Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
	// 	Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	// }

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

	private IEnumerator ReleaseImpactMotion(float slowdownTime){
		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
		yield return new WaitForSecondsRealtime(slowdownTime);
		Time.timeScale = 1f;
		
	}
}
