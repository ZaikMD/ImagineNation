using UnityEngine;
using System.Collections;

public class ButtonLoadScene : MenuButton
{
	public string SceneToLoad = "";

	public override void use ()
	{
		if(SceneToLoad != "")
		{
			Application.LoadLevel(SceneToLoad);
		}
		else
		{
			Debug.Log("No Scene Set to Load");
		}
	}
}
