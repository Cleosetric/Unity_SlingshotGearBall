using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    #region Singleton
	private static LevelLoader _instance;
	public static LevelLoader Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
	#endregion
    
    public Animator transition;
    public TextMeshProUGUI textPercentage;
    private AsyncOperation operation;

    private void Start() {
        transition.SetBool("Loading", false);
    }

    public void LoadScene(string sceneName)
	{
        transition.SetBool("Loading", true);
		StartCoroutine(BeginLoad(sceneName));
	}

	private IEnumerator BeginLoad(string sceneName)
	{
        textPercentage.SetText("0%");
        yield return new WaitForSeconds(1f);
		operation = SceneManager.LoadSceneAsync(sceneName);

		while (!operation.isDone)
		{
            textPercentage.SetText((int)(operation.progress * 100f) + "%");
			yield return null;
		}
        yield return new WaitForSeconds(1f);
        textPercentage.SetText("100%");
        transition.SetBool("Loading", false);
		operation = null;
	}
}
