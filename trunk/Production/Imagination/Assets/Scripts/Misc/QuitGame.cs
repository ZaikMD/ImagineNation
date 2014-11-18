using UnityEngine;
using System.Collections;

/*
 * Created by: kole
 * 
 * this script is being used to quit game prior to the menu
 * being implemented.
 */

public class QuitGame : MonoBehaviour {
	// Update is called once per frame
	void Update ()
	{
		if(InputManager.getPause())
		{
			Application.Quit();
		}
	}
}
