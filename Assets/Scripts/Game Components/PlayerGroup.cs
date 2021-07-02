using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroup : BlockGroup {
	private new void Update ( ) {
		base.Update( );

		// Move the player with keyboard input
		MoveWithKeyboard( );
	}

	private void OnDestroy ( ) {
		pauseManager.ReloadScene( );
	}

	private void MoveWithKeyboard ( ) {
		if (Input.GetKey(KeyCode.W)) {
			Move(Vector2.up);
		} else if (Input.GetKey(KeyCode.A)) {
			Move(Vector2.left);
		} else if (Input.GetKey(KeyCode.S)) {
			Move(Vector2.down);
		} else if (Input.GetKey(KeyCode.D)) {
			Move(Vector2.right);
		}
	}
}
