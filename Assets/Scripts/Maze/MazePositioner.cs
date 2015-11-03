using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazePositioner {
	public const float defaultHeight = 0.5f;

	static private Position goalPosition;
	
	static bool [,] visitedSpaces;

	public static Vector3 PositionFromIndex (int x, int y, float height = defaultHeight, float anchorOffset = 0) {
		return new Vector3(x - MazeController.Instance.GetCurrentMaze().Width()/2 - anchorOffset,
		                   height,
		                   y -  MazeController.Instance.GetCurrentMaze().Height()/2 - anchorOffset);
	}

	public static Vector3 PositionFromIndex (Position position, float height = defaultHeight, float anchorOffset = 0) {
		return PositionFromIndex (position.GetX(), position.GetY(), height, anchorOffset);
	}

	public static Vector3[] WorldPathFromPosition (Position startPosition, Position endPosition, Maze maze) {		
		setGoalPosition(endPosition);
		int w = maze.Width();
		int h = maze.Height();
		bool[][] path = Util.InitializeMatrixAsJaggedArray(new bool[w][], w, h);
		bool[][] visited = Util.InitializeMatrixAsJaggedArray(new bool[w][], w, h);
		if (arraySearch(ref path, 
		            ref visited, 
		            startPosition.GetX(), 
		            startPosition.GetY())) {
			return MazePieceController.GetWorldPath(MazePiecesFromPositions(
				pathFromSpaces(path, startPosition, new List<Position>()).ToArray(),
				MazeController.Instance.GetMazePieceControllers()));

		} else {
			return null;
		}
	}



	public static MazePieceController[] MazePiecesFromPositions (Position[] positions, MazePieceController[][] mazePieces) {
		MazePieceController[] mazePieceControllers = new MazePieceController[positions.Length];

		for (int i = 0; i < mazePieceControllers.Length; i++) {
			mazePieceControllers[i] = MazeController.Instance.MazePieceControllerFromPosition(positions[i]);
		}

		return mazePieceControllers;
	}

	public static Position GetClosestMazePosition (Vector3 worldPosition) {
		float minDistance = float.MaxValue;
		float currentDistance;
		Position closestPosition = null;
	
		foreach (Vector3 mazePieceWorldPosition in MazePieceController.WorldToMazePositions.Keys) {
			if (minDistance > (currentDistance = Vector3.Distance(worldPosition, mazePieceWorldPosition))) {
				closestPosition = MazePieceController.WorldToMazePositions[mazePieceWorldPosition];
				minDistance = currentDistance;
			}
		}

		return closestPosition;
	}
	private static bool inBounds (Position position) {
		Maze maze = MazeController.Instance.GetCurrentMaze();
		int x = position.GetX();
		int y = position.GetY();
		return (x >= 0 &&
		    	x < maze.Width() &&
		    	y >= 0 &&
		        y < maze.Height() && 
		        !isWall(maze.GetPieces()[x][y]));
	}

	private static bool inBounds (int x, int y) {
		return inBounds(new Position(x, y));
	}

	private static void setGoalPosition (Position goalPosition) {
		MazePositioner.goalPosition = goalPosition;
	}

	private static void debugPath (List<Position> positions) {
		string path = "Here's the path ";
		foreach (Position position in positions) {
			path += position.ToString() + " ";
		}
		Debug.Log(path);
	}

	private static void debugMaze (bool[][] maze) {
		string mazeAsString = "";

		for (int x = maze.Length-1; x >= 0; x--) {
			for (int y = 0; y < maze[x].Length; y++) {
				mazeAsString += maze[y][x]?"X":"_";
			}
			mazeAsString += '\n';
		}

		Debug.Log(mazeAsString);
	}

	private static bool arraySearch (ref bool [][] pathSpaces,
	                              ref bool [][] visitedSpaces,
	                              int x, int y) {

		if (isGoal(x, y)) {
			pathSpaces[x][y] = true;
			return true;
		} else if (!inBounds(new Position(x, y)) || visitedSpaces[x][y]) {
			return false;
		}
	
		visitedSpaces[x][y] = true;


		if (inBounds(x, y+1)) {
			if (arraySearch(ref pathSpaces, ref visitedSpaces, x, y+1)) {
				pathSpaces[x][y+1] = true;
				return true;
			}
		} 



		if (inBounds(x, y-1)) {
			if (arraySearch(ref pathSpaces, ref visitedSpaces, x, y-1)) {
				pathSpaces[x][y-1] = true;
				return true;
			}
		}

	

		if (inBounds(x-1, y)) {
			if (arraySearch(ref pathSpaces, ref visitedSpaces, x - 1, y)) {
				pathSpaces[x-1][y] = true;
				return true;
			}
		}

		if (inBounds(x+1, y)) {
			if (arraySearch(ref pathSpaces, ref visitedSpaces, x+1, y)) {
				pathSpaces[x+1][y] = true;
				return true;
			}
		}

		return false;
	}

	private static List<Position> pathFromSpaces (bool[][] pathSpaces, 
	                                              int x, int y,
	                                              List<Position> path) {
		return pathFromSpaces(pathSpaces, new Position(x, y), path);

	}
	private static List<Position> pathFromSpaces (bool[][] pathSpaces, 
	                                              Position currentPosition, 
	                                              List<Position> path) {
		if (path.Count != 0 && path.Contains(goalPosition))
			return path;

		int x = currentPosition.GetX ();
		int y = currentPosition.GetY ();

		Position newPosition; 
		if (inBounds(x, y+1) && 
		    pathSpaces[x][y+1] && 
		    !path.Contains(newPosition = new Position(x, y+1))) {

			path.Add(newPosition);
			path = pathFromSpaces(pathSpaces, x, y+1, path);

		} else if (inBounds(x+1, y) && 
		           pathSpaces[x+1][y] && 
		           !path.Contains(newPosition = new Position(x+1, y))) {

			path.Add(newPosition);
			path = pathFromSpaces(pathSpaces, x+1, y, path);

		}  else if (inBounds(x, y-1) && 
		            pathSpaces[x][y-1] && 
		            !path.Contains(newPosition = new Position(x, y-1))) {

			path.Add(newPosition);
			path = pathFromSpaces(pathSpaces, x, y-1, path);

		}  else if (inBounds(x-1, y) &&
		            pathSpaces[x-1][y] &&
		            !path.Contains(newPosition = new Position(x-1, y))) {

			path.Add (newPosition);
			path = pathFromSpaces(pathSpaces, x-1, y, path);

		} 
		return path;

	}

	private static bool isGoal (int x, int y) {
		return x == goalPosition.GetX() &&
			y == goalPosition.GetY ();
	}

	private static bool isWall (MazePiece pieceType) {
		return pieceType == MazePiece.Wall ||
			pieceType == MazePiece.DestroyableWall;
	}
}
