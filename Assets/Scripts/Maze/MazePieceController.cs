using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class MazePieceController : MonoBehaviour {
	public delegate void EnterLocationAction (Location currentLocation);
	public static event EnterLocationAction OnEnterLocation;

	public MazePiece Type;

	void OnMouseOver () {
		InputController.Instance.MovePointers(Type, transform.position);
	}

	void OnTriggerEnter (Collider collider) {
		if (Type == MazePiece.Finish && OnEnterLocation != null) {
			OnEnterLocation(Location.Finish);
		}
	}

}
