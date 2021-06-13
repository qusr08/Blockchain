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
	// [Space]
	// [SerializeField] private GameObject lineRendererPrefab;
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

	// private List<LineRenderer> lineRenderers = new List<LineRenderer>( );

	private new void OnValidate ( ) {
		base.OnValidate( );

		Doors.Clear( );
		// lineRenderers.Clear( );
		if (ButtonType != ButtonType.White) {
			foreach (DoorObject door in FindObjectsOfType(typeof(DoorObject))) {
				if (door.name == $"{ButtonType} Door") {
					Doors.Add(door);

					/*
					LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>( );

					Color color = Color.black;
					switch (ButtonType) {
						case ButtonType.Red:
							color = Color.red;
							break;
						case ButtonType.Orange:
							color = Color.yellow;
							break;
						case ButtonType.Blue:
							color = Color.blue;
							break;
					}

					lineRenderer.startColor = color;
					lineRenderer.endColor = color;
					lineRenderer.startWidth = LINE_WIDTH;
					lineRenderer.endWidth = LINE_WIDTH;

					lineRenderer.positionCount = 2;
					lineRenderer.useWorldSpace = true;
					lineRenderer.SetPosition(0, transform.position);
					lineRenderer.SetPosition(1, door.transform.position);

					lineRenderer.enabled = false;
					lineRenderers.Add(lineRenderer);
					*/
				}
			}
		}

		UpdateSprite(IsPressed);

		name = $"{ButtonType} Button";
	}

	/*
	private void OnMouseEnter ( ) {
		foreach (LineRenderer lineRenderer in lineRenderers) {
			lineRenderer.enabled = true;
		}
	}

	private void OnMouseExit ( ) {
		foreach (LineRenderer lineRenderer in lineRenderers) {
			lineRenderer.enabled = false;
		}
	}
	*/

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
