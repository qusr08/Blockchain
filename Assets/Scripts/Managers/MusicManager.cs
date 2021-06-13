using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
	[Header(" --- Music Manager Class ---")]
	[SerializeField] private AudioSource audioSource;
	[Space]
	[SerializeField] private List<AudioClip> music = new List<AudioClip>( );

	private static MusicManager instance;

	private void Awake ( ) {
		DontDestroyOnLoad(transform.gameObject);

		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	private void Start ( ) {
		PlayMusic( );
	}

	private void Update ( ) {
		if (!audioSource.isPlaying) {
			PlayMusic(delay: 10f);
		}
	}

	public void PlayMusic (float delay = 0) {
		if (audioSource.isPlaying) {
			return;
		}

		GetMusicTrack( );
		audioSource.PlayDelayed(delay);
	}

	public void StopMusic ( ) {
		audioSource.Stop( );
	}

	private void GetMusicTrack ( ) {
		audioSource.clip = music[Utils.Random.Next(0, music.Count)];
	}
}
