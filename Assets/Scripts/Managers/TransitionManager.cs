using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {
	public const float TRANSITION_FADE_DURATION = 1f;

	[Header(" --- Transition Manager Class ---")]
	[SerializeField] private Animator animator;
	[SerializeField] private Canvas canvas;

	private void OnValidate ( ) {
		if (animator == null) {
			animator = GetComponent<Animator>( );
		}

		if (canvas == null) {
			canvas = GetComponent<Canvas>( );
		}

		canvas.enabled = false;
	}

	private void Awake ( ) {
		canvas.enabled = true;
	}

	public void QuitGame ( ) {
		Application.Quit( );
	}

	public void GoToMainMenu ( ) {
		LoadLevel(0);
	}

	public void LoadNextLevel ( ) {
		if (SceneManager.GetActiveScene( ).buildIndex + 1 >= SceneManager.sceneCountInBuildSettings) {
			GoToMainMenu( );
		} else {
			LoadLevel(SceneManager.GetActiveScene( ).buildIndex + 1);
		}
	}

	public void LoadLevel (int levelNumber) {
		// Just in case, reset the timescale
		Time.timeScale = 1;

		StartCoroutine(ILoadLevel(levelNumber));
	}

	public void ReloadScene ( ) {
		LoadLevel(SceneManager.GetActiveScene( ).buildIndex);
	}

	private IEnumerator ILoadLevel (int levelIndex) {
		animator.SetTrigger("Fade");

		yield return new WaitForSeconds(TRANSITION_FADE_DURATION);

		LevelManager.CurrentLevelNumber = levelIndex;

		SceneManager.LoadScene(levelIndex);
	}
}
