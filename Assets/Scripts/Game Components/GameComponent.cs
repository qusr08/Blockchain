using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameComponent : MonoBehaviour {
	[Header(" --- Game Component Class ---")]
	[SerializeField] protected LevelManager levelManager;
	[SerializeField] protected PauseManager pauseManager;
	[SerializeField] protected GameManager gameManager;
	[Space]
	[SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected Animator animator;
	[SerializeField] protected Collider2D thisCollider2D;

	protected void OnValidate ( ) {
		if (spriteRenderer == null) {
			spriteRenderer = GetComponent<SpriteRenderer>( );
		}

		if (animator == null) {
			animator = GetComponent<Animator>( );
		}

		if (gameManager == null) {
			gameManager = FindObjectOfType<GameManager>( );
		}

		if (levelManager == null) {
			levelManager = FindObjectOfType<LevelManager>( );
		}

		if (pauseManager == null) {
			pauseManager = FindObjectOfType<PauseManager>( );
		}

		if (thisCollider2D == null) {
			thisCollider2D = GetComponent<Collider2D>( );
		}

		// Make sure this game component is on the grid when being placed in the scene
		// Just makes things easier
		float x = transform.position.x;
		float y = transform.position.y;
		if (Mathf.Floor(x) != x) {
			x = Mathf.Floor(x);
		}
		if (Mathf.Floor(y) != y) {
			y = Mathf.Floor(y);
		}
		transform.position = new Vector3(x, y, transform.position.z);
	}

	protected void Start ( ) {
		OnValidate( );
	}

	public abstract void SetSpriteFrame (int index);
}
