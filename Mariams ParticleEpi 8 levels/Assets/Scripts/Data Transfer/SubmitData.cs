using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//this script sends game data to the server
public class SubmitData : MonoBehaviour {



	//This Method respond to the click on the submit button
	public void SubmitUpload(){
		WorldModel world = GameObject.Find ("WorldModel").GetComponent<WorldModel> ();
		Json j = world.jGet ();
		string JSONData = j.getter ();
		JSONData = "[" + JSONData + "]";
		StartCoroutine (Upload (JSONData));
	}

	//Standard method from Unity WebRequest ***need more research***
	IEnumerator Upload(string JSONData) {

		WWWForm form = new WWWForm();
		form.AddField("JSONData", JSONData);
		form.headers["Access-Control-Allow-Credentials"] = "true";
		form.headers["Access-Control-Allow-Headers"] = "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time";
		form.headers["Access-Control-Allow-Methods"] = "GET, POST, OPTIONS";
		form.headers ["Access-Control-Allow-Origin"] = "*";

		//Link to communicate with the amazon server
		using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-54-174-97-112.compute-1.amazonaws.com:3000", form))
		{
			yield return www.Send();

			if (www.isNetworkError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("Form upload complete!");
			}
		}
	}


}
