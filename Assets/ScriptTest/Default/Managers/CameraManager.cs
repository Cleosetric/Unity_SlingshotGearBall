using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
	private static CameraManager _instance;
	public static CameraManager Instance { get { return _instance; } }

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
	public CinemachineVirtualCamera camera2D;
    private CinemachineBasicMultiChannelPerlin noise;

	void Start()
	{
        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 1;
        noise = camera2D.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
	}

    public CinemachineVirtualCamera GetCamera(){
        return camera2D;
    }

	public void Shake(float duration, float magnitude)
	{
		StartCoroutine(PerformShake(duration, magnitude));
	}

    IEnumerator PerformShake(float duration, float amount){
        float ellapsedTime = 0;
        while (ellapsedTime < duration)
        {
            noise.m_AmplitudeGain = amount;
            noise.m_FrequencyGain = 2;
            ellapsedTime += Time.deltaTime;
            yield return null;
        }
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
