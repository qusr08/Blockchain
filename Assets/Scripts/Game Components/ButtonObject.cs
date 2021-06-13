using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType {
	Red, Orange, Blue, White
}

public class ButtonObject : GameComponent {
	[Header(" --- Button Object Class ---")]
	[SerializeField] public ButtonType ButtonType;
	[SerializeField] private List<Sprite> buttonOffSprites = new List<Sprite>( );
	[SerializeField] private List<Sprite> buttonOnSprites = new List<Sprite>( );
	[Space]
	[SerializeField] public List<DoorObject> Doors = new List<DoorObject>( );

	[HideInInspector] public bool IsFullyPressed;
	public bool IsPressed {
		get {
			// Unity is dumb and forced my hand to check this. I have no idea why
			if (gameManager == null) {
				return false;
			}

			// Check to see if the button is either fully pressed or if there is a block on it
			bool isPressed = gameManager.CheckForBlockAtPosition(transform.position) || IsFullyPressed;

			// Update the sprite
			UpdateSprite(isPressed);

			return isPressed;
		}
	}

	private new void OnValidate ( ) {
		base.OnValidate( );

		Doors.Clear( );
		if (ButtonType != ButtonType.White) {
			foreach (DoorObject door in FindObjectsOfType(typeof(DoorObject))) {
				if (door.name == $"{ButtonType} Door") {
					Doors.Add(door);
				}
			}
		}

		UpdateSprite(false);

		name = $"{ButtonType} Button";
	}

	private void Update ( ) {
		if (ButtonType == ButtonType.White && IsPressed) {
			pauseManager.IsLevelComplete = true;
		}
	}

	public void UpdateSprite (bool isPressed) {
		if (isPressed) {
			spriteRenderer.sprite = buttonOnSprites[(int) ButtonType];
		} else {
			spriteRenderer.sprite = buttonOffSprites[(int) ButtonType];
		}
	}

	public override void SetSpriteFrame (int index) {
		throw new System.NotImplementedException( );
	}
}
