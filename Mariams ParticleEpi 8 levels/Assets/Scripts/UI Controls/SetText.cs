using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//this script sets the text for all the text boxes on the game screen
public class SetText : MonoBehaviour {

	//references to onscreen text boxes
	private Text percentTrtA;
	private Text percentTrtB;
	private Text totalCuredA;
	private Text totalCuredB;
	private Text totalTrtA;
	private Text totalTrtB;
	private Text dayNum;
	private Text funds;
	private Text costA;
	private Text costB;
	private Text ai;
	private Text costAEst;
	private Text costBEst;
	private Text perCost;

	//reference to funds animator controller, this should really be in WorldModel
	private Animator fundsAnimation;

	//reference to Syringe script for both sliders
	private Syringe sliderA;
	private Syringe sliderB;

	//reference to Initializer, needed for some static text
	private Initializer init;


	//variables needed for cost estimate filler;
	private int estCostB;
	private int estCostA;
	private int sickTrt;


	//getters for variables needed for cost estimate filler
	public int EstCostA {
		get {
			return estCostA;
		}
	}
		
	public int EstCostB {
		get {
			return estCostB;
		}
	}

	public int SickTrt {
		get {
			return sickTrt;
		}
	}
		


	void Start(){
		//set all the references to the correct game object
		init = GameObject.Find ("WorldModel").GetComponent<Initializer> ();
		sliderA = GameObject.Find ("BodyL").GetComponent<Syringe> ();
		sliderB = GameObject.Find ("BodyR").GetComponent<Syringe> ();
		percentTrtA = GameObject.Find ("PercentTrtA").GetComponent<Text> ();
		percentTrtB = GameObject.Find ("PercentTrtB").GetComponent<Text> ();
		totalCuredA = GameObject.Find ("TotalCuredA").GetComponent<Text> ();
		totalCuredB = GameObject.Find ("TotalCuredB").GetComponent<Text> ();
		totalTrtA = GameObject.Find ("TotalTrtA").GetComponent<Text> ();
		totalTrtB = GameObject.Find ("TotalTrtB").GetComponent<Text> ();
		dayNum = GameObject.Find ("DayNum").GetComponent<Text> ();
		funds = GameObject.Find ("Funds").GetComponent<Text> ();
		costA = GameObject.Find ("CostA").GetComponent<Text> ();
		costB = GameObject.Find ("CostB").GetComponent<Text> ();
		ai = GameObject.Find ("GameAI").GetComponentInChildren<Text> ();
		costAEst = GameObject.Find ("CostAEst").GetComponent<Text> ();
		costBEst = GameObject.Find ("CostBEst").GetComponent<Text> ();
		perCost = GameObject.Find ("PatientCost").GetComponent<Text> ();
		fundsAnimation = GameObject.Find ("ImageFunds").GetComponent<Animator> ();

		//set static text
		perCost.text = "Cost per Patient: " + init.getParam ("sickCost") + "$";
		costA.text = "" + init.getParam ("costA") + "$";
		costB.text = "" + init.getParam ("costB") + "$";

		//set inital text for the rest
		funds.text = "Funds Left: " + init.getParam ("fund")  + "$";
		totalCuredA.text = "Total Cured: " + 0;
		totalCuredB.text = "Total Cured: " + 0;
		totalTrtA.text = "Total Treatment Given: " + 0;
		totalTrtB.text = "Total Treatment Given: " + 0;
		dayNum.text = "Day: 1/" + init.getParam("dayLimit"); 

		//set initial value for animation parameter
		fundsAnimation.SetFloat ("fund_remaining", (float) init.getParam("fund"));

	}


	void Update() {
		//dynamic text updates happen here, i.e. text associated with the sliders
		percentTrtA.text = (Math.Round ((sliderA.percentage * 100), 1, MidpointRounding.AwayFromZero)).ToString () + "%";
		percentTrtB.text = (Math.Round ((sliderB.percentage * 100), 1, MidpointRounding.AwayFromZero)).ToString () + "%";
		costAEst.text = "Estimated Cost: " + (((int) (Math.Round((GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") * ((double.Parse (percentTrtA.text.Remove(percentTrtA.text.Length - 1))) / 100)), MidpointRounding.AwayFromZero) * GameObject.Find ("WorldModel").GetComponent<Initializer> ().getParam("costA")))) + "$";
		costBEst.text = "Estimated Cost: " + (((int) (Math.Min(Math.Round(GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") * ((double.Parse (percentTrtB.text.Remove(percentTrtB.text.Length - 1))) / 100), MidpointRounding.AwayFromZero), GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") - Math.Round((GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") * ((double.Parse (percentTrtA.text.Remove(percentTrtA.text.Length - 1))) / 100)), MidpointRounding.AwayFromZero)) * GameObject.Find ("WorldModel").GetComponent<Initializer> ().getParam("costB")))) + "$";

		//update global variables needed for cost estimate filler
		estCostA = (int) (Math.Round((GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") * ((double.Parse (percentTrtA.text.Remove(percentTrtA.text.Length - 1))) / 100)), MidpointRounding.AwayFromZero) * GameObject.Find ("WorldModel").GetComponent<Initializer> ().getParam("costA"));
		estCostB = (int) (Math.Min (Math.Round (GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") * ((double.Parse (percentTrtB.text.Remove (percentTrtB.text.Length - 1))) / 100), MidpointRounding.AwayFromZero), GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") - Math.Round ((GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSymps") * ((double.Parse (percentTrtA.text.Remove (percentTrtA.text.Length - 1))) / 100)), MidpointRounding.AwayFromZero)) * GameObject.Find ("WorldModel").GetComponent<Initializer> ().getParam ("costB"));
		sickTrt = (int) (GameObject.Find ("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (1).get ("totalSympTrt")  * (int) GameObject.Find ("WorldModel").GetComponent<Initializer> ().getParam ("sickCost"));
		}

	// Updates text associated with test tube, funds remaining, and news panel after you cure some people
	public void updateText(GameBuilder1 world, int i) {

			totalCuredA.text = "Total Cured: " + world.retDay (i).get ("totalA");
			totalCuredB.text = "Total Cured: " + world.retDay (i).get ("totalB");
			totalTrtA.text = "Total Treatment Given: " + world.retDay (i).get ("totalTrtA");
			totalTrtB.text = "Total Treatment Given: " + world.retDay (i).get ("totalTrtB");
			funds.text = "Funds Left: " + (init.getParam ("fund") - world.retDay (i).get ("totalCost")) + "$";
			fundsAnimation.SetFloat ("fund_remaining", (float) ((init.getParam("fund") - world.retDay (i).get ("totalCost"))/init.getParam ("fund")));
			ai.text = "You cured " + world.retDay (1).get ("totalCuredDay") + " people";

	}

	//updates news panel and day number after some more people catch the disease, called in WorldModel
	public void updateDay(GameBuilder1 world, int i) {
		dayNum.text = "Day: " + world.retDay (i).get ("day") + "/" + init.getParam("dayLimit");
		ai.text = Math.Abs(world.retDay (1).get ("totalSymps") - (world.retDay(2).get("totalSymps") - world.retDay(2).get("totalCuredDay"))) + " more people caught the disease today";
	}

	//updates the news panel with number of patients at the beginning of the next day
	public void sickUpdate(GameBuilder1 world){
		ai.text = "Number of Patients: " + world.retDay (1).get ("totalSymps") + ".";
	}

}
