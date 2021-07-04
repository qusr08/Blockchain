using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	[Header(" --- Main Menu Manager --- ")]
	[SerializeField] private Transform levelButtonsParent;

	private void OnValidate ( ) {
		// Update all level button names
		for (int i = 0; i < levelButtonsParent.childCount; i++) {
			string levelName = "???";
			LevelManager.LEVEL_NAMES.TryGetValue(i + 1, out levelName);

			levelButtonsParent.GetChild(i).GetComponentInChildren<Text>( ).text = levelName;
		}
	}
}
