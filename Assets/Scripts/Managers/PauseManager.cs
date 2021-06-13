using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private GameObject levelCompleteLayout;
	[SerializeField] private GameObject pauseLayout;
	[SerializeField] private CanvasGroup pauseGroup;

	private bool _isLevelComplete;
	public bool IsLevelComplete {
		get {
			return _isLevelComplete;
		}

		set {
			_isLevelComplete = value;

			if (_isLevelComplete) {
				IsPaused = false;
			}

			pauseGroup.alpha = _isLevelComplete ? 1 : 0;
			levelCompleteLayout.SetActive(_isLevelComplete);
			Time.timeScale = _isPaused ? 0 : 1;
		}
	}

	private bool _isPaused;
	public bool IsPaused {
		get {
			return _isPaused;
		}

		set {
			_isPaused = value;

			pauseGroup.alpha = _isPaused ? 1 : 0;
			pauseLayout.SetActive(_isPaused);
			Time.timeScale = _isPaused ? 0 : 1;
		}
	}

	private void OnValidate ( ) {
		if (canvas == null) {
			canvas = GetComponent<Canvas>( );
		}

		if (GameObject.Find("Camera") != null) {
			canvas.worldCamera = GameObject.Find("Camera").GetComponent<Camera>( );
		}

		IsPaused = false;
	}

	private void Update ( ) {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			IsPaused = !IsPaused;
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			ReloadScene( );
		}
	}

	public void LoadNextLevel ( ) {
		Time.timeScale = 1;

		if (SceneManager.GetActiveScene( ).buildIndex + 1 >= SceneManager.sceneCount) {
			GoToMainMenu( );
		} else {
			SceneManager.LoadScene(SceneManager.GetActiveScene( ).buildIndex + 1);
		}
	}

	public void ReloadScene ( ) {
		Time.timeScale = 1;

		SceneManager.LoadScene(SceneManager.GetActiveScene( ).buildIndex);
	}

	public void GoToMainMenu ( ) {
		// Make sure to reset the time scale when going back to the titlescreen
		Time.timeScale = 1;

		SceneManager.LoadScene(0);
	}

	public void QuitGame ( ) {
		Application.Quit( );
	}
}
