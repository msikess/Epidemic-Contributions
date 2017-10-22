using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

//This is the file that gathers the serialized variables of each day object. 
// Later the resultig string is sent/parsed to a server that passes it to a 
// shinyApp which creates a table of game's progression. 

public class Json : MonoBehaviour {

	private String jBuilder;

	public Json(){
		jBuilder = null;
	}

	public void jAdder(Calendar world, WorldModel wM){
		String jS;
		if (wM.gameState () != "none") {
			jS = JsonUtility.ToJson (world[world.Count() - 1]);
		} else {
			jS = JsonUtility.ToJson (world[world.Count() - 1]) + ",";
		}
		jBuilder += jS;
	}

	public String getter(){
		return jBuilder;
	}		
}

