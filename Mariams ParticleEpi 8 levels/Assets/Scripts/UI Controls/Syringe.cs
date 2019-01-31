using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Syringe : MonoBehaviour {

	public int id;
	public float percentage;
	float x_pos, y_pos;
	float length;
	GameObject sliderA;
	GameObject sliderB;

	// Use this for initialization
	void Start () {
		length = GameObject.Find ("BodyMask").GetComponent<RectTransform> ().rect.width;
		x_pos = GetComponent<RectTransform>().anchoredPosition.x;
		y_pos = GetComponent <RectTransform> ().anchoredPosition.y;
		sliderA = GameObject.Find ("BodyL");
		sliderB = GameObject.Find ("BodyR");
	}
	
	// Update is called once per frame
	// This function ensures that the two syringes move according to the values of slider A and B respectively.
	void Update () {
		// This ensures that the two syringes do not overlap with each other while the player is dragging them around.
		if (sliderA.GetComponent <Syringe> ().percentage + sliderB.GetComponent <Syringe> ().percentage <= 1) {
			//check that the percentage (from the slider value) is valid (between 0 and 1). 
			if (percentage >= 0 && percentage <= 1) {
				//for left syringe (A), we set the position of it according to slider percentage
				if (id == 1) {
					GetComponent<RectTransform> ().anchoredPosition = new Vector3 (percentage * length, y_pos, 0);

				//for right syringe (B), we set the position of it according to one minus slider percentage, since anchored position is measured from left.
				} else if (id == 2) {
					GetComponent <RectTransform> ().anchoredPosition = new Vector3 ((1 - percentage) * length, y_pos, 0);
				
				//id can only be set to either 1 or 2.
				} else {
					Debug.Log ("Invalid id numbers");
				}
			}
		}
	}


	// setPercent method sets the slider value by discrete intervals of increments.
	public void setPercent(float percent) {
			// Here we implemented the interval as 5%.
			percent /= 20;
			if (id == 1) {
				float other = GameObject.Find ("BodyR").GetComponent <Syringe> ().percentage;
				// This ensures that the other slider does not have a percentage such that sum is over one.
				if (other + percent > 1) {
					GameObject.Find ("Slider_left").GetComponent<Slider> ().value = (1 - other) * 20; 
					return;
				}
			} else {
				float other = GameObject.Find ("BodyL").GetComponent <Syringe> ().percentage;
				// This ensures that the other slider does not have a percentage such that sum is over one.
				if (other + percent > 1) {
					GameObject.Find ("Slider_right").GetComponent<Slider> ().value = (1 - other) * 20;
					return;
				}
			}
		//FINALLY, we set the percentage value according to the percent parameter, which has discrete .05 intervals.
		percentage = percent;
	}
		
}
