using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures; 

public class Group : MonoBehaviour
{
	private float lastFall = 1;
	private float clicked = 0;
	private float clicktime = 0;
	private readonly float clickdelay = 0.3f;
	private SplashManager SplashManager;
	private Figure figure;
	delegate void Splashing();
	Splashing splashing;

	void Start()
    {
		if (!IsValidGridPos())
		{
			Destroy(gameObject);
		}

		SplashManager = SplashManager.Instance;
		if (GetComponent<GroupTest>() != null)
			figure =  GetComponent<GroupTest>().GiveOwnFigure();
		else
			figure = FindObjectOfType<Spawner>().PrepareFigure(this);

		StartCoroutine(DestroyItselfWhenEmpty());
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

		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			clicked++;
			if (clicked == 1) clicktime = Time.time;

			if (clicked > 1 && Time.time - clicktime < clickdelay)
			{
				clicked = 0;
				clicktime = 0;
				MoveToFloor(15);
			}
			else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
		{	
			MoveDownAndFall();
		}
	}

	public void MoveToFloor(int floorLevel)
	{
		transform.position += new Vector3(0, -floorLevel, 0);

		if (!IsValidGridPos())
		{
			transform.position += new Vector3(0, floorLevel, 0);
			MoveToFloor(floorLevel - 1);
		}
		else
		{
			UpdateGrid();
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
			
			FillGrid();
			CheckCollisions();
			FindObjectOfType<Spawner>().SpawnNext();

			enabled = false;
		}

		lastFall = Time.time;
	}

	public void ChangeRotation()
	{
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

	public void RemoveGroupScriptsFromObject()
	{
		enabled = false;
		gameObject.AddComponent<GroupTest>();
	}

	IEnumerator DestroyItselfWhenEmpty()
	{
		while (true)
		{
			if (gameObject.transform.childCount != 0)
			{
				yield return new WaitForSeconds(10f);
			}
			else
			{
				Destroy(gameObject);
				yield break;
			}
		}
	}

	private bool IsValidGridPos()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);

			if (!Grid.InsideBorder(v))
				return false;

			if (Grid.grid[(int)v.x, (int)v.y] != null &&
				Grid.grid[(int)v.x, (int)v.y].parent != transform)
				return false;
		}
		return true;
	}

	private void UpdateGrid()
	{
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.w; ++x)
				if (Grid.grid[x, y] != null)
					if (Grid.grid[x, y].parent == transform)
						Grid.grid[x, y] = null;

		foreach (Transform child in transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);
			Grid.grid[(int)v.x, (int)v.y] = child;
		}
	}

	private void FillGrid()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);
			if ((int)v.y == 0) return;

			while (v.y - 1 >= 0 && Grid.grid[(int)v.x, (int)v.y - 1] == null)
			{
				Grid.grid[(int)v.x, (int)v.y - 1] = child;
				Grid.grid[(int)v.x, (int)v.y - 1].position += new Vector3(0, -1, 0);
				Grid.grid[(int)v.x, (int)v.y] = null;
				v.y = v.y - 1;
			}
		}
	}

	public void CheckCollisions()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);

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
				CheckColorDownElement(child.GetSiblingIndex(), v);
			}
		}
		else
			CheckColorDownElement(child.GetSiblingIndex(), v);
	}

	private void CheckColorDownElement(int ChildIndex, Vector2 v)
	{
		SingleBrick ob2 = Grid.grid[(int)v.x, (int)v.y - 1].gameObject.GetComponent<SingleBrick>();

		if (figure.AllBricks[ChildIndex].MyColor == ob2.MyColor)
		{
			Grid.DeleteElements((int)v.x, (int)v.y);
			UIManager.Instance.UpdatePlayerCount(figure.BrickCost * 2);
			if (splashing == null)
				splashing = SplashingPair;
		}
	}

	private void SplashingPair()
	{
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.RoundVec2(child.position);

			if ((int)v.y == 0) return;

			bool isSomethingDown = Grid.grid[(int)v.x, (int)v.y - 1] != null;
			if (isSomethingDown && Grid.grid[(int)v.x, (int)v.y - 1].parent != transform)
			{
				CheckColorDownElement(child.GetSiblingIndex(), v);
			}
		}
	}

}