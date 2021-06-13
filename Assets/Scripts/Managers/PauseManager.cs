using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private Canvas canvas;
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
		if (canvas == null) {
			canvas = GetComponent<Canvas>( );
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
			ReloadScene( );
		}
	}

	public void SetTimeScale (int value) {
		Time.timeScale = value;
	}

	public void LoadNextLevel ( ) {
		SetTimeScale(1);

		if (SceneManager.GetActiveScene( ).buildIndex + 1 >= SceneManager.sceneCountInBuildSettings) {
			GoToMainMenu( );
		} else {
			SceneManager.LoadScene(SceneManager.GetActiveScene( ).buildIndex + 1);
		}
	}

	public void ReloadScene ( ) {
		SetTimeScale(1);

		SceneManager.LoadScene(SceneManager.GetActiveScene( ).buildIndex);
	}

	public void GoToMainMenu ( ) {
		SetTimeScale(1);

		SceneManager.LoadScene(0);
	}

	public void QuitGame ( ) {
		Application.Quit( );
	}
}
