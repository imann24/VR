using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MazeController : MonoBehaviour {
	public delegate void MazeCompleteAction (Maze maze);
	public static event MazeCompleteAction OnMazeComplete;

	public int MaximumDestroyDistance = 2;
	public int BorderSize = 25;

	public static MazeController Instance;

	public TextAsset [] MazeTemplates;

	public GameObject [] PersistentMazeComponents;

	private List<Maze> allMazes = new List<Maze>();
	private Maze currentMaze;
	private MazePieceController[][] currentMazePieceControllers;

	// To spawn the maze
	public Transform MazeParent;
	public GameObject MazePiecePrefab;
	public GameObject DestroyableWallPrefab;
	public GameObject CharacterPiecePrefab;
	public GameObject FinishPlacePrefab;
	public GameObject EmptyPiecePrefab;
	public GameObject LightPieceBase;
	public GameObject DarkPieceBase;
	public GameObject TorchPrefab;
	public GameObject PointerCellPrefab;

	private const string DONT_DETROY_TAG = "DontDestroy";

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {
		preservePersistentMazeComponents();
		generateMazes();
		SpawnMaze();
		setVisualMoverPosition();
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	public void SpawnMaze (bool shouldDestroyCurrentMaze = true) {
		MazePiece[][] mazePieces = currentMaze.GetPieces();
		currentMazePieceControllers = Util.InitializeMatrixAsJaggedArray(new MazePieceController[currentMaze.Width()][],
		                                                                 currentMaze.Width(),
		                                                                 currentMaze.Height());
		bool isCharacter = false;

		if (shouldDestroyCurrentMaze) destroyCurrentMaze();

		generateBorder (BorderSize);

		for (int x = 0; x < mazePieces.Length; x++) {
			for (int y = 0; y < mazePieces[x].Length; y++) {

				MazePiece currentPieceType = mazePieces[x][y];
				GameObject currentPiece = null;
				spawnTile(x, y);
				if (currentPieceType == MazePiece.Wall) {
					currentPiece = (GameObject) Instantiate(MazePiecePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				} else if (currentPieceType == MazePiece.Start) {
					currentPiece = (GameObject) Instantiate(CharacterPiecePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
					isCharacter = true;
				} else if (currentPieceType == MazePiece.Finish) {
					currentPiece = (GameObject) Instantiate(FinishPlacePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				} else if (currentPieceType == MazePiece.Empty) {
					currentPiece = (GameObject) Instantiate(EmptyPiecePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				} else if (currentPieceType == MazePiece.DestroyableWall) {
					currentPiece = (GameObject) Instantiate(DestroyableWallPrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				}

				if (currentPiece != null) {
					currentPiece.transform.parent = MazeParent;
					currentMazePieceControllers[x][y] = currentPiece.GetComponent<MazePieceController>();
					currentMazePieceControllers[x][y].SetPosition(x, y, true);

					if (isCharacter) {
						VisualPointer.ResetPointerPositions(currentPiece.transform.position);
						isCharacter = false;
					}
				}
			
			}
		}

		spawnTorches(currentMaze);
		
	}

	private bool isCenterPosition (int x, int y, int w, int h) {
		return (x == w/2 && y == h/2);
	}

	private void spawnTile (int x, int y, float baseTileYOffset = -0.5f, float anchorOffset = 0.2f) {
		GameObject currentTile = (GameObject) Instantiate(GetBasePieceTile(x, y), 
		                                                  MazePositioner.PositionFromIndex(x, y, baseTileYOffset, anchorOffset), 
		                                                  Quaternion.identity);
		
		currentTile.transform.parent = MazeParent;
	}

	private Position [] getTorchPositions (Maze maze, int torchSpacing = 3) {
		int numTorches = (int) Mathf.Pow(Mathf.Min (maze.Height(), maze.Width())/torchSpacing - 1, 2.0f);
		Position [] torchPositions = new Position[numTorches];
		int noWallFoundOffset = 0;
		for (int x = 0; x < Mathf.Sqrt(numTorches); x++) {
			for (int y = 0; y < Mathf.Sqrt(numTorches); y++) {
				Position currentTorchPosition = getRandomWallPositionInRange(maze,
			    	                                                         new Position(torchSpacing * x, torchSpacing * y),
			        	                                                     new Position(torchSpacing * (x + 1), torchSpacing * (y + 1)));
				if (currentTorchPosition == null) {
					noWallFoundOffset++;
				} else {
					if (currentTorchPosition == null) {
						Debug.Log(x + " " + y + " " + currentTorchPosition);
					}
					torchPositions[x * (int) Mathf.Sqrt(numTorches) + y - noWallFoundOffset] = currentTorchPosition;
				}
			}
		}

		return Util.RemoveNullElements(torchPositions);
	}

	private Position getRandomWallPositionInRange (Maze maze, Position minCorner, Position maxCorner) {
		int area = Position.Area(minCorner, maxCorner);
		int loopCount = 0;

		Position randomPosition = null;

		while (loopCount++ < area &&
		       (randomPosition == null ||
		 maze.MazePieceFromPosition(randomPosition) != MazePiece.Wall)) {
			randomPosition = Position.RandomPositionInRange(minCorner, maxCorner);
		}

		return maze.MazePieceFromPosition(randomPosition) == MazePiece.Wall?randomPosition:null;
	}

	private void spawnTorches (Maze maze, float torchHeight = 2.0f) {
		Position[] torchPositions = getTorchPositions(maze);

		for (int i = 0; i < torchPositions.Length; i++) {
			GameObject torch = (GameObject) Instantiate(TorchPrefab,
			            MazePositioner.PositionFromIndex(torchPositions[i], torchHeight),
			            Quaternion.identity);
			torch.transform.parent = MazeParent;
		}
	}

	public Maze GetCurrentMaze () {
		return currentMaze;
	}
	
	public void LoadMaze (int mazeIndex) {
		GameController.Instance.OpenInstructionBook();
		GameController.Instance.ToggleOverlayInstructions(false);
		setCurrentMaze (mazeIndex);
		SpawnMaze ();
	}

	public MazePieceController MazePieceControllerFromPosition (Position position) {
		return currentMazePieceControllers[position.GetX()][position.GetY()];
	}

	public MazePieceController[][] GetMazePieceControllers () {
		return currentMazePieceControllers;
	}

	public void WallDestroyed (Position position) {
		currentMaze.ModifyPiece(position, MazePiece.Empty);
	}

	private void destroyCurrentMaze () {
		for (int i = 0; i < MazeParent.childCount; i++) {
			GameObject mazeComponent = MazeParent.GetChild(i).gameObject;
			if (mazeComponent.tag != DONT_DETROY_TAG) {
				Destroy(mazeComponent);
			}
		}

		MazePieceController.WorldToMazePositions.Clear();
	}

	private void generateMazes () {
		for (int i = 0; i < MazeTemplates.Length; i++) {
			allMazes.Add(createMaze(MazeTemplates[i]));
			allMazes[i].SetName(MazeTemplates[i].name);
		}
		setHardestMaze();
		setCurrentMaze(0);
	}

	private void setCurrentMaze (int mazeIndex) {
		if (allMazes == null || mazeIndex >= allMazes.Count) {
			Debug.LogError("Index is out of range");
		} else {
			currentMaze = new Maze(allMazes[mazeIndex]);
		}
	}

	private void addMaze (TextAsset mazeTemplate) {
		allMazes.Add (createMaze(mazeTemplate));
	}

	private Maze createMaze (TextAsset mazeTemplate) {
		return mazeFrom2DStringArray(
			CSVReader.ParseCSV(mazeTemplate));
	}

	private Maze mazeFrom2DStringArray (string[][] mazeAsStringArray) {
		return MazeParser.ParseMaze(mazeAsStringArray); 
	}

	private void preservePersistentMazeComponents () {
		for (int i = 0; i < PersistentMazeComponents.Length; i++) {
			DontDestroyOnLoad(PersistentMazeComponents[i]);
		}
	}

	private void setVisualMoverPosition () {
		int x = currentMaze.Width()/2;
		int y = currentMaze.Height()/2;
		VisualPointer.SetStartingPositionOfMover(new Position(x, y));
		VisualPointer.Pointers[InputController.PointerType.Mover].SetPosition(
			currentMazePieceControllers[x][y].GetWorldPosition());
	}

	private GameObject GetBasePieceTile (int x, int y) {
		if ((x%2 == 0 && y%2 == 0) ||
		    (x%2 != 0 && y%2 != 0)) {
			return DarkPieceBase;
		} else {
			return LightPieceBase;
		}
	}

	private void generateBorder (int borderSize) {
		for (int x = -borderSize; x < currentMaze.Width () + borderSize; x++) {
			for (int y = -borderSize; y < currentMaze.Height() + borderSize; y++) {
				if (currentMaze.WithinMazeOuterBounds(new Position(x, y))) {
					continue;
				}
				
				GameObject pointerCell = (GameObject) Instantiate (PointerCellPrefab, 
				                                                   MazePositioner.PositionFromIndex(x, y),
				                                                   Quaternion.identity);
				pointerCell.transform.parent = MazeParent;
				
				
			}
		}
	}

	private void callMazeCompleteEvent (Location mazeLocation = Location.Finish) {
		if (OnMazeComplete != null && mazeLocation == Location.Finish) {
			OnMazeComplete(currentMaze);
		}
	}

	private void subscribeEvents () {
		MazePieceController.OnEnterLocation += callMazeCompleteEvent;
	}

	private void unsubscribeEvents () {
		MazePieceController.OnEnterLocation -= callMazeCompleteEvent;
	}

	private void setHardestMaze () {
		GameController.Instance.SetHardestMaze(allMazes[allMazes.Count - 1]);
	}
}
