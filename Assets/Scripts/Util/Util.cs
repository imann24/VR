using UnityEngine;
using System.Collections;

public class Util {

	public static void SingletonImplementation<T> (ref T staticInstance, T instance, GameObject associatedObject) {
		if (staticInstance == null) {
			Object.DontDestroyOnLoad(associatedObject);
			staticInstance = instance;
		} else {
			Object.Destroy(associatedObject);
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

	// Generic method returns true or false if the array contains the object
	public static bool ArrayContains<T> (T [] arrayToSearch, T objectToFind) where T : System.IComparable<T> {
		return System.Array.Exists(arrayToSearch, s => {if( s.CompareTo(objectToFind) == 0) 
			return true;
			else 
				return false;
		});
	}

}
