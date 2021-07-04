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

	[HideInInspector] public bool IsFullyPressed; // If the button should stay on
	private bool lastPressedState; // The last state of the button in the previous frame
	private bool _isPressed;
	public bool IsPressed {
		get {
			lastPressedState = _isPressed;

			if (levelManager == null) {
				return false;
			}

			// Check to see if the button is either fully pressed or if there is a block on it
			_isPressed = levelManager.CheckForBlockAtPosition(transform.position) || IsFullyPressed;

			// Update the sprite
			UpdateSprite(_isPressed);

			if (lastPressedState != _isPressed) {
				gameManager.PlaySoundEffect(SoundEffectType.BUTTON);
			}

			return _isPressed;
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
		if (ButtonType == ButtonType.White && IsPressed && !IsFullyPressed) {
			IsFullyPressed = true;
			pauseManager.IsLevelComplete = true;

			gameManager.PlaySoundEffect(SoundEffectType.WIN);
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
