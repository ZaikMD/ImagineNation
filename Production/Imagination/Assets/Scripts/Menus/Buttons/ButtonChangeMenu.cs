using UnityEngine;
using System.Collections;

public class ButtonChangeMenu : MenuButton
{
	public Menu NewMenu = null;
	
	public override void use ()
	{
		if(NewMenu != null)
		{
			MenuManager.Instance.changeMenu(NewMenu);
		}
		else
		{
			Debug.Log("No Menu set");
		}
	}
}
