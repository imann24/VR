using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public static InputController Instance;

	public enum PointerType {Cursor, Mover};

	private bool inputEnabled;

	private CharacterMover currentSelectedCharacter = null;

	private Transform pointer;

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {

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

	public void MovePointers (MazePiece mazePiece, Vector3 position) {
		VisualPointer.Pointers [PointerType.Cursor].GoToGameObject (position, mazePiece);
		if (mazePiece != MazePiece.Wall) {
			VisualPointer.Pointers[PointerType.Mover].GoToGameObject(position, mazePiece);
		}
	}

	public bool HasActiveCharacter () {
		return currentSelectedCharacter != null;
	}
}
