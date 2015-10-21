using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MazeParser {
	private static string [] EMPTY_ID = {"", " "};
	private static string [] WALL_ID = {"x"};
	private static string [] START_ID = {"s"};
	private static string [] FINISH_ID = {"f"};
	
	public static Maze ParseMaze (string [][] pieceIDs) {
		MazePiece[][] mazePieces = new MazePiece[pieceIDs.GetLength(0)][];

		for (int x = 0; x < pieceIDs.Length; x++) {
			mazePieces[x] = new MazePiece[pieceIDs[x].Length];
			for (int y = 0; y < pieceIDs[x].Length; y++) {
				mazePieces[x][y] = ParsePiece(pieceIDs[x][y]);
			}
		}

		return new Maze(mazePieces);
	}

	public static MazePiece ParsePiece (string pieceID) {
		return CheckIDToPiece(pieceID);
	}

	public static void ChangePieceIDs (string [] emptyID = null,
	                                  string [] wallID = null,
	                                  string [] startID = null,
	                                  string [] finishID = null) {
		if (emptyID != null) EMPTY_ID = emptyID;
		if (wallID != null) WALL_ID = wallID;
		if (startID != null) START_ID = startID;
		if (finishID != null) FINISH_ID = finishID;
	}

	public static void AddPieceID (string pieceID, MazePiece type)  {
		string[] idToAdd = {pieceID};
		if (type == MazePiece.Empty) EMPTY_ID.Concat(idToAdd);
		else if (type == MazePiece.Wall) WALL_ID.Concat(idToAdd);
		else if (type == MazePiece.Start) START_ID.Concat(idToAdd);
		else if (type == MazePiece.Finish) FINISH_ID.Concat(idToAdd);
	}
	
	private static MazePiece CheckIDToPiece (string pieceID) {
		if (Util.ArrayContains(EMPTY_ID, pieceID)) return MazePiece.Empty;
		else if (Util.ArrayContains(WALL_ID, pieceID)) return MazePiece.Wall;
		else if (Util.ArrayContains(START_ID, pieceID)) return MazePiece.Start;
		else if (Util.ArrayContains(FINISH_ID, pieceID)) return MazePiece.Finish;
		else {
			Debug.LogError(pieceID + " does not exist in dictionary");
			return MazePiece.Empty;
		}
	}
}
