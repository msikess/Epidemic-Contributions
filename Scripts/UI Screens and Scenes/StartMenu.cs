using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//A simple script to redirect the player to the level selection
// scene by pressing the Start button;
public class StartMenu : MonoBehaviour
{

	public void Begin (int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
		//I can't display the part of the code that gathers the 
		// player ID and the group/class ID for the info that's 
		// later passed on to the server, because the code is not mine;
	}
		
}
