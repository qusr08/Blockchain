using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundEffectType {
	BLOCK_DEATH,
	BUTTON,
	WIN,
	BLOCK_MOVE
}

[System.Serializable]
public class NestedList<T> {
	public List<T> List;
}

public class GameManager : MonoBehaviour {
	public const float AUDIO_MAX_VOLUME = 0.1f;
	public const float MUSIC_PAUSE_DURATION = 10;
	public const float MUSIC_FADE_DURATION = 4;
	public const float EFFECTS_REPEAT_DELAY = 0.2f;

	private static LevelInfo currentLevelInfo;
	private static GameManager instance;

	[Header(" --- Game Manager Class ---")]
	[SerializeField] private AudioSource musicAudioSource;
	[SerializeField] private AudioSource effectsAudioSource;
	[Space]
	[SerializeField] private List<AudioClip> music = new List<AudioClip>( );
	[SerializeField] private int currentSongIndex = 0;
	[Space]
	[SerializeField] private List<NestedList<AudioClip>> effects = new List<NestedList<AudioClip>>( );

	private float musicPauseTime = 0;
	private bool[ ] effectsActive;

	private void OnValidate ( ) {
		if (musicAudioSource == null) {
			musicAudioSource = transform.Find("Music Audio").GetComponent<AudioSource>( );
		}

		if (effectsAudioSource == null) {
			effectsAudioSource = transform.Find("Effects Audio").GetComponent<AudioSource>( );
		}
	}

	private void Awake ( ) {
		// Have it so the manager object does not get destroyed when switching scenes
		DontDestroyOnLoad(transform.gameObject);

		// Make sure there is only one instance of the manager class present in the game
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	private void Start ( ) {
		effectsActive = new bool[effects.Count];
	}

	private void Update ( ) {
		if (!musicAudioSource.isPlaying && musicPauseTime <= 0) {
			StartCoroutine(IFadeInMusic( ));
		}

		if (musicAudioSource.clip.length - musicAudioSource.time < MUSIC_FADE_DURATION) {
			StartCoroutine(IFadeOutMusic( ));
		}

		if (musicPauseTime > 0) {
			musicPauseTime -= Time.unscaledDeltaTime;
		}
	}

	public void PlayRandomSong ( ) {
		if (musicAudioSource.isPlaying) {
			return;
		}

		musicPauseTime = 0;

		// Get a random song to play
		currentSongIndex = Utils.GetRandInt(0, music.Count, new List<int> { currentSongIndex });
		musicAudioSource.clip = music[currentSongIndex];

		// Start playing the song
		musicAudioSource.Play( );
	}

	public void PlaySoundEffect (SoundEffectType effect) {
		if (!effectsActive[(int) effect]) {
			NestedList<AudioClip> effectList = effects[(int) effect];
			effectsAudioSource.PlayOneShot(Utils.Choose(effectList.List.ToArray( )));

			StartCoroutine(IDelaySoundEffect(effect));
		}
	}

	public void StopMusic ( ) {
		musicAudioSource.Stop( );

		musicPauseTime = MUSIC_PAUSE_DURATION;
	}

	private IEnumerator IDelaySoundEffect (SoundEffectType effect) {
		effectsActive[(int) effect] = true;
		float t = 0;

		while (t < EFFECTS_REPEAT_DELAY) {
			t += Time.unscaledDeltaTime;

			yield return null;
		}

		effectsActive[(int) effect] = false;

		yield break;
	}

	private IEnumerator IFadeOutMusic ( ) {
		float currentTime = 0;

		while (currentTime < MUSIC_FADE_DURATION) {
			currentTime += Time.unscaledDeltaTime;
			musicAudioSource.volume = Mathf.Lerp(AUDIO_MAX_VOLUME, 0, currentTime / MUSIC_FADE_DURATION);

			yield return null;
		}

		StopMusic( );

		yield break;
	}

	private IEnumerator IFadeInMusic ( ) {
		float currentTime = 0;

		PlayRandomSong( );

		while (currentTime < MUSIC_FADE_DURATION) {
			currentTime += Time.unscaledDeltaTime;
			musicAudioSource.volume = Mathf.Lerp(0, AUDIO_MAX_VOLUME, currentTime / MUSIC_FADE_DURATION);

			yield return null;
		}

		yield break;
	}
}
