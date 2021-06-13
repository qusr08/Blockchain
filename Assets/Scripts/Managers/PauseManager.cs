using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private Canvas canvas;

	private bool _isPaused;
	public bool IsPaused {
		get {
			return _isPaused;
		}

		set {
			_isPaused = value;

			canvas.enabled = _isPaused;
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
}
