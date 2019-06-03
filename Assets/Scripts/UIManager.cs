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

	public void SetNextFigureImage(GameObject gm)
	{
		Debug.Log($"SetNextFigureImage = {gm} + name = {GameObject.FindGameObjectWithTag("NextFigure").name}");
		//GameObject newChild = gm;
		//newChild.transform.parent = GameObject.FindGameObjectWithTag("NextFigure").transform;
		//newChild.transform.localPosition = Vector3.zero;
		
		Instantiate(gm,
					GameObject.FindGameObjectWithTag("NextFigure").transform);

		Debug.Log($"SetNextFigureImage = {transform.position}");
		//nextFigureImage.sprite = _sprite;
	}

	public void setNewParent(GameObject newChild)
	{
		newChild.GetComponent<Group>().RemoveGroupScriptsFromObject();
		newChild.transform.SetParent(GameObject.FindGameObjectWithTag("NextFigure").transform);
		newChild.transform.localPosition = new Vector3(-469f, -213f, 0);
	}
}
