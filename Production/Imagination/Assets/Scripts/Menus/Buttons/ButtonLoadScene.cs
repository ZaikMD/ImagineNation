/*
*ButtonLoadScene
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

public class ButtonLoadScene : MenuButton
{
	public string SceneToLoad = "";

	public override void use ()
	{
		if(SceneToLoad != "")
		{
			//load the next scene
			Application.LoadLevel(SceneToLoad);
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("No Scene Set to Load");
			#endif
		}
	}
}
