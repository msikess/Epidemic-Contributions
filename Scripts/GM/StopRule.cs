using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRule : MonoBehaviour {

	//checks if the player has reached a lose/win state;
	public static string check (GameBuilder1 world, Initializer init)
	{
			Day today = world.retDay (1);
			Day prev;
			if (world.totalDays () > 1) {
				prev = world.retDay (2);
				if (/*Checks the rules for losing the game using variables from Initializer
					(nothing is hardcoded, verythig is set up int the interace of 
					orldModel GameObject)*/) {
					Debug.Log ("You Lost. Day: ... ");
					// "..." stands for the ending state of the game/params;
					return "lost";
				} else if (/*Checks the rules for winning the game using variables from Initializer
					(nothing is hardcoded, verythig is set up int the interace of 
					orldModel GameObject)*/) {
					Debug.Log ("You won! Day: ...");
					// "..." stands for the ending state of the game/params;
					return "won";
				} else {
					return "none";
				}
			} else {
			return "none";
		}
	}
}
