using UnityEngine;
using System.Collections;

public class MazeSelectorController : MonoBehaviour {
	public bool OverlaidIntructionCard = false;

	public GameObject MazeOptionPrefab;
	public TextMesh MazeTitle;

	public const string DEFAULT_TITLE = "Press a Number Key \n To Choose a Maze";
	public const string VICTORY_TITLE = "Maze Complete!\n" + DEFAULT_TITLE;

	private int enableCount;

	// Use this for initialization
	void Start () {
		generateMazeOptions();
	}

	void OnEnable () {
		enableCount++;

		if (enableCount > 1 && OverlaidIntructionCard) {
			changeMazeSelectorTitle(VICTORY_TITLE);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void generateMazeOptions () {
		int offset = 1;
		TextAsset [] mazes = MazeController.Instance.MazeTemplates;
		for (int i = 0; i < mazes.Length; i++) {
			GameObject mazeOption = (GameObject) Instantiate(MazeOptionPrefab);
			                                                

			mazeOption.GetComponent<MazeOption>().SetMazeInformation(i + offset, 
			                                                         mazes[i].name);

			mazeOption.transform.parent = transform;

			mazeOption.transform.localScale = mazeOptionScale();

			mazeOption.transform.localPosition = positionMazeOption(i);

			mazeOption.transform.localRotation = Quaternion.identity;



		}
	}

	private Vector3 positionMazeOption (int mazeNumber, float spacing = 1.0f, float xPos = -2.0f, float yOffset = 0.5f, float zPos = -3.0f) {
		return new Vector3 (xPos, -mazeNumber * spacing + yOffset, zPos);
	}

	private Vector3 mazeOptionScale (float scale = 0.75f) {
		return new Vector3 (scale, scale, scale);
	}

	private void changeMazeSelectorTitle (string title = DEFAULT_TITLE) {
		MazeTitle.text = title;
	}

}
