using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

//Note: the serialization here is necessary in order
// for this script to dynamically interact with the 
// rest of the scripts. So, we connect each script 
// to an appropriate field in WorldModel.cs of the ScriptHub
// game object in the Inspector window. This way, we enable
// the dynamic info exchange between the scripts;

//this script is the driver for the whole game.
public class WorldModel : MonoBehaviour
{


	//for keeping track of which day we're on (previous or current);
	//Note: in this code I have removed the prevDay code because I'm
	//re-making in in real-time;
	private int state;
	//for the initial params;
	[SerializeField]
	private Initializer init;
	//for game over;
	[SerializeField]
	private GameOver go;
	[SerializeField]
	//For setting up textField texts;
	private UITextScript txt;
	//for UI animation (part one);
	[SerializeField]
	private UIScriptOne one;
	//for UI animation (part two);
	[SerializeField]
	private UIScriptTwo two;
	//for UI interactibles;
	[SerializeField]
	private UIScriptThree three;
	//for JSon;
	private Json j = new Json();


	//coroutine variables;
	private Boolean clicked;
	private Boolean checker;

	//string that determines whether the game is won, lost, or none;
	private String end;

	//for enabling sliders during playtime, and disabling UI animation;
	private GameObject lefty;
	private GameObject righty;
	private Button nextDay;

	//the object that builds a list of days;
	public Calendar world;

	//on start, create a new world/game (the call to constructor automatically
	//	adds the first day initial params and displays it);
	void Awake ()
	{
		lefty = GameObject.Find ("SliderOne");
		righty = GameObject.Find ("SliderTwo");
		nextDay = GameObject.Find ("NextDay").GetComponent<Button> ();
		state = 0;
		world = new Calendar (init, one, two); 
		fill.dayOne (world, init);
		clicked = false;
		checker = false;
		StartCoroutine(two.methodTwo ());
		StartCoroutine (one.methodOne (world));
	}

	//the button function, starts the animation coroutine + the calculations;
	public void btnClicked ()
	{
		clicked = true;
		checker = true;
		StartCoroutine ("taskOnClick");
	}

	//calculates the treatment assignment results for today;
	//creates a next day and displays it;
	//checks if player has reached a lose/win state;
	public IEnumerator taskOnClick ()
	{
		//Calculate treatment decisions/apply treatment;
		if (state == 0){
			if (clicked == true) {
				nextDay.enabled = false;
				lefty.GetComponent<Slider> ().enabled = false;
				righty.GetComponent<Slider> ().enabled = false;
				world.calculate (init);
				if (world.totalDays () == 1) {
					end = "none";
				}
				j.jAdder (world, init, this);
				txt.methodOne (world, 1);
				three.methodTwo (world, 1);

				//1. animation part 1.1
				yield return StartCoroutine(one.methodThree (init, world));
				two.methodThree(init, world);


				//2. animation part 1.2
				yield return StartCoroutine (one.methodTwo ());

				yield return StartCoroutine(two.methodOne());

				clicked = false;
			}
			//Create a New Day;
			if (checker == true && end == "none") {
				world.nextDay (init);
				txt.methodTwo (world, 1);

				lefty.GetComponent<Slider> ().value = 0f;
				righty.GetComponent<Slider> ().value = 0f;

				//3. animation part 2.1
				yield return StartCoroutine(two.methodThree(world));

				//4. animation part 2.2

				yield return StartCoroutine(two.methodTwo());
				yield return new WaitForSecondsRealtime (1f);
				yield return StartCoroutine(one.methodOne(world));

				// Check if game is over or not;
				end = StopRule.check (world, init);
				if (end != "none") {
					world.calculate (init);
					j.jAdder (world, init, this);
					go.gmOver (end);
					yield return null;
				} else {
					checker = false;
					nextDay.enabled = true;
					lefty.GetComponent<Slider> ().enabled = true;
					righty.GetComponent<Slider> ().enabled = true;
					txt.methodThree (world);
				}
				yield return null;
			} 
			yield return null;
		}
		yield return null;
	}

//These methods are needed in other scripts;
	public Boolean cureCheck(){
		return checker;
	}

	public Json jGet(){
		return j;
	}

	public String gameState(){
		return end;
	}

	public Calendar worldRet(){
		return world;
	}
		
}
