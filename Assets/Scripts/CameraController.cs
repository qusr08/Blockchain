using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public const float CAMERA_SMOOTH_SPEED = 0.5f;

	[Header(" --- Camera Controller Class --- ")]
	[SerializeField] public Transform Target;

	private Vector3 velocity;

	private void OnValidate ( ) {
		if (GameObject.Find("Player Block") != null) {
			Target = GameObject.Find("Player Block").transform;
		}

		if (Target != null) {
			transform.position = Utils.VectSetZ(Target.position, transform.position.z);
		}
	}

	private void LateUpdate ( ) {
		// Move the camera as long as there is a target
		if (Target != null) {
			Move( );
		}
	}

	/*
	 * Move the camera to follow the target
	 */
	private void Move ( ) {
		// The vector to move the camera by
		Vector3 move = Utils.VectSetZ(Target.position, transform.position.z);

		// Smoothly transition from the camera's current position to the position of the target
		transform.position = Vector3.SmoothDamp(transform.position, move, ref velocity, CAMERA_SMOOTH_SPEED);
	}
}
