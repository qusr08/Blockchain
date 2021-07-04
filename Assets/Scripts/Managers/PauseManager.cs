using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {
	[Header(" --- Pause Manager Class ---")]
	[SerializeField] private TransitionManager transitionManager;
	[SerializeField] private LevelManager levelManager;
	[SerializeField] private PlayerGroup player;
	[Space]
	[SerializeField] private Animator animator;
	[Space]
	[SerializeField] private Text levelTitle;
	[SerializeField] private Text levelNumber;
	[SerializeField] private Text bestTime;
	[SerializeField] private GameObject bestTimeNewRecordText;
	[SerializeField] private Text leastMoves;
	[SerializeField] private GameObject leastMovesNewRecordText;

	private bool _isLevelComplete;
	public bool IsLevelComplete {
		get {
			return _isLevelComplete;
		}

		set {
			_isLevelComplete = value;
			levelManager.IsLevelComplete = value;

			animator.SetBool("IsLevelComplete", _isLevelComplete);
			SetTimeScale(_isLevelComplete ? 0 : 1);

			// If the level has been completed, update the best moves + time
			if (value) {
				float minimumTime = Mathf.Min(player.CurrentLevelTime, player.CurrentLevelData.BestCompletionTime);
				bestTimeNewRecordText.SetActive(minimumTime < player.CurrentLevelData.BestCompletionTime);
				bestTime.text = $"BEST TIME - {TimeSpan.FromSeconds(minimumTime).ToString(@"mm\:ss\:fff")}";

				int minimumMoves = (int) Mathf.Min(player.CurrentLevelMoves, player.CurrentLevelData.LeastMovesTaken);
				leastMovesNewRecordText.SetActive(minimumMoves < player.CurrentLevelData.LeastMovesTaken);
				leastMoves.text = $"LEAST MOVES - {minimumMoves}";
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

		if (levelManager == null) {
			levelManager = FindObjectOfType<LevelManager>( );
		}

		if (animator == null) {
			animator = GetComponent<Animator>( );
		}

		if (GameObject.Find("Player Block Group") != null) {
			player = GameObject.Find("Player Block Group").GetComponent<PlayerGroup>( );
		}
	}

	private void Start ( ) {
		if (player.CurrentLevelData != null) {
			levelTitle.text = player.CurrentLevelData.LevelName;
			levelNumber.text = $"LEVEL {player.CurrentLevelData.LevelNumber}";

			if (player.CurrentLevelData.BestCompletionTime == float.MaxValue) {
				bestTime.text = $"BEST TIME - NA";
			} else {
				bestTime.text = $"BEST TIME - {TimeSpan.FromSeconds(player.CurrentLevelData.BestCompletionTime).ToString(@"mm\:ss\:fff")}";
			}

			if (player.CurrentLevelData.LeastMovesTaken == int.MaxValue) {
				bestTime.text = $"BEST TIME - NA";
			} else {
				leastMoves.text = $"LEAST MOVES - {player.CurrentLevelData.LeastMovesTaken}";
			}
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
