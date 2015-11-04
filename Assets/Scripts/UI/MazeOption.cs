using UnityEngine;
using System.Collections;

public class MazeOption : MonoBehaviour {
	public TextMesh MazeTitle;
	public TextMesh MazeNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetMazeInformation (int number, string title) {
		MazeTitle.text = title;
		MazeNumber.text = number.ToString();
	}
}
