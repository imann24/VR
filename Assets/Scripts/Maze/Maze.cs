﻿using UnityEngine;
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

	public int Width () {
		return pieces.Length;
	}

	public int Height () {
		int maxHeight = 0;

		for (int i = 0; i < pieces.Length; i++) {
			if (maxHeight < pieces[i].Length) maxHeight = pieces[i].Length;
		}

		return maxHeight;
	}
}