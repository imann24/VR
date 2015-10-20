using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public static InputController Instance;
	
	private bool inputEnabled;

	// Use this for initialization
	void Start () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
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
}
