using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelScreen : MonoBehaviour {
	[SerializeField]
	private Text description;
	[SerializeField]
	private Text objective;
	private int prevLevel;
	private int level;

	public void levelClick(){
		//On the click of the button the level variable is declared as the parsed
		// int value of the button's name;
		level =  Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
		Parser ("Descriptions");
		Parser ("Objectives");
	}

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

	public void back(){
		SceneManager.LoadScene (0);
	}

	public void play(){
		//if a button has been selected, loads the scene 
		// with an index one greater than the button's name or the
		// level variable;
		if (level == null || level == 0) {
		} else {
			SceneManager.LoadScene (level + 1);
		}
	}
}
