using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//This script sends game data to the server.
public class SubmitData : MonoBehaviour {



	//This Method respond to the click on the submit button
	public void SubmitUpload(){
		WorldModel world = GameObject.Find ("WorldModel").GetComponent<WorldModel> ();
		Json j = world.jGet ();
		string JSONData = j.getter ();
		JSONData = "[" + JSONData + "]";
		StartCoroutine (Upload (JSONData));
	}

	
	IEnumerator Upload(string JSONData) {
		// The code does not belong to me, so I refrain from sharing it;
	}


}
