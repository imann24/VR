using UnityEngine;
using System.Collections;

public class Maze {
	private MazePiece [][] pieces;

	public Maze (MazePiece[][] pieces) {
		this.pieces = pieces;
	}

	public void ModifyPiece (int x, int y, MazePiece newType) {
		if (pieces == null ||
		    x >= pieces.Length ||
		    y >= pieces[x].Length) {
			Debug.LogError("Not a valid maze peice at " + x + ", " + y);
			return;
		} else {
			pieces[x][y] = newType;
		}
	}

	public MazePiece [][] GetPieces () {
		return pieces;
	}
}
