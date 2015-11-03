using UnityEngine;

public class Position {

	private int x, y;

	public Position (int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int GetX() {
		return x;
	}

	public int GetY() {
		return y;
	}

	public void SetPosition (int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals(System.Object obj) {
		Position position = (Position) obj;
		if (position != null) {
			return Equals(position);
		} else {
			return false;
		}
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public bool Equals (Position position) {
		return Distance(position) == 0;
	}
	
	public bool Adjacent(Position position) {
		return Distance(position) <= 1;
	}

	public float Distance (Position position) {
		return Mathf.Sqrt(
			Mathf.Pow(x - position.GetX(), 2) +
			Mathf.Pow(y - position.GetY(), 2));
	}
	
	public Position Translate (int x, int y) {
		return new Position (this.x + x, this.y + y);
	}

	public Position[] AdjacentSquares () {
		Position[] positions = {
			Translate(1, 0), 
			Translate(-1, 0), 
			Translate(0, 1), 
			Translate(0, -1)
		};

		return positions;
	}

	public static int Area (Position position1, Position position2) {
		return Mathf.Abs(position1.GetX() - position2.GetX()) *
			Mathf.Abs(position1.GetY() - position2.GetY());
	}

	public static Position RandomPositionInRange (Position minPosition, Position maxPosition) {
		return new Position(Random.Range(minPosition.GetX(), 
		                                 maxPosition.GetX() + 1),
		                    Random.Range(minPosition.GetY(), 
		             maxPosition.GetY() + 1));
	}

	public override string ToString () {
		return ("(" + x + ", " + y + ")");
	}

}
