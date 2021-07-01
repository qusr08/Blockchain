using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	public static System.Random Random = new System.Random( );

	public static Vector2 NorthWest = new Vector2(-1, 1);
	public static Vector2 North = new Vector2(0, 1);
	public static Vector2 NorthEast = new Vector2(1, 1);
	public static Vector2 West = new Vector2(-1, 0);
	public static Vector2 East = new Vector2(1, 0);
	public static Vector2 SouthWest = new Vector2(-1, -1);
	public static Vector2 South = new Vector2(0, -1);
	public static Vector2 SouthEast = new Vector2(1, -1);

	/*
	 * Convert a Vector3 to a Vector3Int object
	 * 
	 * Vector3 vector3				: The vector3 to convert
	 */
	public static Vector3Int Vect3ToVect3Int (Vector3 vector3) {
		return new Vector3Int((int) vector3.x, (int) vector3.y, (int) vector3.z);
	}

	/*
	 * Choose a random object from an array
	 */
	public static T Choose<T> (T[ ] array) {
		return array[Random.Next(0, array.Length)];
	}

	public static int BoolToInt (bool boolean) {
		return boolean ? 1 : 0;
	}

	/*
	 * Gets a random Vector3 with values between the given parameters
	 * 
	 * float xMin					: The minimum the x value can be
	 * float xMax					: The maximum the x value can be
	 * float yMin					: The minimum the y value can be
	 * float yMax					: The maximum the y value can be
	 * float zMin					: The minimum the z value can be
	 * float zMax					: The maximum the z value can be
	 */
	public static Vector3 GetRandVect3 (float xMin, float xMax, float yMin, float yMax, float zMin, float zMax) {
		float x = ((float) Random.NextDouble( ) * (xMax - xMin)) + xMin;
		float y = ((float) Random.NextDouble( ) * (yMax - yMin)) + yMin;
		float z = ((float) Random.NextDouble( ) * (zMax - zMin)) + zMin;

		return new Vector3(x, y, z);
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

	/*
	 * Get a random integer between two values
	 * 
	 * int min						: The minimum value
	 * int max						: The maximum value
	 * List<int> excludedNumbers	: Numbers that will never be given as output as a random number within the range
	 */
	public static int GetRandInt (int min, int max, List<int> excludedNumbers = null) {
		// A list of all valid numbers within the given range
		List<int> validNumbers = new List<int>( );

		// Get all of the valid numbers within the range
		for (int i = min; i < max; i++) {
			if (excludedNumbers == null || !excludedNumbers.Contains(i)) {
				validNumbers.Add(i);
			}
		}

		// Return the random valid number within the range
		return validNumbers[Random.Next(0, validNumbers.Count)];
	}

	public static bool GetRandBool ( ) {
		return (Random.Next(0, 2) == 1);
	}
}
