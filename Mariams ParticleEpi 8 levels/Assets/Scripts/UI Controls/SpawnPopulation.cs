using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

//this script controls the population of the city visualization
public class SpawnPopulation : MonoBehaviour {
	
	//initial stats, set in the inspector
	public int rowCount, colCount, velocity;
	public float radius;

	//globals that keep track of totals
	private int totalSick;
	private int totalCuredA;
	private int totalCuredB;

	//boolean to check when creatures should return to clinic through the pipe; this animation happens in fixedupdate and hence needs a boolean to trigger it.
	private bool returnClinic;


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

	//animation for creatures beeing shot out of the pipe and back into the city
	public IEnumerator appear(){		
		Vector3 pos = new Vector3 (-7f, 4.7f, 0);
		int totalCured = totalCuredA + totalCuredB;

		for (int i = 0; i < totalSick; i++) {
			GameObject sickPeep = Instantiate (healthyPrefab, pos, Quaternion.identity);
			sickPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (3, 8), 0, 0);
			//sickPeep.SetActive (true);
			sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, .4f, .4f);
			sickPop.Add (sickPeep);
			yield return null;
		}

		for (int i = 0; i < totalCured; i++) {
			GameObject sickPeep = Instantiate (healthyPrefab, pos, Quaternion.identity);
			sickPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (3, 8), 0, 0);
			if (i < totalCuredA) {
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 1f, 0);
				curedAPop.Add (sickPeep);
			} else {
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 0, 1f);
				curedBPop.Add (sickPeep);
			}
			yield return null;
		}

		yield return new WaitForSecondsRealtime(.5f);
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

	}

}

