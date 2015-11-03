using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider))]

public class MazePieceController : MonoBehaviour {
	public static Dictionary<Vector3, Position> WorldToMazePositions = new Dictionary<Vector3, Position>();

	public delegate void DestroyWallAction();
	public static event DestroyWallAction OnDestroyWall;

	public delegate void EnterLocationAction (Location currentLocation);
	public static event EnterLocationAction OnEnterLocation;

	private Position mazePosition;
	private bool isCharacter {
		get {
			return Type == MazePiece.Character;
		}
	}
	public MazePiece Type;
	
	void Start () {
	}

	void OnMouseEnter () {
		if (!isCharacter) {
			InputController.Instance.MovePointers(Type, transform.position, mazePosition);
		}

		if (Type == MazePiece.DestroyableWall) {
			if (characterWithinDestroyRange()) {
				Util.ToggleHalo(gameObject, true);
			}

		}
	}

	void OnMouseDown () {
		if (Type == MazePiece.DestroyableWall && characterWithinDestroyRange()) {
			destroyWall();
		}
	}

	void OnMouseExit () {
		if (Type == MazePiece.DestroyableWall) {
			Util.ToggleHalo(gameObject, false);
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (Type == MazePiece.Finish && OnEnterLocation != null) {
			OnEnterLocation(Location.Finish);
		}
	}
	
	public void SetPosition (int x, int y, bool spawingNewMaze = true) {
		mazePosition = new Position(x, y);

		if (WorldToMazePositions.ContainsKey(transform.position) && (!isCharacter || spawingNewMaze)) {
			WorldToMazePositions.Remove(transform.position);
		}

		if (!WorldToMazePositions.ContainsKey(transform.position)) {
			WorldToMazePositions.Add(transform.position, mazePosition);
		}
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

	private void destroyWall () {
		//TODO: Change from destroying the object outright to playing the destroy animation
		Destroy(transform.GetChild(0).gameObject);
		Util.ToggleHalo(gameObject, false);

		callDetroyWallEvent();

		Type = MazePiece.Empty;
		MazeController.Instance.GetCurrentMaze().ModifyPiece(mazePosition,
		                                                     MazePiece.Empty);

	}

	private void callDetroyWallEvent () {
		if (OnDestroyWall != null) {
			OnDestroyWall();
		}
	}

	private bool characterWithinDestroyRange () {
		return mazePosition.Distance(GameController.Instance.MainCharacter.MazePosition) <= 
			MazeController.Instance.MaximumDestroyDistance;
	}
}
