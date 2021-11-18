using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region Singleton
	private static SoundManager _instance;
	public static SoundManager Instance { get { return _instance; } }
	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
			DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
    }
	#endregion

	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Stop(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Stop();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		Debug.Log("SceneLoaded");
		switch (scene.name)
		{
			case "TitleScene":
				Play("Title");
			break;
			case "WorldScene":
				Play("Home");
			break;
			case "BattleScene":
				Play("Battle");
			break;
		}
    }

}
