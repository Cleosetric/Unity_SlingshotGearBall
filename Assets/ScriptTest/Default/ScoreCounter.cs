using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    #region Singleton
	private static ScoreCounter _instance;
	public static ScoreCounter Instance { get { return _instance; } }

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
    public float timeScoring;
    public float timeDelay;
    public int score;
    public int displayScore;
    public TextMeshProUGUI textScore;
    private Coroutine scoreAnim;

    private void Start() {
        score = 0;
        displayScore = 0;
        textScore.SetText(displayScore.ToString());
    }

    public void IncreaseScore(int scoreToAdd){
        if(scoreAnim != null) StopCoroutine(scoreAnim);
        scoreAnim =  StartCoroutine(ScoreUpdater(scoreToAdd));
    }

    private IEnumerator ScoreUpdater(int addScore)
    {
        score += addScore;
        float animationTime = 0f;
        while (animationTime < timeScoring)
        {
            animationTime += Time.unscaledDeltaTime;
            if(displayScore < score)
            {
                displayScore ++; //Increment the display score by 1
                textScore.SetText(displayScore.ToString()); //Write it to the UI
            }
            yield return new WaitForSeconds(timeDelay);
        }
        displayScore = score;
        textScore.SetText(displayScore.ToString());
        scoreAnim = null;
    }
}
