using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorDots : MonoBehaviour {
	//initial stats
	public int rowCount, colCount, numSick;
	public float velocity;
	public float percentTrtA, percentTrtB;

	//game objects
	public GameObject healthyPrefab;
	public Sprite skull;

	//number generator list
	private List<int> availableNumList;
	private List<int> randomNumList;

	//list of total population
	public List<GameObject> healthy;

	// Use this for initialization
	void Start () {
		//generate available number list
		availableNumList = new List<int>();
		randomNumList = new List<int> ();
		for (int i = 0; i < rowCount*colCount; i++) {
			availableNumList.Add (i);
		}
		//generate random list for sick people
		randomNumList = generateSick (availableNumList, randomNumList, numSick);

		//set size of the prefab
		healthyPrefab.GetComponent<Transform> ().localScale = new Vector3(2f/rowCount, 2f/rowCount, 1);

		for (int j = -colCount/2; j < colCount/2; j++) {
			for (int i = -rowCount / 2; i < rowCount / 2; i++) {
				GameObject newPeep = Instantiate (healthyPrefab, new Vector3 (i*9f/rowCount + .6f, -j*6f/rowCount - 1f, 0), Quaternion.identity);
				healthy.Add (newPeep);	
				newPeep.GetComponent<Rigidbody2D> ().velocity = new Vector3(Random.Range (-velocity, velocity), Random.Range (-velocity, velocity), 0);
			}
		}

		//healthy[0].GetComponent<SpriteRenderer> ().enabled = false;

		for (int i = 0; i < numSick; i++) {
			GameObject sickPeep = healthy [randomNumList [i]];
			sickPeep.GetComponent <SpriteRenderer> ().color = new Color(255, 0, 0);
			//sickPeep.GetComponent <SpriteRenderer> ().sprite = skull;
		}

	}

	//update sick population on click event
	public void Update() {
		//		numSick += 5;
		//		randomNumList = generateSick (availableNumList, randomNumList, numSick);
		int total = randomNumList.Count;
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
		int numTrtA = Mathf.CeilToInt (numSick * percentTrtA);
		int numTrtB = Mathf.FloorToInt (numSick * percentTrtB);

		int leftBound = Mathf.CeilToInt(colCount * percentTrtA) - 1;
		int rightBound = Mathf.FloorToInt(colCount * (1 - percentTrtB));
		Debug.Log ("THE RIGHT BOUND IS: " + rightBound);
		Debug.Log ("THE LEFT BOUND IS: " + leftBound);

		int countA = 0;
		int countB = 0;
		int rows = Mathf.CeilToInt (total / colCount);

		//Debug.Log ("Num trated by A: " + numTrtA + ", and treated by B: " + numTrtB);
		//Debug.Log ("The left index is : " + leftBound + ", and right index is: " + rightBound);

		//color sick people treated by A
		for (int j = 0; j < colCount; j++) {
			for (int i = 0; i <= rows; i++) {
				int location = i * 10 + j;
				if (location < total) {
					//Debug.Log ("location is: " + location);
					GameObject sickPeep = healthy [randomNumList [location]];
					int index = location % colCount;
					if (index <= leftBound && countA < numTrtA) {
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (0, 255, 0);
						countA++;
					} else {
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (255, 0, 0);
					}
				}
			}
		}

		//color sick people treated by B
		for (int i = colCount - 1; i >= 0; i--) {
			for (int j = 0; j <= rows; j++) {
				int location = j * 10 + i;
				if (location < total) {
					//Debug.Log ("location is: " + location);
					GameObject sickPeep = healthy [randomNumList [location]];
					int index = location % colCount;
					if (index >= rightBound && countB < numTrtB) {
						sickPeep.GetComponent <SpriteRenderer> ().color = new Color (255, 255, 0);
						countB++;
					} 
				}
			}
		}

		if (rightBound - leftBound <= 1) {
			for (int i = rows; i >= 0; i--) {
				int location = i * 10 + rightBound - 1;
				if (location < total && countB < numTrtB) {
					GameObject sickPeep = healthy [randomNumList [location]];
					sickPeep.GetComponent <SpriteRenderer> ().color = new Color (255, 255, 0);
					countB++;
				}
			}
		}
	}



	public void setPercentageA(float n) {
		percentTrtA = n;
	}

	public void setPercentageB(float n) {
		percentTrtB = n;
	}

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
}
