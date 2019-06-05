using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
	private static SplashManager _instance;

	public static SplashManager Instance
	{
		get
		{
			if (_instance == null)
			{

				Debug.LogError("Error");
			}
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
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
