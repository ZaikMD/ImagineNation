/*
*AutoContinueSplashScreen
*
*resposible for imediately setting the next menu as the current menu and disableing the option of back tracking to this menu
*
*suitable for continueing to a menu system in game (pause menu)
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

public class AutoContinueSplashScreen : Menu 
{
    //this is literally just a starting point for a menu flow
    //you cannot go back to this menu

	public Menu StartMenu;
	// Use this for initialization
	protected override void start ()
	{
		if(StartMenu == null)
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("No Start Menu set");
			#endif
		}
	}
	
	// Update is called once per frame
	public override void update () 
	{
		base.update();

		MenuManager.Instance.changeMenu(StartMenu);

		//dont want the user tobe able to go back to this "menu"
		StartMenu.LastMenu = null;
	}
	
	protected override void changeSelection ()
	{
		
	}
	
	protected override void useButton ()
	{
		
	}
}
