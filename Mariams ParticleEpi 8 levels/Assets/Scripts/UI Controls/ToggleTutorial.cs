using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class ToggleTutorial : MonoBehaviour {

	public Button btn;
	public bool toggle;

	void Start () {
		btn.onClick.AddListener (taskOnClick);
		toggle = false;
		gameObject.SetActive(false);
	}

	public void taskOnClick(){
		toggle = !(toggle);
		if(toggle){
			gameObject.SetActive(true);
			GetComponent<CanvasGroup>().alpha = 1;
		}
		else{
			FadeCanvas();
			//gameObject.SetActive(false);
		}

	}

	public void FadeCanvas(){
		StartCoroutine (DoFade());
	}

	IEnumerator DoFade (){
		CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= Time.deltaTime * 2;
			yield return null;
		}
		gameObject.SetActive(false);
		canvasGroup.interactable = false;
		yield return null;
	}
}
