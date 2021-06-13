using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {
	[Header(" --- Transition Manager Class ---")]
	[SerializeField] private Animator animator;

	private void OnValidate ( ) {
		if (animator == null) {
			animator = GetComponent<Animator>( );
		}	
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
		// Just in case
		Time.timeScale = 1;

		StartCoroutine(ILoadLevel(levelNumber));
	}

	public void ReloadScene ( ) {
		LoadLevel(SceneManager.GetActiveScene( ).buildIndex);
	}

	private IEnumerator ILoadLevel (int levelIndex) {
		animator.SetTrigger("Fade");

		yield return new WaitForSeconds(1);

		SceneManager.LoadScene(levelIndex);
	}
}
