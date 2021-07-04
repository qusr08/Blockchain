using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
	public static void Save (PlayerGroup player) {
		BinaryFormatter formatter = new BinaryFormatter( );

		string path = Application.persistentDataPath + $"/{player.CurrentLevelData.LevelNumber}.level";
		FileStream stream = new FileStream(path, FileMode.Create);

		LevelData levelData = new LevelData(player);
		formatter.Serialize(stream, levelData);
		stream.Close( );
	}

	public static LevelData Load (int levelNumber) {
		LevelData levelData = null;
		string path = Application.persistentDataPath + $"/{levelNumber}.level";

		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter( );
			FileStream stream = new FileStream(path, FileMode.Open);

			levelData = formatter.Deserialize(stream) as LevelData;
			stream.Close( );
		} else {
			levelData = new LevelData(levelNumber);
		}

		return levelData;
	}
}
