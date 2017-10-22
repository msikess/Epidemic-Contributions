using System.Collections;
using UnityEngine;
using System;

/*

I am using serialization in this script in order to display the 
variables in the inspector window of the WorldModel GameObject to
which the script is attached to. This way, since the object is
present in every level, setting up the level variables is much 
easier for the developer.

*/

public class Initializer : MonoBehaviour {

	//This is the file for setting variables at the start of the game.
	//all the fields show up in inspector so no need to set the vals here.
	[SerializeField]
	private int v1;
	[SerializeField]
	private int v2;
	[SerializeField]
	private double v3;
	[SerializeField]
	private double v4;
	[SerializeField]
	private double v5;
	[SerializeField]
	private double v6;
	private double v7;
	private double v8;
    [SerializeField]
    private double v9;
	[SerializeField]
	private int v10;
	[SerializeField]
	private int v11;
	[SerializeField]
	private int v12;

	//The following variables are needed for stop rules.
	[SerializeField]
	private int v13;
	[SerializeField]
	private int v14;
	[SerializeField]
	private int v15;
	[SerializeField]
	private int v16;
		
	/*

	Getter for the params since the instances are private.
	Everything is returned as adouble, the ints are re-declared as integers wherever needed in 
	other code files.

	Purpose: to return the values of the private parameters;
	Input: string (has to exactly match the spelling of the variables in this script);
	Output: double;

	*/

	/*

	Note: that the variables v7 and v8 are dynamic variables that depend on a slider
	in the game. Therefore, everytimme the slider's position is changed, these params
	change their values accordingly too. However, since the dynamic values are reflected
	only in the UI scripts, for this script to detect the change, the variables are 
	re-declared every time the getParam is called. 

	*/

	public double getParam(string param)
	{
		// Set the variables v7 and v8 so that they're up to their latest values;
		v7 = GameObject.Find ("BodyL").GetComponent<Syringe> ().percentage;
		v8 = GameObject.Find ("BodyR").GetComponent<Syringe> ().percentage;
		if (v7 + v8 > 1 || v7 + v8 < 0) {
			if (Math.Max (v7, v8) == v7) {
				v8 = Math.Abs (v8);
				v7 = 1 - v8;
			} else {
				v7 = Math.Abs (v7);
				v8 = 1 - v7;
			}
		}
		// Getter checks;
		if (param.Equals ("v1", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v1;
		} 
		else if (param.Equals ("v2", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v2;
		}
		else if (param.Equals ("v3", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v3;
		}
		else if (param.Equals ("v4", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v4;
		}
		else if (param.Equals ("v5", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v5;
		}
		else if (param.Equals ("v6", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v6;
		}
		else if (param.Equals ("v7", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v7;
		}
		else if (param.Equals ("v8", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v8;
		}
		else if (param.Equals ("v9", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return v9;
		}
		else if (param.Equals ("v10", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v10;
		}
		else if (param.Equals ("v11", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v11;
		}
		else if (param.Equals ("v12", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v12;
		}
		else if (param.Equals ("v13", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v13;
		}
		else if (param.Equals ("v14", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v14;
		}
		else if (param.Equals ("v15", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v15;
		}
		else if (param.Equals ("v16", System.StringComparison.InvariantCultureIgnoreCase)) 
		{
			return (double)v16;
        }
        else {
			throw new Exception ();
		}
	}

}
