using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures;

public class Group : MonoBehaviour
{
	private float lastFall = 1;
	private Figure figure;
	delegate void Splashing();
	Splashing splashing;

	void Start()
    {
		Debug.Log("figure");
		if (!IsValidGridPos())
		{
			Destroy(gameObject);
		}

		if(GetComponent<GroupTest>() != null)
		{
			GetComponent<GroupTest>().GiveOwnFigure();
		}
		else
		{
			figure = FindObjectOfType<Spawner>().PrepareFigure(this);
		}
		//Debug.Log($"figure = {figure.HasSecondFloor}");
		//figure = new Figure(this.gameObject.tag)
		//{
		//	AllBricks = gameObject.GetComponentsInChildren<SingleBrick>()
		//};

		//figure.FillBricksHashMap();

		//for (int i = 0; i < transform.childCount; i++)
		//{
		//	if (i % 2 == 1 && figure.HasSecondFloor)
		//	{
		//		transform.GetChild(i).GetComponent<SpriteRenderer>().color = figure.SecondSpriteColor;
		//		continue;
		//	}
		//	transform.GetChild(i).GetComponent<SpriteRenderer>().color = figure.FirstSpriteColor;
		//}
	}

	public void SetFigure(Figure _figure)
	{
		Debug.Log("SetFigure");
		this.figure = _figure;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			ChangeRotation();
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveLeft();
		}

		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveRight();
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
		{
			MoveDownAndFall();
		}
	}

	public void MoveLeft()
	{
		transform.position += new Vector3(-1, 0, 0);
			
		if (IsValidGridPos())
			UpdateGrid();
		else
			transform.position += new Vector3(1, 0, 0);
		
	}

	public void MoveRight()
	{
		transform.position += new Vector3(1, 0, 0);
		
		if (IsValidGridPos())
			UpdateGrid();
		else
			transform.position += new Vector3(-1, 0, 0);
	}

	public void MoveDownAndFall()
	{
		transform.position += new Vector3(0, -1, 0);
		
		if (IsValidGridPos())
		{
			UpdateGrid();
		}
		else
		{
			transform.position += new Vector3(0, 1, 0);
			
			CheckCollisions();
			
			FindObjectOfType<Spawner>().SpawnNext();
			//FindObjectOfType<Spawner>().SpawnNextV1();

			enabled = false;
		}

		lastFall = Time.time;
	}

	public void ChangeRotation()
	{
		//Debug.Log(figure.RotatedBy180GameObject);
		if (!figure.HasSecondFloor) return;

		if (!figure.RotatedBy180GameObject)
			figure.RotatedBy180GameObject = true;
		else
			figure.RotatedBy180GameObject = false;
		RotateElement();
	}

	public void RotateElement()
	{
		int val = figure.upperColorNumber;
		figure.upperColorNumber = figure.lowerColorNumber;
		figure.lowerColorNumber = val;

		for (int i = 0; i < transform.childCount; i += 2)
		{
			this.gameObject.transform.GetChild(i).transform.position     += new Vector3(0, figure.RotatedBy180GameObject ? 1 : -1, 0);
			this.gameObject.transform.GetChild(i + 1).transform.position += new Vector3(0, figure.RotatedBy180GameObject ? -1 : 1, 0);
		}
	}

	public virtual void CheckCollisions()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.roundVec2(child.position);

			if (v.y == 0) continue;

			bool isSomethingDown = Grid.grid[(int)v.x, (int)v.y - 1] != null;
			if (isSomethingDown && Grid.grid[(int)v.x, (int)v.y - 1].parent != transform)
			{
				PlaceOfConnection(child, v);
			}
		}
		
		if (figure.HasSecondFloor && splashing != null)
			splashing();
	}

	public void PlaceOfConnection(Transform child, Vector2 v)
	{
		if (figure.HasSecondFloor)
		{
			if (child.GetSiblingIndex() % 2 == (figure.RotatedBy180GameObject ? 1 : 0))
			{
				CheckColorDownElementV2(child.GetSiblingIndex(), v);
			}
		}
		else
			CheckColorDownElementV2(child.GetSiblingIndex(), v);
	}

	public void CheckColorDownElement(int ChildIndex, Vector2 v)
	{
		Group ob = Grid.grid[(int)v.x, (int)v.y - 1].gameObject.GetComponentInParent<Group>();
		int underColorNumber = ob.figure.HasSecondFloor ? 
			ob.figure.upperColorNumber : ob.figure.lowerColorNumber;

		if (figure.lowerColorNumber == underColorNumber)
		{
			Grid.DeleteElements((int)v.x, (int)v.y);
			splashing = SplashingPair;
		}
	}
	//свернуть с куском кода из делегата
	public void CheckColorDownElementV2(int ChildIndex, Vector2 v)
	{
		SingleBrick ob2 = Grid.grid[(int)v.x, (int)v.y - 1].gameObject.GetComponent<SingleBrick>();

		if (figure.AllBricks[ChildIndex].MyColor == ob2.MyColor)
		{
			Grid.DeleteElements((int)v.x, (int)v.y);
			UIManager.Instance.UpdatePlayerCount(figure.BrickCost * 2);
			splashing = SplashingPair;
		}
	}

	bool IsValidGridPos()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.roundVec2(child.position);

			// Not inside Border?
			if (!Grid.InsideBorder(v))
				return false;

			// Block in grid cell (and not part of same group)?
			if (Grid.grid[(int)v.x, (int)v.y] != null &&
				Grid.grid[(int)v.x, (int)v.y].parent != transform)
				return false;
		}
		return true;
	}

	void UpdateGrid()
	{
		// Remove old children from grid
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.w; ++x)
				if (Grid.grid[x, y] != null)
					if (Grid.grid[x, y].parent == transform)
						Grid.grid[x, y] = null;

		// Add new children to grid
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.roundVec2(child.position);
			Grid.grid[(int)v.x, (int)v.y] = child;
		}
	}
	
	//Delegate
	private void SplashingPair()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.roundVec2(child.position);

			if (v.y == 0) continue;

			bool isSomethingDown = Grid.grid[(int)v.x, (int)v.y - 1] != null;
			if (isSomethingDown && Grid.grid[(int)v.x, (int)v.y - 1].parent != transform)
			{
				SingleBrick ob2 = Grid.grid[(int)v.x, (int)v.y - 1].gameObject.GetComponent<SingleBrick>();

				if (figure.AllBricks[child.GetSiblingIndex()].MyColor == ob2.MyColor)
				{
					Grid.DeleteElements((int)v.x, (int)v.y);
					UIManager.Instance.UpdatePlayerCount(figure.BrickCost * 2);
				} 
			}
		}
	}

	public void RemoveGroupScriptsFromObject()
	{
		enabled = false;
		gameObject.AddComponent<GroupTest>();
	}
}
