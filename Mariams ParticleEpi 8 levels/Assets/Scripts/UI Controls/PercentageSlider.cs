using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PercentageSlider : MonoBehaviour {

	//for the sliders that show at what percent each tube is;

	public static void setter(){
		Image tubeR = GameObject.Find ("TubeFillerR").GetComponent<Image> ();
		Slider percR = GameObject.Find ("PercentageR").GetComponent<Slider> ();
		Image tubeL = GameObject.Find ("TubeFillerL").GetComponent<Image> ();
		Slider percL = GameObject.Find ("PercentageL").GetComponent<Slider> ();
		percR.value = tubeR.fillAmount;
		percR.GetComponentInChildren<Text> ().text = "<- " + (Math.Round(percR.value * 100, 2)) + "%";
		percL.value = tubeL.fillAmount;
		percL.GetComponentInChildren<Text> ().text = "<- " + (Math.Round(percL.value * 100, 2)) + "%";
	}
}
