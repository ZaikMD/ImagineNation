using UnityEngine;
using System.Collections;

public class ButtonPickCharacter : MenuButton 
{		
	public Players PlayerToSet = Players.PlayerOne;
	public Characters CharacterBeingPicked = Characters.Zoey;

	protected override void start ()
	{
	}

	protected override void update ()
	{
		if(PlayerToSet == Players.PlayerOne)
		{
			if(CharacterBeingPicked == GameData.Instance.PlayerOneCharacter)
			{
				ButtonState = ButtonStates.Disabled;
			}
			else
			{
				ButtonState = ButtonStates.Default;
			}
		}
		else if(PlayerToSet == Players.PlayerTwo)
		{
			if(CharacterBeingPicked == GameData.Instance.PlayerTwoCharacter)
			{
				ButtonState = ButtonStates.Disabled;
			}
			else
			{
				ButtonState = ButtonStates.Default;
			}
		}
	}

	protected override void highlightedState ()
	{
	}

	protected override void defaultState ()
	{
	}

	protected override void disabledState()
	{
	}

	public override void use ()
	{
		if(PlayerToSet == Players.PlayerOne)
		{
			GameData.Instance.PlayerOneCharacter = CharacterBeingPicked;
		}
		else if(PlayerToSet == Players.PlayerTwo)
		{
			GameData.Instance.PlayerTwoCharacter = CharacterBeingPicked;
		}
	}
}
