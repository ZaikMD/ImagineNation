/*
*SplashScreen
*
*a start point for the menu system that only has functionality for coninueing
*
*
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class SplashScreen : Menu 
{
	public Menu MainMenu;

	public bool IsRotatingMenu = false;

	// Use this for initialization
	protected override void start ()
	{
		if(MainMenu == null)
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("No Main Menu set");
			#endif
		}
	}
	
	// Update is called once per frame
	public override void update () 
	{
		if(InputManager.getJumpDown() || InputManager.getPauseDown())
		{
			if(MainMenu != null)
			{
				//go to the next menu
				MenuManager.Instance.changeMenu(MainMenu);
			}
		}
	}

	protected override void changeSelection ()
	{

	}

	protected override void useButton ()
	{

	}
}
