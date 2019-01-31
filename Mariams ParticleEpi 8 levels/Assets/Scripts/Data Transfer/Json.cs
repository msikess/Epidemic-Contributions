using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class Json : MonoBehaviour {

	private String jBuilder;

	public Json(){
		jBuilder = null;
	}

	public void jAdder(GameBuilder1 world, Initializer init, WorldModel wM){
		String jS;
		if (wM.gameState () != "none") {
			jS = JsonUtility.ToJson (new JSONData (world, init));
		} else {
			jS = JsonUtility.ToJson (new JSONData (world, init)) + ",";
		}
		jBuilder += jS;
	}

	public String getter(){
		return jBuilder;
	}		
}

public class JSONData {
	[SerializeField]
	private string playerID;
	[SerializeField]
	private string groupID;
	[SerializeField]
	private int day;
	[SerializeField]
	private int population;
	[SerializeField]
	private int budget;
	[SerializeField]
	private int availToTreat;
	[SerializeField]
	private int aTreat;
	[SerializeField]
	private int aCure;
	[SerializeField]
	private int bTreat;
	[SerializeField]
	private int bCure;
	[SerializeField]
	private int aCost;
	[SerializeField]
	private int bCost;
	[SerializeField]
	private int sickCost;

	public JSONData(GameBuilder1 world, Initializer init) {
		Day cur = world.retDay (1);

		playerID = PlayerData.playerdata.playerID;
		groupID = PlayerData.playerdata.groupID;
		day = cur.get ("day");
		population = (int)init.getParam("population");
		budget = (int)init.getParam("fund") - cur.get("totalCost");
		availToTreat = cur.get ("totalSymps");
		aTreat = cur.get ("sickTrtA");
		aCure = cur.get ("curedANum");
		bTreat = cur.get ("sickTrtB");
		bCure = cur.get ("curedBNum");
		aCost = (int)init.getParam ("costA") * cur.get ("sickTrtA");
		bCost = (int)init.getParam ("costB") * cur.get ("sickTrtB");
		sickCost = cur.get ("totalSymps") * 100;
	}
}
