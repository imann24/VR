using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public static InputController Instance;

	public enum PointerType {Cursor, Mover};
	
	private bool inputEnabled = true;

	private CharacterMover currentSelectedCharacter = null;

	private Transform pointer;

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {
		makeCursorInvisible();
	}

	void Update () {
		if (currentSelectedCharacter != null) {
			currentSelectedCharacter.MoveCharacter (
				Util.MatchPosition (
					pointer.transform,
			       	currentSelectedCharacter.transform,
					true, 
					false,
					true));
		}

		if (Input.GetMouseButtonUp(0)) {
			inputEnabled = true;
		}
	}

	public void SetSelectCharacterMazePosition (Position position) {
		if (currentSelectedCharacter != null) {
			currentSelectedCharacter.SetMazePosition(position);
		}
	}

	public void ToggleInputEnabled (bool inputEnabled) {
		this.inputEnabled = inputEnabled;
	}

	public bool InputEnabled () {
		return inputEnabled;
	}

	public void Restart () {
		MazeController.Instance.SpawnMaze();
		CanvasController.Instance.ToggleCanvas(false);
	}

	public void SetSelecterCharacter (CharacterMover selectedCharacter) {
		currentSelectedCharacter = selectedCharacter;
	}

	public void SetMovementPointer (Transform pointer) {
		this.pointer = pointer;
	}

	public void MovePointers (MazePiece mazePiece, Vector3 worldPosition, Position mazePosition) {
		VisualPointer.Pointers [PointerType.Cursor].GoToGameObject (worldPosition, mazePiece);
		if (inputEnabled) {
			if (mazePiece != MazePiece.Wall) {
				VisualPointer.Pointers[PointerType.Mover].GoToGameObject(mazePosition, mazePiece);
			} else {
				if (currentSelectedCharacter != null) {
					inputEnabled = false;
				}
				VisualPointer.Pointers[PointerType.Mover].StopMovement();
			}
		}
	}

	public bool HasActiveCharacter () {
		return currentSelectedCharacter != null;
	}

	private void makeCursorInvisible () {
		Cursor.visible = false;
	}
}
