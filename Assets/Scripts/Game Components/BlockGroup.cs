using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockGroup : MonoBehaviour {
	[Header(" --- Block Group Class ---")]
	[SerializeField] protected GameManager gameManager;
	[SerializeField] protected PauseManager pauseManager;
	[SerializeField] protected AudioSource audioSource;
	[Space]
	[SerializeField] private List<AudioClip> moveClips = new List<AudioClip>( );
	[Space]
	[SerializeField] protected List<BlockObject> connectedBlocks = new List<BlockObject>( );

	// Whether or not this group can move
	public bool CanMove {
		get {
			// If the game is paused, then blocks should not be able to move
			if (pauseManager.IsPaused) {
				return false;
			}

			// This group can only move if every block has finished moving
			foreach (BlockObject block in connectedBlocks) {
				// If one block can't move yet, then the entire group can't move
				if (!block.CanMove) {
					return false;
				}
			}

			return true;
		}
	}

	// Whether or not the block group has at least one block on solid ground
	public bool IsGrounded {
		get {
			foreach (BlockObject block in connectedBlocks) {
				// As long as one of them is, return true
				if (block.IsGrounded) {
					return true;
				}
			}

			return false;
		}
	}

	protected void OnValidate ( ) {
		if (gameManager == null) {
			gameManager = FindObjectOfType<GameManager>( );
		}

		if (pauseManager == null) {
			pauseManager = FindObjectOfType<PauseManager>( );
		}

		if (audioSource == null) {
			audioSource = GetComponent<AudioSource>( );
		}

		// Add all child block objects to the parent object
		// Duplicates will not be added
		foreach (BlockObject block in transform.GetComponentsInChildren<BlockObject>( )) {
			AddBlock(block);
		}
	}

	protected void Update ( ) {
		// If there are no more connected blocks in this group, then destroy it
		if (connectedBlocks.Count == 0) {
			Destroy(gameObject);
		}
	}

	/*
	 * Merge this group with another one
	 */
	public void MergeToGroup (BlockGroup blockGroup) {
		// An array to hold the blocks that were in the group that is going to be destroyed
		BlockObject[ ] tempBlocks;

		// Make sure the player group object always has priority and is never destroyed
		if (blockGroup is PlayerGroup) {
			// Remove all blocks from this group
			tempBlocks = new BlockObject[connectedBlocks.Count];
			connectedBlocks.CopyTo(tempBlocks);

			// Empty all of the blocks from the group
			RemoveAllBlocks( );

			// Add all of the blocks that were in this group to the other group
			blockGroup.AddBlocks(tempBlocks.ToList( ));

			// Destroy this group object
			Destroy( );
		} else {
			// Remove all blocks from the other group
			tempBlocks = new BlockObject[blockGroup.connectedBlocks.Count];
			blockGroup.connectedBlocks.CopyTo(tempBlocks);

			// Empty all of the blocks from the group
			blockGroup.RemoveAllBlocks( );

			// Add all of the blocks that were in the other group to this group
			AddBlocks(tempBlocks.ToList( ));

			// Destroy the other group object
			blockGroup.Destroy( );
		}
	}

	/*
	 * Move all of the blocks within this group
	 * 
	 * Vector2 direction				: The direction to move
	 */
	public void Move (Vector2 direction) {
		if (CanMove) {
			// A list for all the blocks that need to move when this group moves
			List<BlockObject> blocksToMove = new List<BlockObject>( );
			// A list for all the blocks that still need to be checked to see if this group is allowed to move
			List<BlockObject> blocksToCheck = new List<BlockObject>( );
			// A list of all the blocks that have already been checked
			List<BlockObject> blocksAlreadyChecked = new List<BlockObject>( );
			BlockObject tempBlock = null;

			// Add all of the current blocks in this group to the blocks that need to be checked
			blocksToCheck.AddRange(connectedBlocks);

			for (int i = blocksToCheck.Count - 1; i >= 0; i--) {
				if (blocksToCheck[i].IsDead) {
					return;
				}

				// If the block was already checked, do not try and check it again because that will cause an infinite loop
				if (!blocksAlreadyChecked.Contains(blocksToCheck[i])) {
					// If this block will run into a wall, then do not move the entire group
					if (blocksToCheck[i].CheckForWall(direction)) {
						return;
					}

					// If there is a block that is going to be pushed by this group's blocks and not stick to them, add them to a list
					if (!blocksToCheck[i].CheckForBlock(direction, out tempBlock, checkGroup: false)) {
						// Make sure there is an actual block there
						if (tempBlock != null) {
							// Add the blocks that will be pushed to the "will be pushed" array
							if (tempBlock.IsConnected) {
								foreach (BlockObject block in tempBlock.BlockGroup.connectedBlocks) {
									blocksToCheck.Insert(0, block);
									i++;
								}
							} else {
								blocksToCheck.Insert(0, tempBlock);
								i++;
							}
						}
					}

					blocksToMove.Add(blocksToCheck[i]);
					blocksAlreadyChecked.Add(blocksToCheck[i]);
				}

				// Remove the block that was just checked from the list
				blocksToCheck.RemoveAt(i);
			}

			PlayMoveSound( );

			// If all the blocks can be pushed, then move all of the blocks in the specified direction
			foreach (BlockObject block in blocksToMove) {
				block.Move(direction);
			}
		}
	}

	/*
	 * Add a block to this group
	 * 
	 * BlockObject block				: The block to add
	 */
	public void AddBlock (BlockObject block) {
		// Make sure the block was not already added to this group
		if (!connectedBlocks.Contains(block)) {
			connectedBlocks.Add(block);
		}

		// Make sure the blocks has this group as its group
		block.transform.SetParent(transform);
		block.BlockGroup = this;
	}

	/*
	 * Add a bunch of blocks to this group
	 * 
	 * List<BlockObject> blocks				: All the blocks to add
	 */
	public void AddBlocks (List<BlockObject> blocks) {
		// Simply loop through the list and add each block
		foreach (BlockObject block in blocks) {
			AddBlock(block);
		}
	}

	/*
	 * Remove a block from this group
	 * 
	 * BlockObject block				: The block to remove
	 */
	public void RemoveBlock (BlockObject block) {
		connectedBlocks.Remove(block);

		// Make sure the block is no longer in this group
		block.transform.SetParent(gameManager.transform);
		block.BlockGroup = null;
	}

	/*
	 * Remove every block from this group
	 */
	public void RemoveAllBlocks ( ) {
		// Loop through all the blocks in the group and remove them
		for (int i = connectedBlocks.Count - 1; i >= 0; i--) {
			RemoveBlock(connectedBlocks[i]);
		}
	}

	/*
	 * Destroy this group
	 */
	public void Destroy ( ) {
		// Destroy each of the blocks in the group
		for (int i = connectedBlocks.Count - 1; i >= 0; i--) {
			connectedBlocks[i].Destroy( );
		}
	}

	/*
	 * Play a random move sound
	 */
	private void PlayMoveSound ( ) {
		audioSource.clip = moveClips[Utils.Random.Next(0, moveClips.Count)];
		audioSource.Play( );
	}
}
