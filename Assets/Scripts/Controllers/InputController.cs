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

		checkForMazeLoad();

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

			if (GameController.Instance.InstructionBookletClosed()) {
				GameController.Instance.OpenInstructionBook();
			}
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

	public CharacterMover GetSelectedCharacter () {
		return currentSelectedCharacter;
	}

	public void SetMovementPointer (Transform pointer) {
		this.pointer = pointer;
	}

	public void MovePointers (MazePiece mazePiece, Vector3 worldPosition, Position mazePosition) {
		VisualPointer.Pointers [PointerType.Cursor].GoToGameObject (worldPosition, mazePiece);
		if (inputEnabled) {
			if (mazePiece != MazePiece.Wall) {
				VisualPointer.Pointers[PointerType.Mover].GoToGameObject(mazePosition, mazePiece);
			} 
		}
	}

	public bool HasActiveCharacter () {
		return currentSelectedCharacter != null;
	}

	private void makeCursorInvisible () {
		Cursor.visible = false;
	}

	// Update is called once per frame
	private void checkForMazeLoad () {
		if (Input.GetKeyDown (KeyCode.R)) {
			MazeController.Instance.SpawnMaze();
		} else if (Input.GetKeyDown(KeyCode.Alpha1)) {
			MazeController.Instance.LoadMaze(0);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			MazeController.Instance.LoadMaze(1);
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			MazeController.Instance.LoadMaze(2);
		} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			MazeController.Instance.LoadMaze(3);
		} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			MazeController.Instance.LoadMaze(4);
		} else if (Input.GetKeyDown(KeyCode.Alpha6)) {
			MazeController.Instance.LoadMaze(5);
		} else if (Input.GetKeyDown(KeyCode.Alpha7)) {
			MazeController.Instance.LoadMaze(6);
		} else if (Input.GetKeyDown(KeyCode.Alpha8)) {
			MazeController.Instance.LoadMaze(7);
		} else if (Input.GetKeyDown(KeyCode.Alpha9)) {
			MazeController.Instance.LoadMaze(8);
		} else if (Input.GetKeyDown(KeyCode.Alpha0)) {
			MazeController.Instance.LoadMaze(9);
		}
	}

}
