using UnityEngine;
using System.Collections;

public class PointerCell : MonoBehaviour {
	void OnMouseEnter () {
		VisualPointer.Pointers [InputController.PointerType.Cursor].GoToGameObject (transform.position, MazePiece.Empty);
	}

}
