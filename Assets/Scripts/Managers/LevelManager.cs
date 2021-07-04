using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
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

	public static int CurrentLevelNumber;

	[Header(" --- Level Manager Class --- ")]
	[SerializeField] private GameObject blockGroupPrefab;
	[Space]
	[SerializeField] private List<ButtonObject> buttons = new List<ButtonObject>( );
	[SerializeField] private List<DoorObject> doors = new List<DoorObject>( );
	[SerializeField] private List<BlockObject> blocks = new List<BlockObject>( );
	[Space]
	[SerializeField] public bool IsLevelComplete;

	private void OnValidate ( ) {
		blocks.Clear( );
		blocks.AddRange(FindObjectsOfType<BlockObject>( ));

		doors.Clear( );
		doors.AddRange(FindObjectsOfType<DoorObject>( ));

		buttons.Clear( );
		buttons.AddRange(FindObjectsOfType<ButtonObject>( ));
	}

	/*
	 * Check for a block at a certain position
	 * 
	 * Vector3 position					: The position to check
	 */
	public bool CheckForBlockAtPosition (Vector3 position) {
		foreach (BlockObject block in blocks) {
			if (block == null) {
				continue;
			}

			// If one of the blocks is within range of the position, then there is a block there
			if (Vector3.Distance(block.transform.position, position) <= 0.5) {
				return true;
			}
		}

		return false;
	}

	/*
	 * Create a new block group
	 * 
	 * List<BlockObject> blocks				: The blocks to add to the group initially
	 */
	public void CreateGroup (List<BlockObject> blocks) {
		// Instatiate a new group object
		BlockGroup newGroup = Instantiate(blockGroupPrefab, transform, true).GetComponent<BlockGroup>( );

		// Add the blocks to the group
		newGroup.AddBlocks(blocks);
	}
}
