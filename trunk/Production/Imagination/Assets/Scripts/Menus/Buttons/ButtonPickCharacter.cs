using UnityEngine;
using System.Collections;

public class ButtonPickCharacter : MenuButton 
{		
	public Enums.Players PlayerToSet = Enums.Players.PlayerOne;
	public Enums.Characters CharacterBeingPicked = Enums.Characters.Zoey;

	protected override void start ()
	{
	}

	protected override void update ()
	{
		if(PlayerToSet == Enums.Players.PlayerOne)
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
		else if(PlayerToSet == Enums.Players.PlayerTwo)
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
		if(PlayerToSet == Enums.Players.PlayerOne)
		{
			GameData.Instance.PlayerOneCharacter = CharacterBeingPicked;
		}
		else if(PlayerToSet == Enums.Players.PlayerTwo)
		{
			GameData.Instance.PlayerTwoCharacter = CharacterBeingPicked;
		}
	}
}
