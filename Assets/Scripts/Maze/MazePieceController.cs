﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]

public class MazePieceController : MonoBehaviour {
	public delegate void EnterLocationAction (Location currentLocation);
	public static event EnterLocationAction OnEnterLocation;

	public MazePiece Type;

	void OnMouseOver () {
		if (Type == MazePiece.Wall) {
			InputController.Instance.ToggleInputEnabled(false);
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (Type == MazePiece.Finish && OnEnterLocation != null) {
			OnEnterLocation(Location.Finish);
		}
	}

}