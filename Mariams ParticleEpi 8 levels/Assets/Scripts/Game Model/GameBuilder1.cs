using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class GameBuilder1 {
	
	//List of all the days the player has played through;
	[SerializeField]
	private List<Day> calendar;
	private Randomize rander;


	//GameBuilder constructor
	public GameBuilder1(Initializer init, SpawnM1 spawner, SpawnPopulation spawner2){
		this.calendar = new List<Day> ();
		this.calendar.Add (initDay (init));
		spawner.onStart (init, this);
		spawner2.onStart (init, this);
		rander = new Randomize((int)init.getParam("variance"));
	}

	//for retreiving a specific day from the calendar;
	//needed for adding more days and checking things (see below) or for .csv additions (see WorldModel);
	public Day retDay(int i){
		return this.calendar[calendar.Count - i];
	}

	//needed to .csv additions (see WorldModel);
	public int totalDays(){
		return this.calendar.Count;
	}

	//This is for adding the first day to calendar;
	//needs to be separate as calculations for some of the Day params are different than for further Days;
	public Day initDay(Initializer init){
		
		if ((init.getParam("percentWithTrtA") + init.getParam("percentWithTrtB")) > 1 || (init.getParam("percentWithTrtA") + init.getParam("percentWithTrtB")) < 0 
			|| init.getParam("costA") < 0 || init.getParam("costB") < 0 || init.getParam("population") < 100 || init.getParam("effectiveA") < 0 || init.getParam("effectiveB") < 0
			|| init.getParam("spreadRate") < 0 || init.getParam("percentWithSymp") < 0 || init.getParam("percentWithTrtA") < 0 || init.getParam("percentWithTrtB") < 0
			|| init.getParam("spreadRate") > (32 / init.getParam("population")) || init.getParam("dayLimit") <= 0) {
			throw new IOException ();
		}
		//set up variables;
		int day = 1;
		int sickNum = (int) init.getParam("initSick");
		int healthyNum = (int) init.getParam("population") - sickNum;
		int healthyWithSympNum = (int) (Math.Round((healthyNum * init.getParam("percentWithSymp")), MidpointRounding.AwayFromZero));
		int totalSymps = sickNum + healthyWithSympNum;
		int totalSympTrt = totalSymps;
		int totalSickTrt = sickNum;
		int check = (int) init.getParam("population");
		return new Day (day, healthyNum, sickNum, healthyWithSympNum, totalSymps, totalSympTrt, totalSickTrt,  check);
	}

	//for calculating the treatment assignment consequences;
	public void calculate(Initializer init){
		Day today = retDay (1);
		int sympTrtA = (int)(Math.Round ((today.get ("totalSympTrt") * init.getParam ("percentWithTrtA")), MidpointRounding.AwayFromZero));
		int sympTrtB = Math.Min ((int)(Math.Round ((today.get ("totalSympTrt") * init.getParam ("percentWithTrtB")), MidpointRounding.AwayFromZero)), today.get ("totalSympTrt") - sympTrtA);
		int sickTrtA = (int)(Math.Round ((today.get ("totalSickTrt") * init.getParam ("percentWithTrtA")), MidpointRounding.AwayFromZero));
		int sickTrtB = Math.Min ((int)(Math.Round ((today.get ("totalSickTrt") * init.getParam ("percentWithTrtB")), MidpointRounding.AwayFromZero)), today.get ("totalSickTrt") - sickTrtA);
		int curedANum = rander.curedPopulationRnd (sickTrtA, init.getParam ("effectiveA"));//*//(int)(Math.Round ((init.getParam ("effectiveA") * sickTrtA), MidpointRounding.AwayFromZero));
		int curedBNum = rander.curedPopulationRnd (sickTrtB, init.getParam ("effectiveB"));//*//(int)(Math.Round ((init.getParam ("effectiveB") * sickTrtB), MidpointRounding.AwayFromZero));
        int recovered = (int) Math.Floor((init.getParam("recovery") * (today.get("sickNum") - curedANum - curedBNum)));
		int totalCuredDay = curedANum + curedBNum + recovered;
		int totalCuredTotal;
		if (calendar.Count == 1) {
			totalCuredTotal = totalCuredDay;
		} else {
			totalCuredTotal = retDay (2).get ("totalcuredtotal") + totalCuredDay;
		}
		int catchDisease = Math.Min ((int)(Math.Round (init.getParam ("spreadRate") * today.get ("sickNum") * today.get ("healthyNum"), MidpointRounding.AwayFromZero)), today.get ("healthyNum"));
		int costNum = (sympTrtA * (int)init.getParam ("costA")) + (sympTrtB * (int)init.getParam ("costB")) + ((int) init.getParam("sickCost") * today.get ("sickNum"));
		int costA = (sympTrtA * (int)init.getParam ("costA"));
		int costB = (sympTrtB * (int)init.getParam ("costB"));
		int totalCost;
		int totalA;
		int totalB;
		int totalTrtA;
		int totalTrtB;
		if (calendar.Count == 1) {
			totalCost = costNum;
			totalA = curedANum;
			totalB = curedBNum;
			totalTrtA = sympTrtA;
			totalTrtB = sympTrtB;
		} else {
			totalCost = retDay (2).get ("totalcost") + costNum;
			totalA = retDay (2).get ("totala") + curedANum;
			totalB = retDay (2).get ("totalb") + curedBNum;
			totalTrtA = retDay (2).get ("totalTrtA") + sympTrtA;
			totalTrtB = retDay (2).get ("totalTrtB") + sympTrtB;
		}
		int budget = (int) init.getParam ("fund") - totalCost;
		today.restOfDay (sympTrtA, sympTrtB, sickTrtA, sickTrtB, curedANum, curedBNum, recovered, totalCuredDay, totalCuredTotal, catchDisease, costNum, totalCost, totalA, totalB, totalTrtA, totalTrtB, costA, costB, budget);
	}

	//For creating the daily stats of teh next day;
	public void nextDay(Initializer init){
		Day prev = retDay (1);
		int day = calendar.Count + 1;
		int sickNum = prev.get("sickNum") + prev.get("catchDisease") - prev.get("curedANum") - prev.get("curedBNum");
		int totalSickTrt = sickNum;
		int healthyNum = (int) init.getParam("population") - sickNum - prev.get("totalCuredTotal");
		int healthyWithSympNum = (int) (Math.Round((healthyNum * init.getParam("percentWithSymp")), MidpointRounding.AwayFromZero));
		int totalSymps = sickNum + healthyWithSympNum;
		int totalSympTrt = totalSymps;
		int check = prev.get ("check");
		calendar.Add(new Day (day, healthyNum, sickNum, healthyWithSympNum, totalSymps, totalSympTrt, totalSickTrt, check));
	}

}
