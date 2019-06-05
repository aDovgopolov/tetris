using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public static int w = 10;
	public static int h = 20;
	public static Transform[,] grid = new Transform[w, h];

	public static Vector2 RoundVec2(Vector2 v)
	{
		return new Vector2(Mathf.Round(v.x),
						   Mathf.Round(v.y));
	}

	public static bool InsideBorder(Vector2 pos)
	{
		return ((int)pos.x >= 0 &&
				(int)pos.x < w &&
				(int)pos.y >= 0);
	}

	public static void DeleteElements(int x, int y)
	{
		Destroy(grid[x, y].gameObject);
		Destroy(grid[x, y - 1].gameObject);
		grid[x, y] = null;
		grid[x, y - 1] = null;

		if (grid[x, y + 1] != null)
		{
			grid[x, y - 1] = grid[x, y + 1];
			grid[x, y + 1] = null;
			
			grid[x, y - 1].position += new Vector3(0, -2, 0);
		}
	}

}
