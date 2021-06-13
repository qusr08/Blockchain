using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum BlockType {
	Player, Green, Blue, Red, Purple
}

public class BlockObject : GameComponent {
	public static float BLOCK_MOVE_STEPDUR = 0.15f;
	public static int BLOCK_ALT_SPRITE_INDEX = 4;

	private int[ , ] blockRuleMatrix = new int[ , ] {
		{ 0, 1, 1, 0, 0},
		{ 1, 1, 1, 0, 1},
		{ 1, 1, 1, 0, 1},
		{ 0, 0, 0, 0, 1},
		{ 0, 1, 1, 1, 1}
	};

	[Header(" --- Block Object Class --- ")]
	[SerializeField] private Tilemap floorTilemap;
	[Space]
	[SerializeField] public BlockType BlockType;
	[SerializeField] private List<Sprite> playerSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> greenSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> blueSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> redSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> purpleSprites = new List<Sprite>( );
	[Space]
	[SerializeField] public BlockGroup BlockGroup;

	// Whether or not the block is part of a group or not
	public bool IsConnected {
		get {
			return (BlockGroup != null);
		}
	}

	// Wether or not the block is on the ground
	public bool IsGrounded {
		get {
			return (floorTilemap.GetTile(floorTilemap.WorldToCell(transform.position)) != null);
		}
	}

	// Whether or not the block can move
	public bool CanMove {
		get {
			return (movement == null);
		}
	}

	private bool _canStick = true;
	public bool CanStick {
		get {
			return _canStick;
		}

		set {
			_canStick = value;

			if (!_canStick) {
				spriteRenderer.sprite = blueSprites[BLOCK_ALT_SPRITE_INDEX];
			}
		}
	}

	private bool _isDead;
	public bool IsDead {
		get {
			return _isDead;
		}

		set {
			_isDead = value;

			if (_isDead) {
				animator.SetTrigger("IsDead");
			}
		}
	}

	private Coroutine movement;

	private new void OnValidate ( ) {
		base.OnValidate( );

		if (floorTilemap == null) {
			if (GameObject.Find("Floor") != null) {
				floorTilemap = GameObject.Find("Floor").GetComponent<Tilemap>( );
			}
		}

		// Change the sprite of the block based on its type
		SetSpriteFrame(0);

		// Update the name of the block
		name = $"{BlockType} Block";
	}

	private void Start ( ) {
		// Update all the blocks in the scene to form groups if they start next to each other
		CheckSurroundingBlocks( );
	}

	/*
	 * Move 1 tile in a certain direction over time
	 * 
	 * Vector2 direction				: The direction to move
	 */
	protected IEnumerator IMove (Vector2 direction) {
		// Get the starting and ending positions of the movement
		Vector2 startPosition = transform.position;
		Vector2 toPosition = transform.position + (Vector3) direction;

		// Move the block gradually to the ending position
		float t = 0.0f;
		while (t < 1.0f) {
			transform.position = Vector2.Lerp(startPosition, toPosition, t);
			t += Time.deltaTime / BLOCK_MOVE_STEPDUR;

			yield return new WaitForEndOfFrame( );
		}

		// Set the transform at the end to get rid of any inconsistancies
		transform.position = toPosition;
		movement = null;

		// Check to see if the group that this block is part of has been destroyed or not
		if (BlockGroup == null) {
			if (!IsGrounded) {
				Destroy( );
				yield break;
			}
		} else {
			if (!BlockGroup.IsGrounded) {
				BlockGroup.Destroy( );
				yield break;
			}
		}

		// Check the surrounding blocks to see if any groups/combinations need to happen
		CheckSurroundingBlocks( );
	}

	/*
	 * Move the block in a certain direction
	 * 
	 * Vector2 direction				: The direction to move
	 */
	public void Move (Vector2 direction) {
		if (movement == null && !IsDead) {
			movement = StartCoroutine(IMove(direction));
		}
	}

	/*
	 * Check the surrounding space to see if blocks are there and perform actions on them accordingly
	 */
	private void CheckSurroundingBlocks ( ) {
		// A list to contain all the blocks that surround this block
		List<BlockObject> surroundingBlocks = new List<BlockObject>( );
		BlockObject tempBlock = null;

		// Check for blocks in all 4 directions around this block
		if (CheckForBlock(Vector2.up, out tempBlock)) {
			surroundingBlocks.Add(tempBlock);
		}
		if (CheckForBlock(Vector2.right, out tempBlock)) {
			surroundingBlocks.Add(tempBlock);
		}
		if (CheckForBlock(Vector2.down, out tempBlock)) {
			surroundingBlocks.Add(tempBlock);
		}
		if (CheckForBlock(Vector2.left, out tempBlock)) {
			surroundingBlocks.Add(tempBlock);
		}

		// Make sure there is at least one block around this block
		if (surroundingBlocks.Count > 0) {
			foreach (BlockObject block in surroundingBlocks) {
				// If this block is connected, then either merge the block groups or add the other blocks to this block group
				if (IsConnected) {
					// If the other block has a different block group, the groups need to be merged
					if (block.IsConnected && CheckForDifferentGroups(block)) {
						BlockGroup.MergeToGroup(block.BlockGroup);
					} else {
						BlockGroup.AddBlock(block);
					}
				} else {
					// If the other block has a different block group, the groups need to be merged
					if (block.IsConnected && CheckForDifferentGroups(block)) {
						block.BlockGroup.AddBlock(this);
					} else {
						// Create a new group if both blocks do not have groups
						gameManager.CreateGroup(new List<BlockObject> { this, block });
					}
				}
			}
		}
	}

	/*
	 * Check to see if there is a block in a certain direction around this block
	 * 
	 * Vector2 direction					: The direction to check
	 * out BlockObject block				: The block that (might) be there
	 * bool checkGroup						: Whether or not to check and see if the block groups are different
	 * bool checkRules						: Whether or not to check the block rules 
	 */
	public bool CheckForBlock (Vector2 direction, out BlockObject block, bool checkGroup = true, bool checkRules = true) {
		// Send a raycast out from this block in the specified direction
		RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, 1);
		block = null;

		// If the raycast hit a block, check to see if the block is valid based on the parameters
		if (hit2D && hit2D.transform.tag.Equals("Block")) {
			block = hit2D.transform.GetComponent<BlockObject>( );
			bool passedTests = true;

			// Check to see if the groups are different
			if (checkGroup) {
				passedTests = CheckForDifferentGroups(block);
			}

			// Check the block rules
			if (checkRules) {
				passedTests = CheckBlockRules(block);
			}

			return passedTests;
		}

		return false;
	}

	/*
	 * Check to see if a wall is in a certain direction
	 * 
	 * Vector2 direction				: The direction to check
	 */
	public bool CheckForWall (Vector2 direction) {
		// Send a raycast out from this block in the specified direction
		RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, 1);

		// If something was hit and the tags are different, it can be assumed to be a wall
		if (hit2D) {
			return hit2D.transform.tag.Equals("Door");
		}

		return false;
	}

	/*
	 * Check the rules matrix to make sure this block and another block can stick together
	 * 
	 * BlockObject block					: The other block to compare
	 */
	public bool CheckBlockRules (BlockObject block) {
		// If the value in the array is 1, then the blocks can stick together
		int ruleValue = blockRuleMatrix[(int) BlockType, (int) block.BlockType];

		if (ruleValue == 1) {
			// Check to see if this block or the other block is a blue block that can no longer stick to anything
			// If that is the case, then it should be pushed and not added to the group
			if (block.BlockType == BlockType.Blue) {
				if (!block.CanStick || !CanStick) {
					return false;
				} else {
					block.CanStick = false;
				}
			}
			if (BlockType == BlockType.Blue) {
				if (!block.CanStick || !CanStick) {
					return false;
				} else {
					CanStick = false;
				}
			}
		}

		return (ruleValue == 1);
	}

	/*
	 * Check to make sure the groups of two different blocks are different
	 * 
	 * BlockObject block					: The other block to compare to
	 */
	public bool CheckForDifferentGroups (BlockObject block) {
		// If one of the block groups are null, that counts as having different block groups
		return (block.BlockGroup == null || BlockGroup == null || block.BlockGroup != BlockGroup);
	}

	/*
	 * Destroy this block
	 */
	public void Destroy ( ) {
		if (!IsDead) {
			IsDead = true;
		}
	}

	/*
	 * Set the sprite of the block
	 * 
	 * int index						: The index of the frame to show
	 */
	public override void SetSpriteFrame (int index) {
		// If the index is out of range of the array, then quit because that shouldn't happen
		if (index < 0 || index >= playerSprites.Count) {
			Destroy(gameObject);
			return;
		}

		// Set the sprite of the block to a certain frame
		switch (BlockType) {
			case BlockType.Player:
				spriteRenderer.sprite = playerSprites[index];
				break;
			case BlockType.Green:
				spriteRenderer.sprite = greenSprites[index];
				break;
			case BlockType.Blue:
				spriteRenderer.sprite = blueSprites[index];
				break;
			case BlockType.Red:
				spriteRenderer.sprite = redSprites[index];
				break;
			case BlockType.Purple:
				spriteRenderer.sprite = purpleSprites[index];
				break;
		}
	}
}
