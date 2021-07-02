using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private TransitionManager transitionManager;
	[Space]
	[SerializeField] private Animator animator;
	[Space]
	[SerializeField] private Text levelTitle;
	[SerializeField] private Text levelNumber;

	private bool _isLevelComplete;
	public bool IsLevelComplete {
		get {
			return _isLevelComplete;
		}

		set {
			_isLevelComplete = value;

			animator.SetBool("IsLevelComplete", _isLevelComplete);
			SetTimeScale(_isLevelComplete ? 0 : 1);
		}
	}

	private bool _isPaused;
	public bool IsPaused {
		get {
			return _isPaused;
		}

		set {
			_isPaused = value;

			animator.SetBool("IsPaused", _isPaused);
			SetTimeScale(_isPaused ? 0 : 1);
		}
	}

	private void OnValidate ( ) {
		if (transitionManager == null) {
			transitionManager = FindObjectOfType<TransitionManager>( );
		}

		if (animator == null) {
			animator = GetComponent<Animator>( );
		}
	}

	private void Awake ( ) {
		/*
		if (GameManager.currentLevelInfo != null) {
			levelTitle.text = GameManager.currentLevelInfo.LevelName;
			levelNumber.text = $"Level {GameManager.currentLevelInfo.LevelNumber}";
		}
		*/
	}

	private void Update ( ) {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			IsPaused = !IsPaused;
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			ReloadScene( );
		}
	}

	public void SetTimeScale (int value) {
		Time.timeScale = value;
	}

	public void QuitGame ( ) {
		if (transitionManager != null) {
			transitionManager.QuitGame( );
		}
	}

	public void GoToMainMenu ( ) {
		if (transitionManager != null) {
			transitionManager.GoToMainMenu( );
		}
	}

	public void LoadNextLevel ( ) {
		if (transitionManager != null) {
			transitionManager.LoadNextLevel( );
		}
	}

	public void LoadLevel (int levelNumber) {
		if (transitionManager != null) {
			transitionManager.LoadLevel(levelNumber);
		}
	}

	public void ReloadScene ( ) {
		if (transitionManager != null) {
			transitionManager.ReloadScene( );
		}
	}
}
