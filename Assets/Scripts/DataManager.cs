using System;
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

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();

		FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.Open);

		PlayerData data = new PlayerData
		{
			health = 1,
			gems = 1
		};

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
