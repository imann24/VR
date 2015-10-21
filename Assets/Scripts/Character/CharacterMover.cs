using UnityEngine;
using System.Collections;

public class CharacterMover : MonoBehaviour {

	// Dragging code from: http://answers.unity3d.com/questions/12322/drag-gameobject-with-mouse.html
	private Vector3 offset;
	private Vector3 screenPoint;
	
	bool debug = true;
	// Use this for initialization
	void Start () {
		Util.ToggleHalo(gameObject, false);
	}

	void OnMouseOver () {
		InputController.Instance.ToggleInputEnabled(true);
		Util.ToggleHalo(gameObject, true);
	}

	void OnMouseExit () {
		if (!Input.GetMouseButton(0)) {
			Util.ToggleHalo(gameObject, false);
		}
	}

	void OnMouseDown () {
		InputController.Instance.SetSelecterCharacter (this);
		if (debug)
			return;

		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		VisualPointer.Instance.ToggleHalo (false);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag () {
	
		if (debug)
			return;
		if (!InputController.Instance.InputEnabled()) {
			return;
		}

		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint + offset);
		curPosition.y = transform.position.y;

		transform.position = curPosition;
	}

	void OnMouseUp () {
		InputController.Instance.SetSelecterCharacter (null);
		if (debug)
			return;
		VisualPointer.Instance.ToggleHalo (true);
		Util.ToggleHalo(gameObject, false);
	}

	void OnTriggerEnter (Collider collider) {
		MazePieceController controller;
		if ((controller = collider.GetComponent<MazePieceController>()) != null &&
		    controller.Type == MazePiece.Wall) {
			InputController.Instance.SetSelecterCharacter(null);
		}
	}

	public void MoveCharacter (Vector3 newPosition) {
		transform.position = newPosition;
	}
}
