using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroup : BlockGroup {
	public LevelData CurrentLevelData;
	public float CurrentLevelTime;
	public float CurrentLevelMoves;

	private bool isLevelComplete;

	private void Awake ( ) {
		CurrentLevelData = SaveSystem.Load(LevelManager.CurrentLevelNumber);
	}

	private new void Update ( ) {
		base.Update( );

		// Move the player with keyboard input
		MoveWithKeyboard( );

		if (!levelManager.IsLevelComplete && !isLevelComplete) {
			CurrentLevelTime += Time.deltaTime;
		} else {
			isLevelComplete = true;

			// Update the best scores for this level before saving
			CurrentLevelData.BestCompletionTime = Mathf.Min(CurrentLevelTime, CurrentLevelData.BestCompletionTime);
			CurrentLevelData.LeastMovesTaken = (int) Mathf.Min(CurrentLevelMoves, CurrentLevelData.LeastMovesTaken);

			SaveSystem.Save(this);
		}
	}

	private void OnDestroy ( ) {
		pauseManager.ReloadScene( );
	}

	private void MoveWithKeyboard ( ) {
		if (isLevelComplete) {
			return;
		}

		Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		bool didMove = false;

		if (move.x > 0) {
			didMove = Move(Vector2.right);
		} else if (move.x < 0) {
			didMove = Move(Vector2.left);
		} else if (move.y > 0) {
			didMove = Move(Vector2.up);
		} else if (move.y < 0) {
			didMove = Move(Vector2.down);
		}

		if (didMove) {
			CurrentLevelMoves++;
		}
	}
}
