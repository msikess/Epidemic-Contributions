using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

//this script is the driver for the whole game.
public class WorldModel : MonoBehaviour
{


	//for keeping track of which day we're on;
	private int state;
	//for the initial params;
	[SerializeField]
	private Initializer init;
	//for fillers;
	[SerializeField]
	private Filler fill;
	//for game over;
	[SerializeField]
	private GameOver go;
	[SerializeField]
	//For setting up textField texts;
	private SetText txt;
	//for spawning the seats;
	[SerializeField]
	private SpawnM1 spawner;
	//for spawning the population;
	[SerializeField]
	private SpawnPopulation spawner2;
	//for JSon;
	private Json j = new Json();
	//CSV outputter for testing;
	private CSVC csv;


	//coroutine variables;
	private Boolean clicked;
	private Boolean checker;

	//string that determines whether the game is won, lost, or none;
	private String end;

	//for enabling sliders during playtime, and disabling during prevDay;
	private GameObject lefty;
	private GameObject righty;
	private Button nextDay;

	//Soundtrack
	public AudioSource pump;

	//the object that builds a list of days (a.k.a. calendar);
	public GameBuilder1 world;

	//on start, create a new world/game (the call to constructor automatically
	//	adds the first day initial params and displays it);
	void Awake ()
	{
		lefty = GameObject.Find ("Slider_left");
		righty = GameObject.Find ("Slider_right");
		nextDay = GameObject.Find ("NextDay").GetComponent<Button> ();
		state = 0;
		world = new GameBuilder1 (init, spawner, spawner2); 
		fill.dayOne (world, init);
		clicked = false;
		checker = false;
		StartCoroutine(spawner2.disappear ());
		StartCoroutine (spawner.appear (world));
		csv = new CSVC (init.fileP());
	}

	//the sick creatures leave the population and enter the clinic at the beginning
	//void Start(){
		//StartCoroutine(spawner2.disappear ());
	//	StartCoroutine (spawner.appear (world));
	//}


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
					csv.firstDayAdder(world, init);
				} else {
					csv.nextDayAdder(world, init);
				}
				j.jAdder (world, init, this);
				txt.updateText (world, 1);
				fill.nextDay (world, 1);

				//1. cure people in clinic and population
				yield return StartCoroutine(spawner.cure (init, world));
				spawner2.cure(init, world);


				//2. make cured/still sick people move from clinic to population
				GameObject.Find ("Pump").GetComponent<Animator> ().SetBool ("Pump", true);
				pump.Play ();
				yield return StartCoroutine (spawner.disappear ());
				GameObject.Find ("Pump").GetComponent<Animator> ().SetBool ("Pump", false);

				yield return StartCoroutine(spawner2.appear());

				clicked = false;
			}
			//Create a New Day;
			if (checker == true && end == "none") {
				world.nextDay (init);
				PercentageSlider.setter ();
				txt.updateDay (world, 1);

				//commented out for now since we don't want to reset the values to 0;
				lefty.GetComponent<Slider> ().value = 0f;
				righty.GetComponent<Slider> ().value = 0f;

				//3. make people catch disease in population
				yield return StartCoroutine(spawner2.catchDisease(world));
				//yield return new WaitForSecondsRealtime (1f);

				//4. move sick people from population to clinic

				yield return StartCoroutine(spawner2.disappear());
				GameObject.Find ("Pump").GetComponent<Animator> ().SetBool ("PumpBack", true);
				yield return new WaitForSecondsRealtime (1f);
				yield return StartCoroutine(spawner.appear(world));
				GameObject.Find ("Pump").GetComponent<Animator> ().SetBool ("PumpBack", false);

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
					txt.sickUpdate (world);
				}
				yield return null;
			} 
			yield return null;
		}
		yield return null;
	}

	public Boolean cureCheck(){
		return checker;
	}

	public Json jGet(){
		return j;
	}

	public String gameState(){
		return end;
	}

	public GameBuilder1 worldRet(){
		return world;
	}
		
}
