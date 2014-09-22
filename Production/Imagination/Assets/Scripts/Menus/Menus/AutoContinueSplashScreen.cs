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
			Debug.Log("No Start Menu set");
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
