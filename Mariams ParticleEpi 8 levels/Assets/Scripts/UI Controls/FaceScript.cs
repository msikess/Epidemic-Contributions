using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceScript : MonoBehaviour {

	//the float percentage of the fill;
	private float fillAmt;
	private float fillAmt2;

	//since the code is attached tot he whole object, the img here is the filler part;
	[SerializeField]
	private Image img;
	[SerializeField]
	private Image img2;

	//setting the max value so we know what corresponds to 100% or 1 on the filler slider;
	// * no need for a min value since it's 0;
	public float MaxValue { get; set; }

	//sets the current level of the fill to (technically) the value/max.
	public float Value { 
		get{
			return fillAmt;
		}
		set{
			fillAmt = mapper (value, MaxValue);
		}
	}

	public float Val {
		get{
			return fillAmt2;
		}
		set{
			fillAmt2 = mapper (value, MaxValue);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//faceHandler ();
		if (fillAmt != img.fillAmount) {
			img.fillAmount = Mathf.Lerp (img.fillAmount, fillAmt, 4 * Time.deltaTime);
		}
		if (img2 != null && fillAmt2 != img2.fillAmount) {
			img2.fillAmount = Mathf.Lerp (img2.fillAmount, fillAmt2, 4 * Time.deltaTime);
		}
	}

	//takes in the cur val and max val and returns the percentage we're going to display;
	private float mapper(float val, float inMax){
		if (inMax == 0) {
			return 0;
		} else {
			return (val / inMax);
		}
	}
}
