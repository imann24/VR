using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider))]

public class MazePieceController : MonoBehaviour {
	public static Dictionary<Vector3, Position> WorldToMazePositions = new Dictionary<Vector3, Position>();

	public delegate void EnterLocationAction (Location currentLocation);
	public static event EnterLocationAction OnEnterLocation;

	private Position mazePosition;

	public MazePiece Type;
	
	void Start () {
	}


	void OnDestroy () {
		WorldToMazePositions.Remove(transform.position);
	}

	void OnMouseEnter () {
		InputController.Instance.MovePointers(Type, transform.position, mazePosition);
	}

	void OnTriggerEnter (Collider collider) {
		if (Type == MazePiece.Finish && OnEnterLocation != null) {
			OnEnterLocation(Location.Finish);
		}
	}
	
	public void SetPosition (int x, int y) {
		mazePosition = new Position(x, y);
		WorldToMazePositions.Add(transform.position, mazePosition);
	}

	public Position GetMazePosition () {
		return mazePosition;
	}

	public Vector3 GetWorldPosition () {
		return transform.position;
	}

	public static Vector3[] GetWorldPath (MazePieceController[] mazePieceControllers) {
		Vector3[] worldPath = new Vector3[mazePieceControllers.Length];

		for (int i = 0; i < worldPath.Length; i++) {
			worldPath[i] = mazePieceControllers[i].GetWorldPosition();
		}

		return worldPath;
	}
}
