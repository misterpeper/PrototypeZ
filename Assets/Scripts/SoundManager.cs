using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	private static readonly string FirstPlay = "FirstPlay";
	private static readonly string MusicPref = "MusicPref";
	private int firstPlayInt;
	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;

	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	public AudioMixer mixer;
	public Slider volSlider;

	// Singleton instance.
	public static SoundManager Instance = null;

	[SerializeField] private AudioClip mainMusic;
	public float defaulValue;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

	public void Start()
	{
		firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

		if (firstPlayInt == 0)
        {
			defaulValue = 1.0f;
			volSlider.value = defaulValue;
			PlayerPrefs.SetFloat(MusicPref, defaulValue);
			PlayerPrefs.SetInt(FirstPlay, -1);
        }
		else
        {
			defaulValue = PlayerPrefs.GetFloat(MusicPref);
			volSlider.value = defaulValue;
		}

		PlayMusic(mainMusic);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip)
	{
		EffectsSource.clip = clip;
		EffectsSource.Play();
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		EffectsSource.pitch = randomPitch;
		EffectsSource.clip = clips[randomIndex];
		EffectsSource.Play();
	}

	public void SaveSoundSettings()
    {
		PlayerPrefs.SetFloat(MusicPref, volSlider.value);
    }

    private void OnApplicationFocus(bool inFocus)
    {
        if(!inFocus)
        {
			SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
		MusicSource.volume = volSlider.value;
    }
}