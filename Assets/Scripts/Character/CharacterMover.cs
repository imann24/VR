using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]

public class CharacterMover : MonoBehaviour {

	// Dragging code from: http://answers.unity3d.com/questions/12322/drag-gameobject-with-mouse.html
	private Vector3 offset;
	private Vector3 screenPoint;
	private Rigidbody rigibody;
	bool suppressMicroMovement = false;

	private Quaternion forwardRotation = new Quaternion(0, 180, 0, 0);
	public Position MazePosition{get; private set;}

	bool debug = true;
	// Use this for initialization
	void Start () {
		Util.ToggleHalo(gameObject, false);
		rigibody = GetComponent<Rigidbody> ();
		setStartingMovementRotation();
		setStartingMazePosition();
	}

	void OnMouseEnter () {
		suppressMicroMovement = true;
		InputController.Instance.ToggleInputEnabled(true);
		VisualPointer.Pointers[InputController.PointerType.Mover].StopMovement();
		VisualPointer.Pointers[InputController.PointerType.Mover].SetPosition(transform.position);
		Util.ToggleHalo(gameObject, true);
	}

	void OnMouseExit () {
		if (!Input.GetMouseButton(0)) {
			Util.ToggleHalo(gameObject, false);
		}
		suppressMicroMovement = false;
	}

	void OnMouseDown () {
		InputController.Instance.SetSelecterCharacter (this);
		if (debug)
			return;

		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		VisualPointer.Pointers[InputController.PointerType.Mover].SetPosition(transform.position);
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
		if (suppressMicroMovement) {
			return;
		}

		rigibody.MovePosition (newPosition);
	}

	public void SetMazePosition (Position mazePosition) {
		MazePosition = mazePosition;
		GetComponent<MazePieceController>().SetPosition(
			mazePosition.GetX(), 
			mazePosition.GetY());
	}

	void setStartingMovementRotation () {
		transform.localRotation = forwardRotation;
	}

	void setStartingMazePosition () {
		MazePosition = GetComponent<MazePieceController>().GetMazePosition();
	}
}
