using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script provides enables the player to mute all sound in the game
public class SoundControl : MonoBehaviour {

	private AudioSource bkgMusic;
	private AudioSource pumpSound;

	void Start(){
		bkgMusic = GameObject.Find ("BackgroundMusic").GetComponent<AudioSource> ();
		pumpSound = GameObject.Find ("PumpingSound").GetComponent<AudioSource> ();
	}

	public void mute(){
		if (bkgMusic.mute != true) {
			bkgMusic.mute = true;
			pumpSound.mute = true;
		} else {
			bkgMusic.mute = false;
			pumpSound.mute = false;
		}
	}
}
