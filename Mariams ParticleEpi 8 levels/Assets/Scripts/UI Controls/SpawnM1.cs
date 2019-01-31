using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

public class SpawnM1 : MonoBehaviour {
	//initial stats
	public int rowCount, colCount, velocity;
	public float radius;

	//total is the number of munchkins sick at a particular day.
	private int total;
	//uncured represents the number of them who remain sick after an excriciating day of treatments
	private int uncured;


	//game objects
	public GameObject healthyPrefab;
	public Sprite seat;
	public Sprite healed;
	public Sprite unTreated;
	public Sprite trtA;
	public Sprite trtB;


	//number generator list
	private List<int> availableNumList;

	//list of total population
	public List<GameObject> healthy;

	//left and right sliders
	private GameObject lefty;
	private GameObject righty;

	//link to WorldModel so that we can access daily stats and initial parameters later
	[SerializeField]
	private WorldModel game;


	public void onStart(Initializer init, GameBuilder1 world){
		lefty = GameObject.Find ("Slider_left");
		righty = GameObject.Find ("Slider_right");

		//generate available number list
		availableNumList = new List<int>();
		for (int i = 0; i < rowCount*colCount; i++) {
			availableNumList.Add (i);
		}

		int numSick = world.retDay (1).get ("totalSymps");
		total = 0;
		uncured = 0;

		//set size of the prefab
		healthyPrefab.GetComponent<Transform> ().localScale = new Vector3(radius/rowCount, radius/rowCount, 1);

		//Instantiate a 2D array of healthyPrefabs
		int count = 0;
		for (int j = -Mathf.FloorToInt(rowCount/2f); j < Mathf.CeilToInt(rowCount/2f); j++) {
			for (int i = -Mathf.CeilToInt(colCount/2f); i < Mathf.FloorToInt(colCount/2f); i++) {

				//The first 5 seats are always colored purplish to indicate the winning condition.
				if (count < 5) {

					//The new Vector3 positions of the prefabs are hard coded to fit the screen. 
					//Can be adjusted if the designs change in the future (Yes, I'm talking to you, future coders:))
					GameObject newPeep = Instantiate (healthyPrefab, new Vector3 (i * 9.8f / colCount + -2.6f, -j * 4.2f / rowCount - .7f, 0), Quaternion.identity);
					newPeep.GetComponent <SpriteRenderer> ().color = new Color (0.2f, 0f, 0.2f);
					//Add the instantiated GameObject into healthy List to keep track of them (and modify later!). 
					healthy.Add (newPeep);	
					//Cool little particle collision effect that you can experiment with by setting the velocity param in the WorldModel.
					newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
				} else {
				//The othe seats are generated as regular healthyPrefabs. The rest is exactly the same as above.
					GameObject newPeep = Instantiate (healthyPrefab, new Vector3 (i * 9.8f / colCount + -2.6f, -j * 4.2f / rowCount - .7f, 0), Quaternion.identity);
					healthy.Add (newPeep);	
					newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
				}
				count++;
			}
		}
	}


	//The coroutine for the munchkins to show up one by one as they enter the clinic.
	public IEnumerator appear(GameBuilder1 world){
		//First, let's get the # of sick munchkins from WorldModel
		int sick = world.retDay (1).get ("totalSymps");
		//And we start "total" with 0, before incrementing it all the up to the # of sick Mooches.
		total = 0;
		if (total <= healthy.Count) {
			//Now we wanna increment total all the way till the actual # of sick, 
			//     or healthy.Count, which is the # of seats available in clinic.
			while (total < sick && total < healthy.Count) {
				total += 1;
				uncured = total;
				for (int i = 0; i < total; i++) {
					GameObject sickPeep = healthy [i];
					sickPeep.GetComponent <SpriteRenderer> ().sprite = unTreated;
					sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, .5f, .5f);
				}
		
				//yield return to give off the one-by-one pseudo-animation effect. 
				//Realize that we used 1f/sick to control the speed of animation: The more sick, the faster, so that you don't get bored watching.
				yield return new WaitForSecondsRealtime (1f/sick);
			}
		}
		yield return new WaitForSecondsRealtime(.5f);
	}


	//The coroutine for the munchkins to disappear one by one as they exit the clinic through the pipe.
	public IEnumerator disappear(){
		for (int i = total - 1; i >= 0; i--) {

			//Again, the first 5 seats, we wanna keep them in a different color to indicate the winning condition.
			if (i < 5) {
				GameObject sickPeep = healthy [i];
				sickPeep.GetComponent <SpriteRenderer> ().sprite = seat;
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (0.2f, 0f, 0.2f, 0.5f);
			} else {
			//For all the other seats, just switch back to seat Sprite and color them grey, the dullest of all colors.
				GameObject sickPeep = healthy [i];
				sickPeep.GetComponent <SpriteRenderer> ().sprite = seat;
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (.6f, .6f, 7f, .5f);
			}
			//Again, animation stuff.
			yield return null;
		}
		yield return null;
	}

	//The coroutine for curing muchkins with treatment A and B. The cured ones are color-coded accordingly as well.
	public IEnumerator cure(Initializer init, GameBuilder1 world){
		//Total cured
		int cured = world.retDay (1).get ("totalCuredDay");
		//Cured by treatment A and B
		int curedA = world.retDay (1).get ("curedAnum");
		int curedB = world.retDay (1).get ("curedbnum");
		//Total # of sick minus total # of cured is the # uncured, hence the name "target".
		int target = total - cured;
		//set uncured to total first, and we'll decrement it to target later!
		uncured = total;

		//Calculate how many rows of sick Munch'es there're in the clinic.
		int rows = Mathf.CeilToInt ((float)total / colCount);

		//color people cured by A
		for (int j = 0; j < colCount; j++) {
			for (int i = 0; i <= rows; i++) {
				int location = i * colCount + j;
				if (location < total) {
					GameObject sickPeep = healthy [location];
					if (uncured > (total - curedA)) {
						sickPeep.GetComponent <SpriteRenderer> ().sprite = healed;
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (.3f, 1f, .3f);
						uncured -= 1;
					}
				}
			}
		}

		//color people cured by B
		for (int i = colCount - 1; i >= 0; i--) {
			for (int j = rows; j >= 0; j--) {
				int location = j * colCount + i;
				if (location < total) {
					GameObject sickPeep = healthy [location];
					if (uncured > (total - curedA - curedB)) {
						sickPeep.GetComponent <SpriteRenderer> ().sprite = healed;
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (.3f, .3f, 1f);
						uncured -= 1;
					}
				}
			}
		}
		yield return new WaitForSecondsRealtime(1f);
	}


	//Update sick population every frame. The Munchkins are highlighted in different ways based on the treatment combination.
	void Update() {
		//Find the percentage of each treatment based on player's slider positions.
		float leftPercent = lefty.GetComponent<Slider> ().value / 20;
		float rightPercent = righty.GetComponent<Slider> ().value / 20;

		//Then calculate how many Munch'es those percentages correspond to, like so:
		int numTrtA = Mathf.CeilToInt ((float) (total * leftPercent));
		int numTrtB = Mathf.FloorToInt ((float) (total * rightPercent));

		//And we can thus compute the left and right boundaries beyond which we should color and highlight the Munches accordingly.
		int leftBound = Mathf.FloorToInt((float) (colCount * leftPercent));
		int rightBound = Mathf.FloorToInt((float) (colCount * (1 - rightPercent)));

		//Calculate how many rows of sick Munch'es there're in the clinic.
		int rows = Mathf.CeilToInt ((float)total / colCount);

		//Let's start the counts of A and B at 0, and work our way up to numTrtA and numTrtB respectively.
		int countA = 0;
		int countB = 0;


		//color sick people treated by A
		for (int j = 0; j < colCount; j++) {
			for (int i = 0; i <= rows; i++) {
				int location = i * colCount + j;
				if (location < total && uncured <= healthy.Count) {
					//Debug.Log ("location is: " + location);
					GameObject sickPeep = healthy [location];
					int index = location % colCount;
					if (index <= leftBound && countA < numTrtA && game.cureCheck() == false) {
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = false;
						sickPeep.GetComponent <cakeslice.Outline> ().color = 1;
						sickPeep.GetComponent <SpriteRenderer> ().sprite = trtA;
						countA++;
					} else if (game.cureCheck () == false){
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = true;
						sickPeep.GetComponent <SpriteRenderer> ().sprite = unTreated;
					} else {
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = true;
					}
				}
			}
		}

		//color sick people treated by B
		for (int i = colCount - 1; i >= 0; i--) {
			for (int j = rows; j >= 0; j--) {
				int location = j * colCount + i;
				if (location < total && uncured <= healthy.Count) {
					//Debug.Log ("location is: " + location);
					GameObject sickPeep = healthy [location];
					int index = location % colCount;
					if (index >= rightBound - 1 && countB < numTrtB && game.cureCheck() == false) {
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = false;
						sickPeep.GetComponent <cakeslice.Outline> ().color = 2;
						sickPeep.GetComponent <SpriteRenderer> ().sprite = trtB;
						countB++;
					} 
				}
			}
		}

		//paint the rest blue to show empty seats
		for (int i = total; i < healthy.Count; i++) {
			GameObject seats = healthy [i];
			seats.GetComponent <cakeslice.Outline> ().eraseRenderer = true;
			seats.GetComponent <SpriteRenderer> ().color = new Color (.6f, .6f, 7f, .5f);
		}
	}

}
