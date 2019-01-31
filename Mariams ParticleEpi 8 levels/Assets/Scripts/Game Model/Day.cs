using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Day {

	//manipulated vars/vars that are calculated and are not user-manipulated
	[SerializeField]
	private int day;
	//[SerializeField]
	private int healthyNum;
	//[SerializeField]
	private int healthyWithSympNum;
	//[SerializeField]
	private int sickNum;
	//[SerializeField]
	private int totalSymps;
	[SerializeField]
	private int totalSympTrt;
	[SerializeField]
	private int sympTrtA;
	[SerializeField]
	private int sympTrtB;
	//[SerializeField]
	private int totalSickTrt;
	//[SerializeField]
	private int sickTrtA;
	//[SerializeField]
	private int sickTrtB;
	[SerializeField]
	private int curedANum;
	[SerializeField]
	private int curedBNum;
    [SerializeField]
    private int recovered;
	//[SerializeField]
	private int totalCuredDay;
	//[SerializeField]
	private int totalCuredTotal;
	//[SerializeField]
	private int catchDisease;
	[SerializeField]
	private int costNum;
	//[SerializeField]
	private int check;

	//Following variables are not a part of the model, they are created 
	//	for the sake of necessity of calculations in the game;

	//not a variable that's documented, it's only used to track win/lose state in terms of 
	//	the budget that the player is not supposed to exceed;
	//[SerializeField]
	private int totalCost;
	[SerializeField]
	private int costA;
	[SerializeField]
	private int costB;
	[SerializeField]
	private int budget;
	//the next four are needed for cured bar calculations (also not documented);;
	[SerializeField]
	private int totalA;
	[SerializeField]
	private int totalB;
	[SerializeField]
	private int totalTrtA;
	[SerializeField]
	private int totalTrtB;

	//Day constructor
	public Day (int day, int healthyNum, int sickNum, int healthyWithSympNum, int totalSymps, int totalSympTrt, int totalSickTrt, 
		int check){

		this.day = day;
		this.healthyNum = healthyNum;
		this.sickNum = sickNum;
		this.healthyWithSympNum = healthyWithSympNum;
		this.totalSymps = totalSymps;
		this.totalSympTrt = totalSympTrt;
		this.totalSickTrt = totalSickTrt;
		this.check = check;

	}

	public void restOfDay (int sympTrtA, int sympTrtB, int sickTrtA, int sickTrtB, int curedANum, int curedBNum, int rec, int totalCuredDay, 
		int totalCuredTotal, int catchDisease, int costNum, int totalCost, int totalA, int totalB, int totalTrtA, int totalTrtB, int costA, int costB, int budget){
		this.sympTrtA = sympTrtA;
		this.sympTrtB = sympTrtB;
		this.sickTrtA = sickTrtA;
		this.sickTrtB = sickTrtB;
		this.curedANum = curedANum;
		this.curedBNum = curedBNum;
		this.recovered = rec;
		this.totalCuredDay = totalCuredDay;
		this.totalCuredTotal = totalCuredTotal;
		this.catchDisease = catchDisease;
		this.costNum = costNum;
		this.totalCost = totalCost;
		this.totalA = totalA;
		this.totalB = totalB;
		this.totalTrtA = totalTrtA;
		this.totalTrtB = totalTrtB;
		this.costA = costA;
		this.costB = costB;
		this.budget = budget;
	}

	//getter for the instances of a Day since the class is private;
	public int get(string param){
        if (param.Equals("day", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return day;
        }
        else if (param.Equals("healthyNum", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return healthyNum;
        }
        else if (param.Equals("sickNum", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return sickNum;
        }
        else if (param.Equals("healthyWithSympNum", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return healthyWithSympNum;
        }
        else if (param.Equals("totalsymps", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalSymps;
        }
        else if (param.Equals("totalsymptrt", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalSympTrt;
        }
        else if (param.Equals("symptrta", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return sympTrtA;
        }
        else if (param.Equals("symptrtb", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return sympTrtB;
        }
        else if (param.Equals("totalsicktrt", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalSickTrt;
        }
        else if (param.Equals("sicktrta", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return sickTrtA;
        }
        else if (param.Equals("sicktrtb", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return sickTrtB;
        }
        else if (param.Equals("curedAnum", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return curedANum;
        }
        else if (param.Equals("curedbnum", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return curedBNum;
        }
        else if (param.Equals("totalcuredday", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalCuredDay;
        }
        else if (param.Equals("totalcuredtotal", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalCuredTotal;
        }
        else if (param.Equals("catchdisease", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return catchDisease;
        }
        else if (param.Equals("costNum", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return costNum;
        }
        else if (param.Equals("check", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return check;
        }
        else if (param.Equals("totalcost", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalCost;
        }
        else if (param.Equals("totala", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalA;
        }
        else if (param.Equals("totalb", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalB;
        }
        else if (param.Equals("totalTrtA", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalTrtA;
        }
        else if (param.Equals("totalTrtB", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return totalTrtB;
        }
        else if (param.Equals("costA", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return costA;
        }
        else if (param.Equals("costB", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return costB;
        }
        else if (param.Equals("budget", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return budget;

        }
        else if (param.Equals("recovered", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return recovered;
        }
        else
        {
            throw new UnityException();
        }
	}

}
