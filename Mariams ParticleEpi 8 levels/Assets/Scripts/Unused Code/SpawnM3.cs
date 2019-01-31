using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

public class SpawnM3 : MonoBehaviour {
	//initial stats
	public int rowCount, colCount, velocity;
	public float radius;
	private int total;
	private int totalCuredA;
	private int totalCuredB;


	//game objects
	public GameObject healthyPrefab;

	//number generator list
	private List<int> availableNumList;

	//list of total population
	public List<GameObject> healthy;

	//sliders
	private GameObject lefty;
	private GameObject righty;

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
		int numCuredA = world.retDay (1).get ("totala");
		int numCuredB = world.retDay (1).get ("totalb");
		total = numSick;
		totalCuredA = numCuredA;
		totalCuredB = numCuredB;

		//set size of the prefab
		healthyPrefab.GetComponent<Transform> ().localScale = new Vector3(radius/rowCount, radius/rowCount, 1);

		for (int j = -Mathf.FloorToInt(rowCount/2f); j < Mathf.CeilToInt(rowCount/2f); j++) {
			for (int i = -Mathf.CeilToInt(colCount/2f); i < Mathf.FloorToInt(colCount/2f); i++) {
				GameObject newPeep = Instantiate (healthyPrefab, new Vector3 (i*15f/colCount + .1f, -j*3f/rowCount + 3.3f, 0), Quaternion.identity);
				healthy.Add (newPeep);	
				newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3(Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
			}
		}

	}

	public IEnumerator buttonClicked(Initializer init, GameBuilder1 world) {
		int numSick = world.retDay (1).get ("totalSymps");
		if (total <= healthy.Count) {
			while (total <= numSick && total <= healthy.Count) {
				for (int i = 0; i < total; i++) {
					GameObject sickPeep = healthy [i];
					sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
				}
				total += 1;
			}
		}
		yield return null;
	}

	public void cure(Initializer init, GameBuilder1 world){
		int cured = world.retDay (1).get ("totalCuredDay");
		int target = total - cured;
		total = target;
		totalCuredA = world.retDay (1).get ("totala");
		totalCuredB = world.retDay (1).get ("totalb");
	}

	public IEnumerator disappear() {
		
		for (int i = total + totalCuredA + totalCuredB; i >= totalCuredA + totalCuredB; i--) {
			GameObject sickPeep = healthy [i];
			sickPeep.SetActive (false);
		}
		yield return null;
	}

	public IEnumerator appear(){
		int totalCured = totalCuredA + totalCuredB;
		for (int i = 0; i < totalCured; i++) {
			GameObject sickPeep = healthy [i];
			sickPeep.SetActive (true);
			if (i < totalCuredA) {
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 1f, 0);
			} else {
				sickPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 0, 1f);
			}
			yield return null;
		}


		for (int i = totalCured; i < totalCured + total; i++) {
			GameObject sickPeep = healthy [i];
			sickPeep.SetActive (true);
			sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
			yield return null;
		}

		yield return new WaitForSecondsRealtime(1f);
	}

	public IEnumerator catchDisease(GameBuilder1 world){
		int newSick = world.retDay (1).get ("totalSymps") - total;
		for (int i = total + totalCuredA + totalCuredB; i <= totalCuredA + totalCuredB + total + newSick; i++) {
			GameObject sickPeep = healthy [i];
			sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
		}

		total = total + newSick;
		yield return new WaitForSecondsRealtime(1f);
	}

}
