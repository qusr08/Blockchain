using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public static Dictionary<int, string> LEVEL_NAMES = new Dictionary<int, string>( ) {
		[1] = "Starship One",
		[2] = "A Stone's Throw",
		[3] = "Reaching Out",
		[4] = "A Sticky Situation",
		[5] = "Hook, Line, and Sinker",
		[6] = "Take a U-Turn",
		[7] = "Time is Fleeting",
		[8] = "Push Off",
		[9] = "Double Trouble",
		[10] = "Through The Back Door",
		[11] = "Strike Through The Heart",
		[12] = "Drop-Off",
		[13] = "Crossing The Abyss",
		[14] = "Tied Hands",
		[15] = "Jailbreak",
		[16] = "Matching Frame",
		[17] = "Pushing Your Limits"
	};

	[Header(" --- Main Menu Manager --- ")]
	[SerializeField] private Transform levelButtonsParent;

	private void OnValidate ( ) {
		// Update all level button names
		for (int i = 0; i < levelButtonsParent.childCount; i++) {
			string levelName = "???";
			LEVEL_NAMES.TryGetValue(i + 1, out levelName);

			levelButtonsParent.GetChild(i).GetComponentInChildren<Text>( ).text = levelName;
		}
	}


}
