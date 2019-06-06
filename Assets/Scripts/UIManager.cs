using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager _instance;
	private int totalCount = 0;
	
	public Text playerCount;
	public Image nextFigureImage;

	public static UIManager Instance
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

	public void UpdatePlayerCount(int count)
	{
		totalCount += count;
		playerCount.text = "" + totalCount;
	}

	public void SetNewParent(GameObject newChild)
	{
		newChild.GetComponent<Group>().RemoveGroupScriptsFromObject();
		newChild.transform.SetParent(GameObject.FindGameObjectWithTag("NextFigure").transform);
		newChild.transform.localPosition = new Vector3(-461.5f, -208.5f, 0);
	}
}
