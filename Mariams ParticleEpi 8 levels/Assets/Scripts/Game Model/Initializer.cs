using System.Collections;
using UnityEngine;
using System;

public class Initializer : MonoBehaviour {

	//This is the file for setting variables at the start of the game;
	//all the fields show up in inspector so no need to set the vals here;
	[SerializeField]
	private int population;
	[SerializeField]
	private int initSick;
	[SerializeField]
	private double effectiveA;
	[SerializeField]
	private double effectiveB;
	[SerializeField]
	private double spreadRate;
	[SerializeField]
	private double percentWithSymp;
	private double percentWithTrtA;
	private double percentWithTrtB;
    [SerializeField]
    private double recoveryRate;
	[SerializeField]
	private int costA;
	[SerializeField]
	private int costB;
	[SerializeField]
	private int variance;

	//The following variables are needed for stop rules;
	[SerializeField]
	private int dayLimit;
	[SerializeField]
	private int fundLimit;
	[SerializeField]
	private int sickCost;
	[SerializeField]
	private int goalPeople;

	//for CSCV file path
	[SerializeField]
	private string filePath;
		
	//getter for the params since the instances are private;
	//everything is returned as adouble, the ints are re-declared as ints wherever needed in other code files;
	public double getParam(string param){
		percentWithTrtA = GameObject.Find ("BodyL").GetComponent<Syringe> ().percentage;
		percentWithTrtB = GameObject.Find ("BodyR").GetComponent<Syringe> ().percentage;
		if (percentWithTrtA + percentWithTrtB > 1 || percentWithTrtA + percentWithTrtB < 0) {
			if (Math.Max (percentWithTrtA, percentWithTrtB) == percentWithTrtA) {
				percentWithTrtB = Math.Abs (percentWithTrtB);
				percentWithTrtA = 1 - percentWithTrtB;
			} else {
				percentWithTrtA = Math.Abs (percentWithTrtA);
				percentWithTrtB = 1 - percentWithTrtA;
			}
		}
		if (param.Equals ("population", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)population;
		} else if (param.Equals ("costA", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)costA;
		} else if (param.Equals ("costB", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)costB;
		} else if (param.Equals ("effectiveA", System.StringComparison.InvariantCultureIgnoreCase)) {
			return effectiveA;
		} else if (param.Equals ("effectiveB", System.StringComparison.InvariantCultureIgnoreCase)) {
			return effectiveB;
		} else if (param.Equals ("spreadRate", System.StringComparison.InvariantCultureIgnoreCase)) {
			return spreadRate;
		} else if (param.Equals ("percentWithSymp", System.StringComparison.InvariantCultureIgnoreCase)) {
			return percentWithSymp;
		} else if (param.Equals ("percentWithTrtA", System.StringComparison.InvariantCultureIgnoreCase)) {
			return percentWithTrtA;
		} else if (param.Equals ("percentWithTrtB", System.StringComparison.InvariantCultureIgnoreCase)) {
			return percentWithTrtB;
		} else if (param.Equals ("fund", System.StringComparison.InvariantCultureIgnoreCase)) {
			return fundLimit;
		} else if (param.Equals ("dayLimit", System.StringComparison.InvariantCultureIgnoreCase)) {
			return dayLimit;
		} else if (param.Equals ("sickCost", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)sickCost;
		} else if (param.Equals ("initSick", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)initSick;
		} else if (param.Equals ("variance", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)variance;
		} else if (param.Equals ("goalPeople", System.StringComparison.InvariantCultureIgnoreCase)) {
			return (double)goalPeople;
        } else if (param.Equals("recovery", System.StringComparison.InvariantCultureIgnoreCase)){
            return recoveryRate;
        } else {
			throw new Exception ();
		}
	}

	public String fileP(){
		return filePath;
	}

}
