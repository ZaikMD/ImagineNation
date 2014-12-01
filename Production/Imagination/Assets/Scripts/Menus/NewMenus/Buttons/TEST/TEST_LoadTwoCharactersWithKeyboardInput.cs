using UnityEngine;
using System.Collections;

public class TEST_LoadTwoCharactersWithKeyboardInput : ButtonV2 
{
	public Characters i_PlayerOne;
	public Characters i_PlayerTwo;

	public string i_SceneToLoad = "LoadingScreen";

	public override void use (PlayerInput usedBy)
	{
		GameData.Instance.PlayerOneCharacter = i_PlayerOne;
		GameData.Instance.PlayerTwoCharacter = i_PlayerTwo;

		GameData.Instance.m_PlayerOneInput = PlayerInput.Keyboard;
		GameData.Instance.m_PlayerTwoInput = PlayerInput.Keyboard;

		Application.LoadLevel (i_SceneToLoad);
	}
}
