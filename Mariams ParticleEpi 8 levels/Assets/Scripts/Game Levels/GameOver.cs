using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//this script controls the canvas that appears once you win/lose the game
public class GameOver : MonoBehaviour {
	private Canvas can;
	private Text txt;

	// Use this for initialization
	void Start () {
		can = GameObject.Find ("GameOverUI").GetComponent<Canvas> ();
		txt = GameObject.Find ("WinLose").GetComponentInChildren<Text> ();
		can.GetComponent<Canvas> ().enabled = false;
	}

	//replays the game scene
	public void replay() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//takes you the the main menu
	public void toMenu() {
		SceneManager.LoadScene (0);
	}

	//determines text to be displayed based on whether you won/lost
	public void gmOver(string state){
		if (state != "none") {
			can.GetComponent<Canvas> ().enabled = true;
			GameObject.Find ("NextDay").GetComponent<Button> ().enabled = false;
			if (state.Equals ("lost")) {
				txt.text = "You Lost...";
			}
			if (state.Equals ("won")) {
				txt.text = "You Won!";
			}
		}
	}

}
