using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeController : MonoBehaviour {

	public static MazeController Instance;

	public TextAsset [] MazeTemplates;

	public GameObject [] PersistentMazeComponents;

	private List<Maze> allMazes = new List<Maze>();
	private Maze currentMaze;

	// To spawn the maze
	public Transform MazeParent;
	public GameObject MazePiecePrefab;
	public GameObject CharacterPiecePrefab;
	public GameObject FinishPlacePrefab;
	public GameObject EmptyPiecePrefab;

	private const string DONT_DETROY_TAG = "DontDestroy";

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {
		preservePersistentMazeComponents();
		generateMazes();
		SpawnMaze();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnMaze (bool shouldDestroyCurrentMaze = true) {
		MazePiece[][] mazePieces = currentMaze.GetPieces();

		if (shouldDestroyCurrentMaze) destroyCurrentMaze();

		for (int x = 0; x < mazePieces.Length; x++) {
			for (int y = 0; y < mazePieces[x].Length; y++) {
				MazePiece currentPieceType = mazePieces[x][y];
				GameObject currentPiece = null;

				if (currentPieceType == MazePiece.Wall) {
					currentPiece = (GameObject) Instantiate(MazePiecePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				} else if (currentPieceType == MazePiece.Start) {
					currentPiece = (GameObject) Instantiate(CharacterPiecePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				} else if (currentPieceType == MazePiece.Finish) {
					currentPiece = (GameObject) Instantiate(FinishPlacePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				} else if (currentPieceType == MazePiece.Empty) {
					currentPiece = (GameObject) Instantiate(EmptyPiecePrefab, MazePositioner.PositionFromIndex(x, y), Quaternion.identity);
				}

				if (currentPiece != null) currentPiece.transform.parent = MazeParent;
			}
		}
	}

	private void destroyCurrentMaze () {
		for (int i = 0; i < MazeParent.childCount; i++) {
			GameObject mazeComponent = MazeParent.GetChild(i).gameObject;
			if (mazeComponent.tag != DONT_DETROY_TAG) {
				Destroy(mazeComponent);
			}
		}
	}

	private void generateMazes () {
		for (int i = 0; i < MazeTemplates.Length; i++) {
			allMazes.Add(createMaze(MazeTemplates[i]));
		}
		currentMaze = allMazes[0];
	}

	private void setCurrentMaze (int mazeIndex) {
		if (allMazes == null || mazeIndex >= allMazes.Count) {
			Debug.LogError("Index is out of range");
		} else {
			currentMaze = allMazes[mazeIndex];
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

	public Maze GetCurrentMaze () {
		return currentMaze;
	}

	public void LoadMaze (int mazeIndex) {
		setCurrentMaze (mazeIndex);
		SpawnMaze ();
	}
}
