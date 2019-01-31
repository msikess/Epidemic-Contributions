using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

public class SpawnM2 : MonoBehaviour {
	//initial stats
	public int rowCount, colCount, velocity;
	public float radius;
	private int total;
	private int totalCuredA;
	private int totalCuredB;
	private int totalSick;

	//game objects
	public GameObject healthyPrefab;
	public Sprite skull;

	//number generator list
	private List<int> availableNumList;
	//private List<int> randomNumList;

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
		//randomNumList = new List<int> ();
		for (int i = 0; i < rowCount*colCount; i++) {
			availableNumList.Add (i);
		}


		//generate random list for sick people
		int numSick = world.retDay (1).get ("totalSymps");
		int numCuredA = world.retDay (1).get ("totala");
		int numCuredB = world.retDay (1).get ("totalb");
		//int totalPop = (int)(init.getParam ("population") / 2);
		//total = Mathf.RoundToInt(numSick * 100f / totalPop);
		total = numSick;
		totalCuredA = numCuredA;
		totalCuredB = numCuredB;
		//randomNumList = generateSick (availableNumList, randomNumList, total);

		//set size of the prefab
		healthyPrefab.GetComponent<Transform> ().localScale = new Vector3(radius/rowCount, radius/rowCount, 1);

		for (int j = -Mathf.FloorToInt(rowCount/2f); j < Mathf.CeilToInt(rowCount/2f); j++) {
			for (int i = -Mathf.CeilToInt(colCount/2f); i < Mathf.FloorToInt(colCount/2f); i++) {
				GameObject newPeep = Instantiate (healthyPrefab, new Vector3 (i*15f/colCount + .1f, -j*3f/rowCount + 3.3f, 0), Quaternion.identity);
				healthy.Add (newPeep);	
				newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3(Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
			}
		}

		//healthy[0].GetComponent<SpriteRenderer> ().enabled = false;

		for (int i = 0; i < total; i++) {
			GameObject sickPeep = healthy [i];
			sickPeep.GetComponent <SpriteRenderer> ().color = new Color(1f, 0, 0);
			//sickPeep.GetComponent <SpriteRenderer> ().sprite = skull;
		}

	}
		
	public IEnumerator buttonClicked(Initializer init, GameBuilder1 world) {
		int numSick = world.retDay (1).get ("totalSymps");
		//int totalPop = (int)(init.getParam ("population") * 0.3);
		//total = Mathf.RoundToInt(numSick * 100f / totalPop);
		//total = numSick;
		if (total <= healthy.Count) {
			//int catchD = numSick - total;
			while (total <= numSick && total <= healthy.Count) {
				for (int i = 0; i < total; i++) {
					GameObject sickPeep = healthy [i];
					sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
				}
				total += 1;
				//yield return new WaitForSecondsRealtime (.01f);
				//Debug.Log ("currently sick pop: " + total + ", and my target is: " + n + ". Still going? " + (total>=n));
			}
		}
		yield return null;
	}

	public void cure(Initializer init, GameBuilder1 world){
		//int sick = world.retDay (1).get ("totalSymps");
		int cured = world.retDay (1).get ("totalCuredDay");
		int target = total - cured;
		total = target;
		totalCuredA = world.retDay (1).get ("totala");
		totalCuredB = world.retDay (1).get ("totalb");
	}

	public IEnumerator disappear (GameBuilder1 world){
		Color sick = new Color (1f, 0, 0);
		foreach (GameObject sickPeep in healthy) {
			if (sickPeep.GetComponent <SpriteRenderer> ().color == sick) {
				sickPeep.SetActive (false);
			}
		}
		yield return null;
	}

	public IEnumerator appear(){
		foreach (GameObject sickPeep in healthy) {
			if (sickPeep.activeSelf == false) {
				sickPeep.SetActive (true);
			}
		}
		int i = 0;
		while (i < totalCuredA + totalCuredB + total) {
			if (i < totalCuredA) {
				healthy [i].GetComponent <SpriteRenderer> ().color = new Color (0, 1f, 0);
				i++;
			} else if (i < totalCuredA + totalCuredB && i >= totalCuredA) {
				healthy [i].GetComponent <SpriteRenderer> ().color = new Color (0, 0, 1f);
				i++;
			} else if (i < totalCuredA + totalCuredB + total && i >= totalCuredA + totalCuredB) {
				healthy [i].GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
				i++;
			}
		}
		yield return null;
	}

	public IEnumerator catchDisease(GameBuilder1 world, Initializer init){
		Color curea = new Color (0, 1f, 0);
		Color cureb = new Color (0, 0, 1f);
		Color stillSick = new Color (1f, 0, 0);
		int newSick = world.retDay (1).get ("totalSymps") - (world.retDay (2).get ("totalSymps") - world.retDay(2).get("totalCuredDay"));
		int healthyNum =(int) (init.getParam ("population") / 2) - world.retDay (2).get ("totalCuredTotal") - total - newSick - 1;
		int i = 0;
		if (newSick < healthyNum) {
			while (i < newSick) {
				foreach (GameObject sickPeep in healthy) {
					if (sickPeep.GetComponent <SpriteRenderer> ().color != curea
					    && sickPeep.GetComponent <SpriteRenderer> ().color != cureb
					    && sickPeep.GetComponent <SpriteRenderer> ().color != stillSick
					    && random () == true
					    && i < newSick) {
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
						i++;
					}
				}
			}
		} else {
				foreach (GameObject sickPeep in healthy) {
				if (sickPeep.GetComponent <SpriteRenderer> ().color != curea
				     && sickPeep.GetComponent <SpriteRenderer> ().color != cureb
				     && sickPeep.GetComponent <SpriteRenderer> ().color != stillSick) {
					sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
				} else {
					yield return null;
				}
				}
		}
		total = total + newSick;
		yield return null;
	}

	//update sick population on click event
	/*void Update() {
		//		numSick += 5;
		//		randomNumList = generateSick (availableNumList, randomNumList, numSick);

		//		Debug.Log ("There are " + total + " sick now.");
		//		Debug.Log ("There are " + availableNumList.Count + " healthy now.");
		//		for (int i = total - numSick; i < total; i++) {
		//			GameObject sickPeep = healthy [randomNumList [i]];
		//			sickPeep.GetComponent <SpriteRenderer> ().color = new Color(255, 0, 0);
		//			//sickPeep.GetComponent <SpriteRenderer> ().sprite = skull;
		//			sickPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3(Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
		//		}
		//		printList (availableNumList);

		//test cured pop
		float leftPercent = lefty.GetComponent<Slider> ().value / 20;
		float rightPercent = righty.GetComponent<Slider> ().value / 20;
		int numTrtA = Mathf.CeilToInt ((float) (total * leftPercent));
		int numTrtB = Mathf.FloorToInt ((float) (total * rightPercent));

		int leftBound = Mathf.FloorToInt((float) (colCount * leftPercent));
		int rightBound = Mathf.FloorToInt((float) (colCount * (1 - rightPercent)));

		int countA = 0;
		int countB = 0;
		int rows = Mathf.CeilToInt ((float)total / colCount);
		//Debug.Log ("TOTAL SICK NUM: " + total);

		//Debug.Log ("Num trated by A: " + numTrtA + ", and treated by B: " + numTrtB);
		//Debug.Log ("The left index is : " + leftBound + ", and right index is: " + rightBound);

		//color sick people treated by A
		for (int j = 0; j < colCount; j++) {
			for (int i = 0; i <= rows; i++) {
				int location = i * colCount + j;
				if (location < total && total <= healthy.Count) {
					//Debug.Log ("location is: " + location);
					GameObject sickPeep = healthy [location];
					int index = location % colCount;
					if (index <= leftBound && countA < numTrtA && game.cureCheck() == false) {
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = false;
						sickPeep.GetComponent <cakeslice.Outline> ().color = 1;
						countA++;
					} else {
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = true;
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (1f, 0, 0);
					}
				}
			}
		}

		//color sick people treated by B
		for (int i = colCount - 1; i >= 0; i--) {
			for (int j = rows; j >= 0; j--) {
				int location = j * colCount + i;
				if (location < total && total <= healthy.Count) {
					//Debug.Log ("location is: " + location);
					GameObject sickPeep = healthy [location];
					int index = location % colCount;
					if (index >= rightBound - 1 && countB < numTrtB && game.cureCheck() == false) {
						sickPeep.GetComponent <cakeslice.Outline> ().eraseRenderer = false;
						sickPeep.GetComponent <cakeslice.Outline> ().color = 2;
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
	}*/

	void printList(List<int> num) {
		string ret = "";
		for (int i = 0; i < num.Count; i++) {
			ret += num [i].ToString ();
			ret += ", ";
		}
		Debug.Log (ret);
	}

	List<int> generateSick (List<int> available, List<int> selected, int n) {
		for (int i = 0; i < n; i++) {
			selected.Add (i);
			available.Remove (i);
		}
		return selected;
	}

	//generate n unrepeated numbers from x elements
	List<int> generateRandomNum (List<int> available, List<int> selected, int n) {
		for (int i = 0; i < n; i++) {
			int newNum = Random.Range (0, available.Count);
			while (selected.Contains (newNum)) {
				newNum = Random.Range (0, available.Count);
			}
			selected.Add (newNum);
			available.Remove (newNum);
		}
		return selected;
	}

	public bool random(){
		if (Random.value >= .5) {
			return true;
		} else {
			return false;
		}
	}



}
