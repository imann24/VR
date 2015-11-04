using UnityEngine;
using System.Linq;
using System.Collections;

public class GameController : MonoBehaviour {
	public delegate void InstructionPageTurnAction();
	public static event InstructionPageTurnAction OnInstructionPageTurn;

	public delegate void MazeCompleteAction(bool hardestMaze);
	public static event MazeCompleteAction OnMazeComplete;

	public static GameController Instance;
	public CharacterMover MainCharacter {get; private set;}

	public GameObject OverlaidInstructions;
	public GameObject OverlaidInstructionsCoverPage;
	public GameObject TabletopInstructions;

	public GameState CurrentState = GameState.Start;

	public Maze HardestMaze {private set; get;}

	// Use this for initialization
	void Start () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
		AudioController.Instance.SetMusic(CurrentState);
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	public void SetMainCharacter (CharacterMover mainCharacter) {
		MainCharacter = mainCharacter;
	}

	public bool HasMainCharacter () {
		return MainCharacter != null;
	}

	public void OpenInstructionBook () {
		callInstructionPageTurnEvent();
		OverlaidInstructionsCoverPage.SetActive(false);
		OverlaidInstructions.SetActive(true);
	}

	public bool InstructionBookletClosed () {
		return OverlaidInstructionsCoverPage.activeSelf;
	}

	public bool InstructionBookletOpen () {
		return OverlaidInstructions.activeSelf;
	}

	public void ToggleOverlayInstructions (bool active) {
		callInstructionPageTurnEvent();
		OverlaidInstructions.SetActive(active);
		TabletopInstructions.SetActive(!active);
		CurrentState = active ? GameState.Start : GameState.Game;
		AudioController.Instance.SetMusic(CurrentState);
	}

	public void SetHardestMaze (Maze maze) {
		HardestMaze = maze;
	}

	private bool isHardestMaze (Maze maze) {
		return maze.Equals(HardestMaze);
	}

	private void callInstructionPageTurnEvent () {
		if (OnInstructionPageTurn != null) {
			OnInstructionPageTurn();
		}
	}

	private void subscribeEvents () {
		MazePieceController.OnEnterLocation += (Location currentLocation) =>  
			ToggleOverlayInstructions(currentLocation == Location.Finish);
		MazeController.OnMazeComplete += handleMazeComplete;
	}

	private void unsubscribeEvents () {
		MazePieceController.OnEnterLocation -= (Location currentLocation) =>  
			ToggleOverlayInstructions(currentLocation == Location.Finish);
		MazeController.OnMazeComplete -= handleMazeComplete;
	}

	private void handleMazeComplete (Maze maze) {
		callOnMazeCompleteEvent(isHardestMaze(maze));
	}

	private void callOnMazeCompleteEvent (bool isHardestMaze) {
		if (OnMazeComplete != null) {
			OnMazeComplete(isHardestMaze);
		}
	}
}
