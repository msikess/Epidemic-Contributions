using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filler : MonoBehaviour {


	//params needed for level fills;
	//FaceScripts represent the filling scripts for test tube A, B and the funds bar.
	private FaceScript a;
	private FaceScript b;
	private FaceScript fnd;
	private FaceScript fnd2;
	//Setting the maximum values for the test tubes ans funds bar.
	private float maxValA;
	private float maxValB;
	private float maxValFunds;


	/*			functions related to setting and updating the cured bars			*/

	//sets the current values of the fillers;
	void curValsSetter (FaceScript one, FaceScript two, FaceScript three, float av, float bv, float cv)
	{
		one.Value = av;
		two.Value = bv;
		three.Value = cv;
		three.Val = cv;
	}

	//needed for faceScript which calculates the relative ratios of
	//	cur. vals depending on the max. vals;
	void maxValSetter (FaceScript one, FaceScript two, float av, float bv)
	{
		one.MaxValue = av;
		two.MaxValue = bv;
	}

	//To set up all the objects and the maxVals if the ydon't vary later;
	public void dayOne (GameBuilder1 world, Initializer init){
		a = GameObject.Find ("TubeL").GetComponent<FaceScript> ();
		b = GameObject.Find ("TubeR").GetComponent<FaceScript> ();
		fnd = GameObject.Find ("FundsBar").GetComponent<FaceScript> ();
		maxValA = (float)world.retDay(1).get("totalTrtA");
		maxValB = (float)world.retDay (1).get ("totalTrtB");
		maxValFunds = (float)init.getParam("fund");
		maxValSetter (a, b, maxValA, maxValB);
		fnd.MaxValue = maxValFunds;
		curValsSetter (a, b, fnd,
			(float)world.retDay (1).get ("totalA"), 
			(float)world.retDay (1).get ("totalB"),
			(float)maxValFunds);
	}

	//for every consequent daily update;
	public void nextDay (GameBuilder1 world, int i){
		maxValA = (float)world.retDay(i).get("totalTrtA");
		maxValB = (float)world.retDay (i).get ("totalTrtB");
		maxValSetter (a, b, maxValA, maxValB);
		curValsSetter (a, b, fnd,
			(float)world.retDay (i).get ("totalA"), 
			(float)world.retDay (i).get ("totalB"),
			(float)maxValFunds - world.retDay (i).get ("totalCost"));
	}

	//Update function is called every frame to update the values of fnd, the funds bar.
	void Update(){
		//The inital case, where fnd is at its max value or total number of days is one.
		if (fnd.Value == 1f || GameObject.Find("WorldModel").GetComponent<WorldModel> ().worldRet ().totalDays() == 1) {
			fnd.Value = (float)maxValFunds 
						- (GameObject.Find ("WorldModel").GetComponent<SetText> ().EstCostA 
						+ GameObject.Find ("WorldModel").GetComponent<SetText> ().EstCostB 
						+ GameObject.Find ("WorldModel").GetComponent<SetText> ().SickTrt);
		} else {
		//Otherwise, compute the total remaining funds as follows:
			fnd.Value = (float)maxValFunds 
						- GameObject.Find("WorldModel").GetComponent<WorldModel> ().worldRet ().retDay (2).get ("totalCost") 
						- (GameObject.Find ("WorldModel").GetComponent<SetText> ().EstCostA 
						+ GameObject.Find ("WorldModel").GetComponent<SetText> ().EstCostB 
						+ GameObject.Find ("WorldModel").GetComponent<SetText> ().SickTrt);
		}
	}

}
