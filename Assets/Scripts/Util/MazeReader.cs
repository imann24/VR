using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeParser {
	private static string EMPTY_ID = "";
	private static string WALL_ID = "x";
	private static string START_ID = "s";
	private static string FINISH_ID = "f";

	private static Dictionary<string, MazePiece> IDToPiece;
	
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
		CheckIDToPieceDictionary();
		if (!IDToPiece.ContainsKey(pieceID)) {
			Debug.Log((int)pieceID[1] + " " + WALL_ID.Length);
			Debug.LogError(pieceID + " does not exist in dictionary");
			return MazePiece.Empty;
		} else {
			return IDToPiece[pieceID];
		}

	}

	public static void ChangePieceID (string emptyID = null,
	                                  string wallID = null,
	                                  string startID = null,
	                                  string finishID = null) {
		if (emptyID != null) EMPTY_ID = emptyID;
		if (wallID != null) WALL_ID = wallID;
		if (startID != null) START_ID = startID;
		if (finishID != null) FINISH_ID = finishID;
	}

	private static void CheckIDToPieceDictionary () {
		if (IDToPiece == null) {
			IDToPiece = new Dictionary<string, MazePiece>();
			IDToPiece.Add(EMPTY_ID, MazePiece.Empty);
			IDToPiece.Add(WALL_ID, MazePiece.Wall);
			IDToPiece.Add(START_ID, MazePiece.Start);
			IDToPiece.Add(FINISH_ID, MazePiece.Finish);
		}
	}
}
