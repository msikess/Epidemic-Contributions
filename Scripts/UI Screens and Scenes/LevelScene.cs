using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.IO;

//Script for the level selection scene that comes after the start/main menu
// (as a result of the player clicking the Start button);
public class LevelScene : MonoBehaviour {
	[SerializeField]
	private Text description;
	[SerializeField]
	private Text objective;
	private int level;

//Once a level is clicked/selected, displays the relevant info in the 
// corresponding text boxes; 
	public void levelClick(){
		level =  Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
		Parser ("Descriptions");
		Parser ("Objectives");
	}

//Parser for the texts to be displayed (Parses from a txt file in
// the Resources folder);
	public void Parser (String textBox)
	{
		TextAsset txt = Resources.Load (textBox) as TextAsset;
		String[] file = Regex.Split (txt.text, "\n|\r|\r\n");
		int lvl = 0;
		int levl;
		foreach (String line in file) {
			if (Int32.TryParse (line, out levl) == true) {
				lvl = Int32.Parse (line);
				continue;
			} 
			if (line.Equals ("")) {
				continue;
			}
			if (lvl == level) {
				if (textBox == description.name) {
					description.text = line;
				} else {
					objective.text = line;
				}
			} else
				continue;
		}
	}

//Takes the player back to the main menu;
	public void back(){
		SceneManager.LoadScene (0);
	}

//Allows the player to start game only onec a level is selected;
// (+1 is considering that the level screen has an index too.
// Therefore, the build index of level one becomes 2, for \
// level two 3, etc);
	public void play(){
		if (level == null || level == 0) {
		} else {
			SceneManager.LoadScene (level + 1);
		}
	}
}
