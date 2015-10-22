using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualPointer : MonoBehaviour {
	public InputController.PointerType Type;
	public static Dictionary <InputController.PointerType, VisualPointer> Pointers = new Dictionary<InputController.PointerType, VisualPointer>();
	private IEnumerator currentMoveCoroutine;
	private float yOffset = 0.5f;
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

	public void GoToGameObject (Vector3 gameObjectPosition, MazePiece type) {
		//if (type == MazePiece.Wall && InputController.Instance.HasActiveCharacter ()) {
		//	return;
		//}

		if (currentMoveCoroutine != null)
			StopCoroutine (currentMoveCoroutine);

		gameObjectPosition.y += yOffset;

		currentMoveCoroutine = LerpToPosition (gameObjectPosition);
		StartCoroutine (currentMoveCoroutine);
	}

	public void ToggleHalo (bool active) {
		Util.ToggleHalo (gameObject, active);
	}

	IEnumerator LerpToPosition (Vector3 targetPosition) {
		Vector3 initialPosition = transform.position;
		float steps = 15;
		for (float i = 0; i < steps; i++) {
			transform.position = Vector3.Lerp(initialPosition, targetPosition, i/steps);
			yield return new WaitForEndOfFrame();
		}
	}
}
