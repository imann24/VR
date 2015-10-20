using UnityEngine;
using System.Collections;

public class CharacterMover : MonoBehaviour {

	// Dragging code from: http://answers.unity3d.com/questions/12322/drag-gameobject-with-mouse.html
	private Vector3 offset;
	private Vector3 screenPoint;
	// Use this for initialization
	void Start () {
		Util.ToggleHalo(gameObject, false);
	}

	void OnMouseOver () {
		Util.ToggleHalo(gameObject, true);
	}

	void OnMouseExit () {
		if (!Input.GetMouseButton(0)) {
			Util.ToggleHalo(gameObject, false);
		}
	}

	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag () {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint + offset);
		curPosition.y = transform.position.y;
		transform.position = curPosition;
	}

	void OnMouseUp () {
		Util.ToggleHalo(gameObject, false);
	}
}
