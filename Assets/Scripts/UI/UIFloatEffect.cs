using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloatEffect : MonoBehaviour {
	[Header(" --- UI Float Effect Class --- ")]
	[SerializeField] private float moveTime = 5;
	[SerializeField] private float positionChange = 0.1f;
	[SerializeField] private float rotationChange = 1.5f;

	private Vector3 startingPosition;
	private Vector3 startingRotationEulers;
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Vector3 toPosition;
	private Quaternion toRotation;

	private float lastTime;

	private void Start ( ) {
		// Set all default and starting values
		startingPosition = lastPosition = transform.position;
		startingRotationEulers = transform.eulerAngles;
		toPosition = startingPosition;
		toRotation = lastRotation = Quaternion.identity;

		// Make sure the UI element starts to move as soon as the game starts
		lastTime = -moveTime;
	}

	private void Update ( ) {
		// If the time specified to move has elapsed, generate a new position and rotation to move to
		if (Time.unscaledTime - lastTime >= moveTime) {
			SetNewOrientation( );

			lastTime = Time.unscaledTime;
		}

		// Smoothly transition between the last location and the new location over the time specified
		float t = (Time.unscaledTime - lastTime) / moveTime;
		transform.position = Vector3.Slerp(lastPosition, toPosition, t);
		transform.rotation = Quaternion.Slerp(lastRotation, toRotation, t);
	}

	/*
	 * Get a new orientation for the UI element to go to
	 */
	private void SetNewOrientation ( ) {
		lastPosition = toPosition;
		lastRotation = toRotation;

		toPosition = startingPosition + Utils.GetRandVect3(-positionChange, positionChange, -positionChange, positionChange, 0, 0);
		toRotation = Quaternion.Euler(startingRotationEulers + Utils.GetRandVect3(0, 0, 0, 0, -rotationChange, rotationChange));
	}
}
