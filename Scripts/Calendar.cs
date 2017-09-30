using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

// Note that the class along with its List<Day> insatnce had to be
//serialized in order to store each day's data correctly in a Json file;

[Serializable]
public class Calendar {
	
	//List of all the days the player has played through;
	[SerializeField]
	private List<Day> calendar;
	private Randomize rander;


	//GameBuilder constructor
	public Calendar(Initializer init, UIScriptOne one, UIScriptTwo two){
		this.calendar = new List<Day> ();
		this.calendar.Add (initDay (init));
		one.onStart (init, this);
		two.onStart (init, this);
		rander = new Randomize((int)init.getParam("v12")); //While I can't share the Randomize script,
																//I can say that it's the one where I contributed
																//to make reading CSV files in WebGL Build possible;
	}

	//For retreiving a specific day from the calendar;
	//Also needed for adding more days and checking things (see below);
	public Day retDay(int i){
		return this.calendar[calendar.Count - i];
	}

	public int totalDays(){
		return this.calendar.Count();
	}

// The next three methods are for doing the calculations in order to instantiate all the
//variables in each day object. However, you'll notice that initDay and nextDay are using 
//pretty similar code. That is because, despite the overall similarity, there are certain
//variables that are calculated differently on a First day of each level than on the consecutive
//days of that same level .

	//This is for adding the first day to calendar;
	//needs to be separate as calculations for some of the Day params are different than for further Days;
	public Day initDay(Initializer init){
		
		if ((parameters in Initializer are not set up in accordance to the model's limitations) {
			throw new Exception ();
		}
		else 
		{
		//set up variables;
		int p1 = 1;
		int p2 = (int) init.getParam("v2");
		int p3 = (int) init.getParam("v1") - p2;
		int p4 = (int) (Math.Round((p3 * init.getParam("v4")), MidpointRounding.AwayFromZero));
		int p5 = p2 + p4;
		int p6 = p5;
		int p7 = p2;
		int p19 = (int) init.getParam("v1");
		return new Day (p1, p2, p3, p4, p5, p6, p7, p19);
		}
	}

	//for calculating the treatment assignment consequences;
	public void calculate(Initializer init){
		Day today = retDay (1);
		int p8 = (int)(Math.Round ((today.get ("p6") * init.getParam ("v7")), MidpointRounding.AwayFromZero));
		int p9 = Math.Min ((int)(Math.Round ((today.get ("p6") * init.getParam ("v8")), MidpointRounding.AwayFromZero)), today.get ("p6") - p8);
		int p10 = (int)(Math.Round ((today.get ("p7") * init.getParam ("v7")), MidpointRounding.AwayFromZero));
		int p11 = Math.Min ((int)(Math.Round ((today.get ("p7") * init.getParam ("v8")), MidpointRounding.AwayFromZero)), today.get ("p7") - p10);
		int p12 = rander.curedPopulationRnd (p10, init.getParam ("v5"));
		int p13 = rander.curedPopulationRnd (p11, init.getParam ("v6"));
        int p14 = (int) Math.Floor((init.getParam("v9") * (today.get("p2") - p12 - p13)));
		int p15 = p12 + p13 + p14;
		int p16;
		if (calendar.Count == 1) {
			p16 = p15;
		} else {
			p16 = retDay (2).get ("p16") + p15;
		}
		int p17 = Math.Min ((int)(Math.Round (init.getParam ("v4") * today.get ("p2") * today.get ("p3"), MidpointRounding.AwayFromZero)), today.get ("p3"));
		int p18 = (p8 * (int)init.getParam ("v10")) + (p9 * (int)init.getParam ("v11")) + ((int) init.getParam("v15") * today.get ("p5"));
		int p20 = (p8 * (int)init.getParam ("v10"));
		int p21 = (p9 * (int)init.getParam ("v11"));
		int p22;
		int p23;
		int p24;
		int p25;
		int p26;
		if (calendar.Count == 1) {
			p22 = p18;
			p23 = p12;
			p24 = p13;
			p25 = p8;
			p26 = p9;
		} else {
			p22 = retDay (2).get ("p22") + p18;
			P23 = retDay (2).get ("p23") + p12;
			p24 = retDay (2).get ("p24") + p13;
			p25 = retDay (2).get ("p25") + p8;
			p26 = retDay (2).get ("p26") + p9;
		}
		int p27 = (int) init.getParam ("v16") - p22;
		today.restOfDay (p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p20, p21, p22, p23, p24, p25, p26, p27);
	}

	//For creating the daily stats of teh next day;
	public void nextDay(Initializer init){
		Day prev = retDay (1);
		int p1 = calendar.Count + 1;
		int p2 = prev.get("02") + prev.get("p16") - prev.get("p12") - prev.get("p13");
		int p7 = p2;
		int p3 = (int) init.getParam("v1") - p2 - prev.get("p15");
		int p4 = (int) (Math.Round((p3 * init.getParam("v3")), MidpointRounding.AwayFromZero));
		int p5 = p2 + p4;
		int p6 = p5;
		int p19 = prev.get ("p19");
		calendar.Add(new Day (p1, p2, p3, p4, p5, p6, p7, p19));
	}

}
