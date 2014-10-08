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
