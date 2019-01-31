using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLightPopulation : MonoBehaviour {

	//initial stats, set in the inspector
	public int rowCount, colCount, velocity;
	public float radius;

	//globals that keep track of totals
	private int totalSick;
	private int totalCuredA;
	private int totalCuredB;

	//boolean to check when creatures should return to clinic through the pipe; this animation happens in fixedupdate and hence needs a boolean to trigger it.
	private bool returnClinic;
	private bool appearInPopulation;


	//references to prefabs that make up the population of the city; some are animated (jigglyHealthy) and some are not (healthyPrefab).
	public GameObject healthyPrefab;
	public GameObject jigglyHealthy;

	//Lists that keep track of the sick, cured by A, cured by B and healthy sub populations within the city
	public List<GameObject> sickPop;
	public List<GameObject> curedAPop;
	public List<GameObject> curedBPop;
	public List<GameObject> healthyPop;



	[SerializeField]
	private WorldModel game;

	public void onStart (Initializer init, GameBuilder1 world)
	{
		//initialize globals
		returnClinic = false;
		appearInPopulation = false;
		totalSick = world.retDay (1).get ("totalSymps");
		totalCuredA = 0;
		totalCuredB = 0;
		int totalHealthy = (int)(init.getParam ("population") - totalSick);

		//set size of the prefab
		healthyPrefab.GetComponent<Transform> ().localScale = new Vector3 (radius / rowCount, radius / rowCount, 1);

		//instantiate all the creatures in a grid
		for (int j = -Mathf.FloorToInt(rowCount/2f); j < Mathf.CeilToInt(rowCount/2f); j++) {
			for (int i = -Mathf.CeilToInt(colCount/2f); i < Mathf.FloorToInt(colCount/2f); i++) {
				float rand = Random.Range (0, 2);
				if (rand < 0.3) {
					GameObject newPeep = Instantiate (jigglyHealthy, new Vector3 (i * 15f / colCount + .1f, -j * 3f / rowCount + 3.2f, 0), Quaternion.identity);
					healthyPop.Add (newPeep);	
					newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
				} else {
					GameObject newPeep = Instantiate (healthyPrefab, new Vector3 (i * 15f / colCount + .1f, -j * 3f / rowCount + 3.2f, 0), Quaternion.identity);
					healthyPop.Add (newPeep);	
					newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
				}
			}
		}

		//remove the creatures who are sick initially
		for (int i = 0; i < totalSick; i++) {
			GameObject sick = healthyPop [i];
			healthyPop.Remove (sick);
			Destroy (sick);
		}
	}


	//adjust globals after creatures are cured
	public void cure(Initializer init, GameBuilder1 world){
		int cured = world.retDay (1).get ("totalCuredDay");
		totalSick -= cured;
		totalCuredA = world.retDay (1).get ("curedAnum");
		totalCuredB = world.retDay (1).get ("curedbnum");
	}

	//waits for a second then sets returnClinic to true, hence triggering the animation for creatures returning to the clinic in fixedUpdate
	public IEnumerator disappear() {
		yield return new WaitForSecondsRealtime (1f);
		returnClinic = true;
		yield return null;
	}

	//animation for creatures going to a random place in population triggered
	public IEnumerator appear(){		
		yield return new WaitForSecondsRealtime (1f);
		int totalCured = totalCuredA + totalCuredB;
		appearInPopulation = true;
		yield return null;
	}

		




	//animation for new creatures catching the disease
	public IEnumerator catchDisease(GameBuilder1 world){
		int newSick = world.retDay (1).get ("totalSymps") - totalSick;
		for (int i = 0; i < newSick; i++) {
			GameObject sickPeep = healthyPop [i];
			sickPop.Add (sickPeep);
			healthyPop.Remove (sickPeep);
			sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, .4f, .4f);
			yield return null;
		}

		totalSick = totalSick + newSick;
		yield return new WaitForSecondsRealtime(.5f);
	}

	//animation for sick creatures returning to the clinic happens here, triggered by returnClinic boolean
	void FixedUpdate() {
		if (sickPop.Count == 0) {
			returnClinic = false;
		}

		if (sickPop.Count == totalSick && curedAPop.Count == totalCuredA && curedBPop.Count == totalCuredB){
			appearInPopulation = false;
		}

		if (returnClinic == true) {
			for (int i = 0; i < sickPop.Count; i++) {
				GameObject sick = sickPop[i];
				Vector3 pipe = new Vector3 (-7f, 4.7f, 0);//position of the pipe
				Vector3 dir = pipe - sick.transform.position;//direction vector for each sick creature
				sick.GetComponent<CircleCollider2D>().isTrigger = true;//makes the circle collider for each sick creature a trigger collider, so they can move through other rigibodies
				sick.GetComponent<Rigidbody2D> ().gravityScale = 0;//gets rid of gravity so they can "fly"to the pipe
				sick.GetComponent<Rigidbody2D> ().velocity = dir.normalized * 6;//moves the sick creature along the direction vector
				if (dir.magnitude <= 0.2f) {//when creature is close enough to pipe, it gets destroyed
					sickPop.Remove (sick);
					Destroy (sick);
				}

			}

		}

		if(appearInPopulation == true){

			int totalCured = totalCuredA + totalCuredB;//get total number cured
			Vector3 door = new Vector3 (-7f, 4.7f, 0);//get position of door

			//sick people
			for (int i = 0; i < totalSick; i++) {
				//instantiate, color, and add sick person to the array
				GameObject sickPeep = Instantiate (healthyPrefab, door, Quaternion.identity);
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, .4f, .4f);


				//move sick person to random position in the population
				Vector3 randPos = new Vector3 (-8f, 6f, 0);
				Vector3 dir = door - sickPeep.transform.position;//direction vector for each sick creature
				sickPeep.GetComponent<CircleCollider2D>().isTrigger = true;//makes the circle collider for each sick creature a trigger collider, so they can move through other rigibodies
				sickPeep.GetComponent<Rigidbody2D> ().gravityScale = 0;//gets rid of gravity so they can "fly"to their position
				sickPeep.GetComponent<Rigidbody2D> ().velocity = dir.normalized * 6;//moves the sick creature along the direction vector
				if (dir.magnitude <= 0.2f) {//when creature is close enough to door, it stops
					sickPeep.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
					sickPop.Add (sickPeep);
				}


			}

			//cured people
			for (int i = 0; i < totalCured; i++) {
				//instantiate cured person
				GameObject curedPeep = Instantiate (healthyPrefab, door, Quaternion.identity);
				if (i < totalCuredA) {
					//color and add to array
					curedPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 1f, 0);
					curedAPop.Add (curedPeep);

				} else {
					//color and add to array
					curedPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 0, 1f);
					curedBPop.Add (curedPeep);
				}

				//move cured person to random position in the population
				Vector3 randPos = new Vector3 (-8f, 7f, 0);
				Vector3 dir = door - curedPeep.transform.position;//direction vector for each sick creature
				curedPeep.GetComponent<CircleCollider2D>().isTrigger = true;//makes the circle collider for each sick creature a trigger collider, so they can move through other rigibodies
				curedPeep.GetComponent<Rigidbody2D> ().gravityScale = 0;//gets rid of gravity so they can "fly"to their position
				curedPeep.GetComponent<Rigidbody2D> ().velocity = dir.normalized * 6;//moves the sick creature along the direction vector
				if (dir.magnitude <= 0.2f) {//when creature is close enough to door, it stops
					curedPeep.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
				}
			}
			
		}

	}
}
