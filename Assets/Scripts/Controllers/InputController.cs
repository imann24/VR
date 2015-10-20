using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public static InputController Instance;
	
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

	public void SetVisualPointer (VisualPointer pointer) {
		this.pointer = pointer.transform;
	}

	public bool HasActiveCharacter () {
		return currentSelectedCharacter != null;
	}
}
