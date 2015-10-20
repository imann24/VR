﻿using UnityEngine;
using System.Collections;

public class MazePositioner {
	public const float defaultHeight = 0.5f;

	public static Vector3 PositionFromIndex (int x, int y, float height = defaultHeight) {
		return new Vector3(x, height, y);
	}

}
