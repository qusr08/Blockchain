using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType {
	Red, Orange, Blue, White
}

public enum DoorOrientationType {
	Vertical, Horizontal
}

public class DoorObject : GameComponent {
	[Header(" --- Door Object Class --- ")]
	[SerializeField] public DoorType DoorType;
	[SerializeField] public DoorOrientationType DoorOrientationType;
	[SerializeField] private List<Sprite> redDoorHorizontalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> orangeDoorHorizontalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> blueDoorHorizontalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> whiteDoorHorizontalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> redDoorVerticalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> orangeDoorVerticalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> blueDoorVerticalSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> whiteDoorVerticalSprites = new List<Sprite>( );
	[Space]
	[SerializeField] private List<ButtonObject> buttons = new List<ButtonObject>( );

	private bool isFullyOpened;
	public bool IsOpen {
		get {
			// If the door is already fully opened, then the door is opened
			if (isFullyOpened) {
				return true;
			}

			// If there are no buttons connected to the door, make sure the door always stays closed
			// Also, if the door is white, it cannot be opened
			if (buttons.Count == 0 || DoorType == DoorType.White) {
				return false;
			}

			// Check to see if all the buttons are pressed
			bool allButtonsPressed = true;
			foreach (ButtonObject button in buttons) {
				if (!button.IsPressed) {
					allButtonsPressed = false;
				}
			}

			// Based on the door type and whether or not all the buttons are pressed, determine if the door is open or not
			switch (DoorType) {
				case DoorType.Red:
					if (allButtonsPressed) {
						isFullyOpened = true;

						foreach (ButtonObject button in buttons) {
							button.IsFullyPressed = true;
						}

						return true;
					}

					break;
				case DoorType.Orange:
					if (allButtonsPressed) {
						return true;
					}

					break;
				case DoorType.Blue:
					if (allButtonsPressed) {
						isFullyOpened = true;

						foreach (ButtonObject button in buttons) {
							button.IsFullyPressed = true;
						}

						return true;
					}

					break;
			}

			return false;
		}
	}

	private new void OnValidate ( ) {
		base.OnValidate( );

		buttons.Clear( );
		if (DoorType != DoorType.White) {
			foreach (ButtonObject button in FindObjectsOfType(typeof(ButtonObject))) {
				if (button.name == $"{DoorType} Button") {
					buttons.Add(button);
				}
			}
		}

		SetSpriteFrame(0);

		name = $"{DoorType} Door";
	}

	private void Update ( ) {
		// Update the door sprites based on the value of its buttons, and whether or not it is open
		animator.SetBool("IsOpen", IsOpen);
		thisCollider2D.enabled = !IsOpen;
	}

	/*
	 * Set the sprite of the block
	 * 
	 * int index						: The index of the frame to show
	 */
	public override void SetSpriteFrame (int index) {
		// If the index is out of range of the array, then quit because that shouldn't happen
		if (index < 0 || index >= redDoorHorizontalSprites.Count) {
			spriteRenderer.sprite = null;

			return;
		}

		// Set the sprite of the block to a certain frame
		switch (DoorType) {
			case DoorType.Red:
				spriteRenderer.sprite = (DoorOrientationType == DoorOrientationType.Vertical) ? redDoorVerticalSprites[index] : redDoorHorizontalSprites[index];
				break;
			case DoorType.Orange:
				spriteRenderer.sprite = (DoorOrientationType == DoorOrientationType.Vertical) ? orangeDoorVerticalSprites[index] : orangeDoorHorizontalSprites[index];
				break;
			case DoorType.Blue:
				spriteRenderer.sprite = (DoorOrientationType == DoorOrientationType.Vertical) ? blueDoorVerticalSprites[index] : blueDoorHorizontalSprites[index];
				break;
			case DoorType.White:
				spriteRenderer.sprite = (DoorOrientationType == DoorOrientationType.Vertical) ? whiteDoorVerticalSprites[index] : whiteDoorHorizontalSprites[index];
				break;
		}
	}

	private void RemoveButton (ButtonObject button) {
		// Remove this door from the button as well as the button from this door
		button.Doors.Remove(this);
		buttons.Remove(button);
	}
}
