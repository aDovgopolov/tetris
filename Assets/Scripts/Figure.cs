﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Figures
{
	public class Figure
	{
		public SingleBrick[] AllBricks{get; set;}
		public int upperColorNumber;
		public int lowerColorNumber = 999;
		public bool RotatedBy180GameObject { get; set; } = false;

		public bool HasSecondFloor
		{
			get; private set;
		}
		public Color FirstSpriteColor
		{
			get; set;
		}

		public Color SecondSpriteColor
		{
			get; set;
		}

		public int BrickCost
		{
			get; private set;
		} = 10;

		public Figure(string tag)
		{
			FirstSpriteColor = GetColor(1);
			SecondSpriteColor = new Color();

			if (tag.Equals("HasSecondFloor"))
			{
				HasSecondFloor = true;
				SecondSpriteColor = GetColor(2);
			}
		}

		public void FillBricksHashMap()
		{
			for (int i = 0; i < AllBricks.Length; i++)
			{
				if (i % 2 == 1 && HasSecondFloor)
				{
					AllBricks[i].MyColor = upperColorNumber; 
					continue;
				}
				AllBricks[i].MyColor = lowerColorNumber;
			}
		}

		public Color GetColor(int floor)
		{
			Color color;
			int j = Random.Range(0, 4);

			switch (j)
			{
				case 0:
					ColorUtility.TryParseHtmlString("#FFFFFF", out color);
					if (floor == 1)
						lowerColorNumber = 0;
					else
						upperColorNumber = 0;
					//Debug.Log("red");
					break;
				case 1:
					ColorUtility.TryParseHtmlString("#964E4E", out color);
					if (floor == 1)
						lowerColorNumber = 1;
					else
						upperColorNumber = 1;
					break;
				case 2:
					color = Color.green;
					if (floor == 1)
						lowerColorNumber = 2;
					else
						upperColorNumber = 2;
					break;
				case 3:
					color = Color.yellow;
					if (floor == 1)
						lowerColorNumber = 3;
					else
						upperColorNumber = 3;
					break;
				case 4:
					color = Color.gray; lowerColorNumber = 4;
					break;
				default:
					color = Color.cyan; lowerColorNumber = 5;
					break;
			}

			return color;
		}
	}
}
