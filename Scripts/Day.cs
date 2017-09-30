using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*

Serialization in this script is used in order to serialize data into
Json file which is later submitted to the DB server. Therefore, whichever
variable is serialized will show up in the file, but the rest will not.

*/

[Serializable]
public class Day {

	//vars/vars that are calculated and are accessible to developers only;
	[SerializeField]
	private int p1;
	private int p2;
	private int p3;
	private int p4;
	private int p5;
	[SerializeField]
	private int p6;
	[SerializeField]
	private int p7;
	[SerializeField]
	private int p8;
	private int p9;
	private int p10;
	private int p11;
	[SerializeField]
	private int p12;
	[SerializeField]
	private int p13;
    [SerializeField]
    private int p14;
	private int p15;
	private int p16;
	private int p17;
	[SerializeField]
	private int p18;
	private int p19;

	/*

	Following variables are not a part of the model, they are created 
	for the sake of necessity of calculations in the game;

	*/

	//The next four variables are only used to track win/lose states;
	private int p20;
	[SerializeField]
	private int p21;
	[SerializeField]
	private int p22;
	[SerializeField]
	private int p23;
	//The next four are needed for Cure UI calculations;
	[SerializeField]
	private int p24;
	[SerializeField]
	private int p25;
	[SerializeField]
	private int p26;
	[SerializeField]
	private int p27;

	//Day constructor;
	//Initializes the variables that need to be set first according to the model.
	public Day (int p1, int p2, int p3, int p4, int p5, int p6, int p9, int p19)
	{

		this.p1 = p1;
		this.p2 = p2;
		this.p3 = p3;
		this.p4 = p4;
		this.p5 = p5;
		this.p6 = p6;
		this.p9 = p9;
		this.p19 = p19;

	}

	/* 

	Because of the way some variables are calculated in the model, I had 
	to split the constructor's function into two by making the constructor
	and a method that does the rest of the initializations. This is that method.

	Purpose: To initialize the rest of the variables that the constructor doesn't
	initialize;
	Input: integers;
	Output: none;

	*/

	public void restOfDay (int p7, int p8, int p10, int p11, int p12, int p13,
						   int p14, int p15, int p16, int p17, int p18, int p20,
						   int p21, int p22, int p23, int p24, int p25, int p26, 
						   int p27)
	{

		this.p7 = p7;
		this.p8 = p8;
		this.p10 = p10;
		this.p11 = p11;
		this.p12 = p12;
		this.p13 = p13;
		this.p14 = p14;
		this.p15 = p15;
		this.p16 = p16;
		this.p17 = p17;
		this.p18 = p18;
		this.p20 = p20;
		this.p21 = p21;
		this.p22 = p22;
		this.p23 = p23;
		this.p24 = p24;
		this.p25 = p25;
		this.p26 = p26;
		this.p27 = p27;
	}

	/*

	Note that if the variables had actually had these kind of simple
	names, there would be an easier and more efficient way of writing
	this getter. However, since we wanted to stay relevant to the 
	original model, we kept the variable names from the model and those 
	names were more complex than these here.

	Purpose: getter for the instances of a Day since the class is private;
	Input: string (has to have exact spelling as the variable name);
	Output: integer;

	*/  

	public int get(string param)
	{

        if (param.Equals("p1", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p1;
        }
        else if (param.Equals("p2", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p2;
        }
        else if (param.Equals("p3", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p3;
        }
        else if (param.Equals("p4", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p4;
        }
        else if (param.Equals("p5", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p5;
        }
        else if (param.Equals("p6", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p6;
        }
        else if (param.Equals("p7", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p7;
        }
        else if (param.Equals("p8", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p8;
        }
        else if (param.Equals("p9", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p9;
        }
        else if (param.Equals("p10", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p10;
        }
        else if (param.Equals("p11", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p11;
        }
        else if (param.Equals("p12", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p12;
        }
        else if (param.Equals("p13", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p13;
        }
        else if (param.Equals("p14", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p14;
        }
        else if (param.Equals("p15", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p15;
        }
        else if (param.Equals("p16", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p16;
        }
        else if (param.Equals("p17", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p17;
        }
        else if (param.Equals("p18", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p18;
        }
        else if (param.Equals("p19", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p19;
        }
        else if (param.Equals("p20", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p20;
        }
        else if (param.Equals("p21", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p21;
        }
        else if (param.Equals("p22", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p22;
        }
        else if (param.Equals("p23", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p23;
        }
        else if (param.Equals("p24", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p24;
        }
        else if (param.Equals("p25", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p25;
        }
        else if (param.Equals("p26", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p26;

        }
        else if (param.Equals("p27", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return p27;
        }
        else
        {
            throw new Exception();
        }
        
	}

}
