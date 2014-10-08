/*
*ButtonPickCharacter
*
*resposible for setting a players selected character
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

public class ButtonPickCharacter : MenuButton 
{		
	//what player is being picked by this button
	public Players PlayerToSet = Players.PlayerOne;
	//what character is this button setting it to
	public Characters CharacterBeingPicked = Characters.Zoe;

	protected override void start ()
	{
	}

	protected override void update ()
	{
		//we need an update to set player twos character while player one is pickeing to avoid a dupicate selection

        //checks whitch player to set
		if(PlayerToSet == Players.PlayerOne)
		{
            //if the character is selected already set it to default
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
            //if the character is selected already set it to default
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
		//set the correct players character
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
