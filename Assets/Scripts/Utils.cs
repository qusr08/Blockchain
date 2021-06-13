using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	/*
	 * Convert a Vector3 to a Vector3Int object
	 * 
	 * Vector3 vector3				: The vector3 to convert
	 */
	public static Vector3Int Vect3ToVect3Int (Vector3 vector3) {
		return new Vector3Int((int) vector3.x, (int) vector3.y, (int) vector3.z);
	}

	/*
	 * Set a Vector3's z value
	 * 
	 * Vector3 vector				: The vector to change
	 * float z						: The new z value
	 */
	public static Vector3 VectSetZ (Vector3 vector, float z) {
		vector.z = z;

		return vector;
	}
}
