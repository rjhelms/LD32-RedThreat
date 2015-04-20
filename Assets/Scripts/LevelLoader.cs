﻿using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public class LevelLoader : MonoBehaviour
{

	public GameObject[] LevelPrefabs;
	public GameObject[] ActorPrefabs;

	public Transform LevelParentTransform;
	public Transform[] ActorParentTransforms;

	public TextAsset Level;
	public TextAsset Actors;
	public AstarPath Pathfinder;
	public GameController MyController;

	// Use this for initialization
	void Start ()
	{
		MyController = GameObject.FindObjectOfType<GameController> ();
		string[][] levelArray = ReadLevel (Level);
		BuildLevel (levelArray);
		Pathfinder.Scan ();

		string[][] actorArray = ReadLevel (Actors);
		BuildActors (actorArray);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public string[][] ReadLevel (TextAsset file)
	{
		string text = file.text;
		string[] lines = Regex.Split (text, "\r\n");
		int rows = lines.Length;
		
		string[][] levelBase = new string[rows][];
		for (int i =0; i< lines.Length; i++) {
			string[] stringsOfLine = Regex.Split (lines [i], ",");
			levelBase [i] = stringsOfLine;
		}
		return levelBase;
	}

	public void BuildLevel (string[][] levelArray)
	{
		for (int i = 0; i < levelArray.Length; i++) {
			for (int j = 0; j < levelArray[i].Length; j++) {
				string currentObjectString = levelArray [i] [j];
				if (!string.IsNullOrEmpty (currentObjectString)) {
					int objectIndex = Convert.ToInt32 (currentObjectString);
					if (LevelPrefabs [objectIndex] != null) {
						float xpos = j * 32;
						float ypos = (levelArray.Length - (i + 1)) * 32;

						GameObject currentObject = (GameObject)Instantiate (LevelPrefabs [objectIndex], 
						                                                   new Vector2 (xpos, ypos), 
						                                                    Quaternion.identity);
						currentObject.transform.parent = LevelParentTransform;
					}
				}
			}
		}
	}

	public void BuildActors (string[][] actorArray)
	{
		for (int i = 0; i < actorArray.Length; i++) {
			string currentObjectString = actorArray [i] [0];
			if (!string.IsNullOrEmpty (currentObjectString)) {
				int objectIndex = Convert.ToInt32 (currentObjectString);
				if (ActorPrefabs [objectIndex] != null) {
					float xpos = Convert.ToInt32 (actorArray [i] [1]) * 32;
					float ypos = Convert.ToInt32 (actorArray [i] [2]) * 32;

					GameObject currentObject = (GameObject)Instantiate (ActorPrefabs [objectIndex],
					                                                   new Vector2 (xpos, ypos),
					                                                   Quaternion.identity);
					currentObject.transform.parent = ActorParentTransforms [objectIndex];
				}
			}
		}
	}

}
