using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController Instance;
	public CharacterMover MainCharacter {get; private set;}

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
		} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			MazeController.Instance.LoadMaze(3);
		} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			MazeController.Instance.LoadMaze(4);
		}
	}

	public void SetMainCharacter (CharacterMover mainCharacter) {
		MainCharacter = mainCharacter;
	}

	public bool HasMainCharacter () {
		return MainCharacter != null;
	}
}
