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

	void Update () {
		matchMousePosition();
	}

	void setReferences () {
		rTransform = GetComponent<RectTransform>();
	}

	void matchMousePosition () {
		rTransform.position = (Vector2) Input.mousePosition + MouseOffset;
	}
}
