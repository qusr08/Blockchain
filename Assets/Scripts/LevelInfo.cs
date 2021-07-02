using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo {
	public string LevelName;
	public int LevelNumber;
	public float BestCompletionTime;
	public int LeastMovesTaken;

	public LevelInfo (string levelName, int levelNumber) {
		LevelName = levelName;
		LevelNumber = levelNumber;
	}
}
