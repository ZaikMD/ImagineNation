using UnityEngine;
using System.Collections;

public class ButtonPickCharacter : MenuButton 
{		
	//public Characters CharacterBeingPicked;
	public string SceneToLoad = "";

	protected override void start ()
	{
	}

	protected override void update ()
	{

	}

	protected override void highlightedState ()
	{

	}

	protected override void defaultState ()
	{

	}

	public override void use ()
	{
		//GameData.Instance.SelectedCharacter = CharacterBeingPicked;

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
