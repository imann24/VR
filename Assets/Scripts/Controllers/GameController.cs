using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			MazeController.Instance.SpawnMaze();
		} else if (Input.GetKeyDown(KeyCode.Alpha1)) {
			MazeController.Instance.LoadMaze(0);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			MazeController.Instance.LoadMaze(1);
		}
	}
}
