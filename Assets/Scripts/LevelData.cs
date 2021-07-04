using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {
	public string LevelName;
	public int LevelNumber;
	public float BestCompletionTime;
	public int LeastMovesTaken;

	public LevelData (PlayerGroup player) {
		LevelName = player.CurrentLevelData.LevelName;
		LevelNumber = player.CurrentLevelData.LevelNumber;
		BestCompletionTime = player.CurrentLevelData.BestCompletionTime;
		LeastMovesTaken = player.CurrentLevelData.LeastMovesTaken;
	}

	public LevelData (int levelNumber) {
		LevelName = LevelManager.LEVEL_NAMES[levelNumber];
		LevelNumber = levelNumber;
		BestCompletionTime = float.MaxValue;
		LeastMovesTaken = int.MaxValue;
	}
}
