using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualPointer : MonoBehaviour {
	public InputController.PointerType Type;
	public static Dictionary <InputController.PointerType, VisualPointer> Pointers = 
		new Dictionary<InputController.PointerType, VisualPointer>();
	private IEnumerator currentMoveCoroutine;
	private float yOffset = 0.5f;

	private Position currentPosition;

	void Awake () {
		if (Pointers.ContainsKey (Type)) {
			Destroy (gameObject);
		} else {
			Pointers.Add(Type, this);
		}
	}

	// Use this for initialization
	void Start () {
		if (Type == InputController.PointerType.Mover) {
			InputController.Instance.SetMovementPointer(transform);
		}
	}

	public void SetPosition (Vector3 worldPosition) {
		setMazePosition(worldPosition);
		transform.position = worldPosition;
	}

	public void GoToGameObject (Vector3 gameObjectPosition, MazePiece type) {
		stopMovement();

		gameObjectPosition.y += yOffset;

		currentMoveCoroutine = LerpToPosition (gameObjectPosition);
		StartCoroutine (currentMoveCoroutine);
	}

	private void stopMovement () {
		if (currentMoveCoroutine != null) {
			StopCoroutine (currentMoveCoroutine);
		}
	}

	public void GoToGameObject (Position mazePiecePosition, MazePiece type) {

		stopMovementCoroutine();

		StartCoroutine(
			currentMoveCoroutine = TraverseMazePositions(
			MazePositioner.WorldPathFromPosition(currentPosition, 
		                                     mazePiecePosition, 
		                                     MazeController.Instance.GetCurrentMaze())));
	}

	public void ToggleHalo (bool active) {
		Util.ToggleHalo (gameObject, active);
	}

	public void StopMovement () {
		stopMovementCoroutine();
	}

	private void stopMovementCoroutine () {
		if (currentMoveCoroutine != null) {
			StopCoroutine(currentMoveCoroutine);
		}
	}

	IEnumerator TraverseMazePositions (Vector3[] path, int currentIndex=0, float speed = 15f) {
		if (path != null && currentIndex < path.Length) {
			Vector3 initialPosition = transform.position;
			Vector3 targetPosition = path[currentIndex];
			float steps = speed;
			setMazePosition(targetPosition);

			for (float i = 0; i < steps; i++) {
				transform.position = Vector3.Lerp(initialPosition, targetPosition, i/steps);
				yield return new WaitForEndOfFrame();
			}



			currentMoveCoroutine = TraverseMazePositions(path, ++currentIndex);
			StartCoroutine(currentMoveCoroutine);

		}
	}

	IEnumerator LerpToPosition (Vector3 targetPosition) {
		Vector3 initialPosition = transform.position;
		float steps = 15;
		for (float i = 0; i < steps; i++) {
			transform.position = Vector3.Lerp(initialPosition, targetPosition, i/steps);
			yield return new WaitForEndOfFrame();
		}
	}

	private void resetPosition (Vector3 startPosition) {
		stopMovement();
		SetPosition(startPosition);
	}

	private void setMazePosition (Vector3 worldPosition) {
		try {
			currentPosition = MazePieceController.WorldToMazePositions[worldPosition];
		} catch { 
			//currentPosition = MazePositioner.GetClosestMazePosition(worldPosition);
		}

		if (Type == InputController.PointerType.Mover) {
			InputController.Instance.SetSelectCharacterMazePosition(currentPosition);
		}
	}

	public static void SetStartingPositionOfMover (Position position) {
		Pointers[InputController.PointerType.Mover].currentPosition = position;
	}
	
	public static void ResetPointerPositions (Vector3 startPosition) {
		Pointers[InputController.PointerType.Cursor].resetPosition(startPosition);
		Pointers[InputController.PointerType.Mover].resetPosition(startPosition);
	}
}
