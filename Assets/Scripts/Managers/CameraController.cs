using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public const float CAMERA_SMOOTH_SPEED = 0.5f;
	public const float MAX_CAMERA_OFFSET = 0.9f;

	[Header(" --- Camera Controller Class --- ")]
	[SerializeField] private Transform backgroundImage;
	[Space]
	[SerializeField] public Transform Target;

	private Vector3 cameraVelocity;
	private Vector3 backgroundVelocity;

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
			MoveCameraWithTarget( );
		} else {
			MoveCameraWithMouse( );
		}
	}

	/*
	 * Move the camera to follow the target
	 */
	private void MoveCameraWithTarget ( ) {
		// The vector to move the camera by
		Vector3 moveCamera = Utils.VectSetZ(Target.position, transform.position.z);
		Vector3 moveBackground = Utils.VectSetZ(moveCamera / 2, backgroundImage.position.z);

		// Smoothly transition from the camera's current position to the position of the target
		transform.position = Vector3.SmoothDamp(transform.position, moveCamera, ref cameraVelocity, CAMERA_SMOOTH_SPEED);
		backgroundImage.position = Vector3.SmoothDamp(backgroundImage.position, moveBackground, ref backgroundVelocity, CAMERA_SMOOTH_SPEED / 2);
	}

	private void MoveCameraWithMouse ( ) {
		Vector3 moveCamera = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		moveCamera.Normalize( );
		moveCamera = Utils.VectSetZ(moveCamera * MAX_CAMERA_OFFSET, transform.position.z);
		Vector3 moveBackground = Utils.VectSetZ(moveCamera / 2, backgroundImage.position.z);

		transform.position = Vector3.SmoothDamp(transform.position, moveCamera, ref cameraVelocity, CAMERA_SMOOTH_SPEED);
		backgroundImage.position = Vector3.SmoothDamp(backgroundImage.position, moveBackground, ref backgroundVelocity, CAMERA_SMOOTH_SPEED / 2);
	}
}
