﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour
{	
	[Serializable]
	public class PlayerData
	{
		public int health;
		public int gems;
	}

	private int health;
	private int gems;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();

		FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.Open);

		PlayerData data = new PlayerData();
		data.health = 1;
		data.gems = 1;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.data"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.Open);

			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();


			health = data.health;
			gems = data.gems;
		}
	}
}