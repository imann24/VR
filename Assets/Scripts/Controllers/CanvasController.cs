using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]

public class CanvasController : MonoBehaviour {
	public static CanvasController Instance;

	CanvasGroup canvasGroup;

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {

		InitializeReferences();
		SubscribeReferences();
	}
	
	void OnDestroy () {
		UnsubscribeReferences();
	}

	private void SubscribeReferences () {
		MazePieceController.OnEnterLocation += HandleEnterLocation;
	}

	private void UnsubscribeReferences () {
		MazePieceController.OnEnterLocation -= HandleEnterLocation;
	}

	private void InitializeReferences () { 
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void HandleEnterLocation (Location currentLocation) {
		bool victory = (currentLocation == Location.Finish);

		if (victory) {
			ToggleCanvas(true);
		}
	}

	public void ToggleCanvas (bool enabled) {
		canvasGroup.interactable = enabled;
		canvasGroup.alpha = enabled?1.0f:0.0f;
	}
}
