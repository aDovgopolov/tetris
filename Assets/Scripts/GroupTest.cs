using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures;

public class GroupTest : MonoBehaviour
{
	private Figure figure;

	void Start()
    {
		Debug.Log("figure GroupTest");

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

		figure = FindObjectOfType<Spawner>().PrepareFigureTest(this);

		//Debug.Log($"figure = {figure}");
		foreach (Transform child in transform)
		{
			Vector2 v = Grid.roundVec2(child.position);
		}
	}
	
	public void RemoveGroupTestScriptsFromObject()
	{
		enabled = false;
		gameObject.GetComponent<Group>().enabled = true;
		gameObject.GetComponent<Group>().SetFigure(figure);
	}

	public Figure GiveOwnFigure()
	{
		return figure;
	}
}
