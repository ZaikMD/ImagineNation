/*
*ButtonChangeMenu
*
*resposible for loading the next scene
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

public class ButtonChangeMenu : MenuButton
{
	public Menu NewMenu = null;
	
	public override void use ()
	{
		if(NewMenu != null)
		{
			//tell menu manager to swap to the next menu
			MenuManager.Instance.changeMenu(NewMenu);
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("No Menu set");
			#endif
		}
	}
}
