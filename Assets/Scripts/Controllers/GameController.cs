using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController Instance;

	public GameState CurrentState = GameState.Game;
	// Use this for initialization
	void Start () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
		AudioController.Instance.SetMusic(CurrentState);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			MazeController.Instance.SpawnMaze();
		} else if (Input.GetKeyDown(KeyCode.Alpha1)) {
			MazeController.Instance.LoadMaze(0);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			MazeController.Instance.LoadMaze(1);
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			MazeController.Instance.LoadMaze(2);
		}
	}
}
