using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private GameObject parentObject;

	private bool _isPaused;
	public bool IsPaused {
		get {
			return _isPaused;
		}

		set {
			_isPaused = value;

			parentObject.SetActive(_isPaused);
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
