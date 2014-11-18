using UnityEngine;
using System.Collections;

/*
 * Created by: Kole
 * 
 * in menu button to exit the game 
 */

public class ButtonQuitGame : MenuButton 
{
	//if button is pressed, quit the level
	public override void use ()
	{
		Application.Quit();
	}
}
