using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Figures;

public class Spawner : MonoBehaviour
{
	public GameObject[] groups;
	private GameObject nextGameObject;
	public Figure figure;

	// Start is called before the first frame update
	void Start()
    {
		//SpawnNextV1();
		//SpawnNext();
		//Debug.Log("Start");
		ShowNextElement();

		Instantiate(groups[Random.Range(0, groups.Length)],
					transform.position,
					Quaternion.identity);
	}

	public void SpawnNextV1()
	{
		int i = Random.Range(0, groups.Length);

		Instantiate(groups[i],
					transform.position,
					Quaternion.identity);
	}

	public void SpawnNext()
	{
		//Debug.Log("SpawnNext");
		//int i = Random.Range(0, groups.Length);

		//Instantiate(groups[i],
		//			transform.position,
		//			Quaternion.identity);
		nextGameObject.transform.position = transform.position;

		if(nextGameObject.GetComponent<GroupTest>() != null)
		{
			nextGameObject.GetComponent<GroupTest>().RemoveGroupTestScriptsFromObject();
		}

		ShowNextElement();
	}

	public void ShowNextElement()
	{
		//Debug.Log("ShowNextElement");
		//int i = Random.Range(0, groups.Length);

		nextGameObject = Instantiate(groups[Random.Range(0, groups.Length)]);
		UIManager.Instance.setNewParent(nextGameObject);//,
					//new Vector3(-513.5f, -227.5f, 0),
					//Quaternion.identity));

	}

	public Figure PrepareFigure(Group group)
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

		//Debug.Log($"PrepareFigure = {figure}");
		return figure;
	}

	public Figure PrepareFigureTest(GroupTest group)
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

		//Debug.Log($"PrepareFigure = {figure}");
		return figure;
	}
}
