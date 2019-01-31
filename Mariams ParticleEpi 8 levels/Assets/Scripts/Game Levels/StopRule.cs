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
				if (world.totalDays () >= init.getParam ("dayLimit")
					|| (today.get ("totalSymps") / init.getParam ("population")) >= .3
					|| prev.get ("totalCost") >= (int)init.getParam ("fund")) {
					Debug.Log ("You Lost. Day: "
						+ world.totalDays ()
						+ ", Percent Diseased: "
						+ (today.get ("sickNum") / init.getParam ("population"))
						+ ", Total Cost: "
						+ today.get ("totalCost") + ".");
					return "lost";
				} else if (today.get ("totalSymps") <= init.getParam("goalPeople")) {
					Debug.Log ("You won! Day: "
					+ world.totalDays ()
					+ ", Percent Diseased: "
					+ (today.get ("sickNum") / init.getParam ("population"))
					+ ", Total Cost: "
					+ today.get ("totalcost") + ".");
					return "won";
				} else {
					return "none";
				}
			} else {
			return "none";
		}
	}
}
