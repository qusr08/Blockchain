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
	[Space]
	[SerializeField] public AudioClip activeSound;
	[SerializeField] public AudioClip winSound;

	[HideInInspector] public bool IsFullyPressed;
	private bool lastPressedState;
	private bool _isPressed;
	public bool IsPressed {
		get {
			lastPressedState = _isPressed;

			// Unity is dumb and forced my hand to check this. I have no idea why
			if (gameManager == null) {
				return false;
			}

			// Check to see if the button is either fully pressed or if there is a block on it
			_isPressed = gameManager.CheckForBlockAtPosition(transform.position) || IsFullyPressed;

			// Update the sprite
			UpdateSprite(_isPressed);

			if (lastPressedState != _isPressed) {
				audioSource.PlayOneShot(activeSound);
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

			audioSource.PlayOneShot(winSound);
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
