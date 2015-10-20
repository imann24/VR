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

}
