using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private Transform camTransform;
	private Vector3 originalPos;

	void Start()
	{
        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 1;
        
        camTransform = Camera.main.transform;
		originalPos = camTransform.localPosition;
	}

	public void Shake(float duration, float magnitude)
	{
		StartCoroutine(PerformShake(duration, magnitude));
	}

    IEnumerator PerformShake(float duration, float amount){
        float ellapsedTime = 0;
        while (ellapsedTime < duration)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * amount;
            ellapsedTime += Time.deltaTime;
            yield return null;
        }
        camTransform.localPosition = originalPos;
    }
}
