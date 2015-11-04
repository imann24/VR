using System;
using UnityEngine;
using System.Collections;

public class Util {

	public static void SingletonImplementation<T> (ref T staticInstance, T instance, GameObject associatedObject) {
		if (staticInstance == null) {
			UnityEngine.Object.DontDestroyOnLoad(associatedObject);
			staticInstance = instance;
		} else {
			UnityEngine.Object.Destroy(associatedObject);
		}
	}

	public static string RemoveSpaces (string targetString) {
		if (string.IsNullOrEmpty(targetString)) {
			return targetString;
		}

		targetString =  targetString.Replace('\r', '\0');
		return targetString[targetString.Length-1] == '\0'?targetString.Substring(0, targetString.Length-1):targetString;
	}

	public static void ToggleHalo (GameObject targetObject, bool active) {
		(targetObject.GetComponent("Halo") as Behaviour).enabled = active;
	}

	public static Vector3 WorldPositionFromMouse () {
		Vector3 currentMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		return Camera.main.ScreenToWorldPoint (currentMousePosition);
	}

	public static Vector3 MatchPosition (Transform leader, Transform follower, bool x, bool y, bool z) {
		return new Vector3 (
			x ? leader.position.x : follower.position.x,
			y ? leader.position.y : follower.position.y,
			z ? leader.position.z : follower.position.y);
	}


	public static Vector3 RoundPositionToNearestHalves (Vector3 position) {
		position.x = RoundToNearestFifth(position.x);
		position.y = RoundToNearestFifth(position.y);
		position.z = RoundToNearestFifth(position.z);

		return position;

	}

	//Code from http://stackoverflow.com/questions/1329426/how-do-i-round-to-the-nearest-0-5
	public static float RoundToNearestFifth (float value) {
		return (float) Math.Round(value * 2f, MidpointRounding.AwayFromZero)/2f;
	}

	// Generic method returns true or false if the array contains the object
	public static bool ArrayContains<T> (T [] arrayToSearch, T objectToFind) where T : System.IComparable<T> {
		return System.Array.Exists(arrayToSearch, s => {if( s.CompareTo(objectToFind) == 0) 
			return true;
			else 
				return false;
		});
	}

	public static T [][] InitializeMatrixAsJaggedArray<T> (T[][] matrix, int w, int h) {
		matrix = new T[w][];

		for (int x = 0; x < w; x++) { 
			matrix[x] = new T[h];
		}

		return matrix;
	}

	public static bool WithinRange (int value, int upperBound, int lowerBound =0, bool includeUpperBound = false) {
		bool withinUpperBound = includeUpperBound?value<=upperBound:value<upperBound;
		return withinUpperBound && value >= lowerBound;
	}

	public static T [] RemoveNullElements <T> (T[] array) {
		int newLength = array.Length;
		int index = 0;
		T[] newArray;
		bool [] nullElements = new bool[array.Length];

		for (int i = 0; i < array.Length; i++) {
			nullElements[i] = array[i] == null;
			newLength = nullElements[i]?newLength-1:newLength;
		}

		newArray = new T[newLength];

		for (int i = 0; i < array.Length; i++) {
			if (nullElements[i]) {
				continue;
			} else {
				newArray[index++] = array[i];
			}
		}

		return newArray;
	}

	public static Color ChangeColorOpacty (Color color, float opacity) {
		return new Color(color.r, color.g, color.b, opacity);
	}
}
