using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private TransitionManager transitionManager;
	[SerializeField] private Animator animator;

	private bool _isLevelComplete;
	public bool IsLevelComplete {
		get {
			return _isLevelComplete;
		}

		set {
			_isLevelComplete = value;

			if (_isLevelComplete) {
				animator.SetTrigger("IsComplete");
			}
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

	private void Update ( ) {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			IsPaused = !IsPaused;
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			transitionManager.ReloadScene( );
		}
	}

	public void SetTimeScale (int value) {
		Time.timeScale = value;
	}

	public void QuitGame ( ) {
		transitionManager.QuitGame( );
	}

	public void GoToMainMenu ( ) {
		transitionManager.GoToMainMenu( );
	}

	public void LoadNextLevel ( ) {
		transitionManager.LoadNextLevel( );
	}

	public void LoadLevel (int levelNumber) {
		transitionManager.LoadLevel(levelNumber);
	}

	public void ReloadScene ( ) {
		transitionManager.ReloadScene( );
	}
}
