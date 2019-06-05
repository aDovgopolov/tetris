 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures;

public class Spawner: MonoBehaviour
{
	public GameObject[] groups;
	private GameObject nextGameObject;
	public Figure figure;
	
	void Start()
    {
		ShowNextElement();

		Instantiate(groups[Random.Range(0, groups.Length)],
					transform.position,
					Quaternion.identity);
	}

	public void SpawnNext()
	{
		nextGameObject.transform.position = transform.position;

		if(nextGameObject.GetComponent<GroupTest>() != null)
		{
			nextGameObject.GetComponent<GroupTest>().RemoveGroupTestScriptsFromObject();
		}

		ShowNextElement();
	}


	public void ShowNextElement()
	{
		nextGameObject = Instantiate(groups[Random.Range(0, groups.Length)]);
		UIManager.Instance.SetNewParent(nextGameObject);
	}


	public Figure PrepareFigure<T>(T group) where T : Component
	{
		figure = new Figure(group.gameObject.tag)
		{
			AllBricks = group.gameObject.GetComponentsInChildren<SingleBrick>()
		};

		figure.FillBricksHashMap();

		for (int i = 0; i < group.transform.childCount; i++)
		{
			if (i % 2 == 1 && figure.HasSecondFloor)
			{
				group.transform.GetChild(i).GetComponent<SpriteRenderer>().color = figure.SecondSpriteColor;
				continue;
			}
			group.transform.GetChild(i).GetComponent<SpriteRenderer>().color = figure.FirstSpriteColor;
		}
		
		return figure;
	}
}
