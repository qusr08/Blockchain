using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
	/*
	public static void Save ( ) {
		BinaryFormatter formatter = new BinaryFormatter( );

		LevelInfo levelInfo = GameManager.currentLevelInfo;
		string path = Application.persistentDataPath + $"/{levelInfo.LevelNumber}.level";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, levelInfo);
		stream.Close( );
	}

	public static void Load (int levelNumber) {
		LevelInfo levelInfo = null;
		string path = Application.persistentDataPath + $"/{levelNumber}.level";

		if (!File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter( );
			FileStream stream = new FileStream(path, FileMode.Open);

			levelInfo = formatter.Deserialize(stream) as LevelInfo;
			stream.Close( );
		} else {
			Debug.LogError($"Save file not found! {path}");
		}

		GameManager.currentLevelInfo = levelInfo;
	}
	*/
}
