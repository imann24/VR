using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]

public class UIPointer : MonoBehaviour {
	public Vector2 MouseOffset;
	private RectTransform rTransform;

	// Use this for initialization
	void Start () {
		setReferences();
	}
	
	// Positioning code from: http://answers.unity3d.com/questions/12322/drag-gameobject-with-mouse.html
	void Update () {

		rTransform.position = (Vector2) Input.mousePosition + MouseOffset;
	}

	void setReferences () {
		rTransform = GetComponent<RectTransform>();
	}
}
