using UnityEngine;
using System.Collections;

public class Maze {
	private MazePiece [][] pieces;
	public string Name {get; private set;}

	public Maze (MazePiece[][] pieces) {
		this.pieces = pieces;
	}

	// Constructor to copy a current maze's contents
	public Maze (Maze maze) {
		this.pieces = maze.GetPieces();
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

	// Overloaded method for the position
	public void ModifyPiece (Position position, MazePiece newType) {
		ModifyPiece(position.GetX(), 
		            position.GetY(),
		            newType);
	}

	public MazePiece [][] GetPieces () {
		return pieces;
	}

	public MazePiece MazePieceFromPosition (Position position) {
		return pieces[position.GetX()][position.GetY()];
	}

	public MazePiece[] MazePiecesFromPositions (Position[] positions) {
		MazePiece[] mazePieces = new MazePiece[positions.Length];

		for (int i = 0; i < positions.Length; i++) {
			mazePieces[i] = MazePieceFromPosition(positions[i]);
		}

		return mazePieces;
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

	public bool WithinMazeInnerBounds (int x, int y) {
		return WithinMazeInnerBounds(new Position(x, y));
	}

	public bool WithinMazeInnerBounds (Position position) {
		if (!WithinMazeOuterBounds(position)) {
			return false;
		}

		Position [] nearbyPositions = position.AdjacentSquares();
		int adjacentWallCount = 0;

		for (int i = 0; i < nearbyPositions.Length; i++) {
			if (!WithinMazeOuterBounds(nearbyPositions[i])) {
				Debug.Log(nearbyPositions[i]);
				continue;
			}
			Debug.Log(this);
			Debug.Log(nearbyPositions[i]);
			if (MazePieceFromPosition(nearbyPositions[i]) == MazePiece.Wall) {
				adjacentWallCount++;
			}
		}

		return adjacentWallCount >= 1;
	}

	public bool WithinMazeOuterBounds (int x, int y) {
		return WithinMazeOuterBounds(new Position(x, y));
	}

	public bool WithinMazeOuterBounds (Position position) {
		return Util.WithinRange(position.GetX(), Width()) &&
			Util.WithinRange(position.GetY(), Height());
	}

	public override string ToString () {
		return Width() + "x" + Height() + " Maze";
	}

	public void SetName (string name) {
		Name = name;
	}
}
