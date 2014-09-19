/*
 * Created by Greg
 * Date: 
 *  
 * 
 * This class is used by Alex, giving him a much larger jump height.
 * 
 * Overrides the base jump function to use a different constant
 * 
 * 
 * 19/9/2014 - Changed to currectly use the new base class functionality - Jason Hein
 */


using UnityEngine;
using System.Collections;

//Normal movement with a higher jump
public class AlexMovementMoonBoots : BaseMovementAbility {
	
	//Super moon boot jump speed
	private const float MOON_BOOTS_JUMP = 15.0f;

	// Initialization
	void Start () {
		base.Start ();
	}
	
<<<<<<< .mine
	// Just calls the base update
	void Update ()
	{
=======
	// Update is called once per frame
	void Update () {


		if(!m_CanMove)
		{
			return;
		}


		//If The player releases the jump key then isSuperJump will turn to false
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{ 
			isSuperJump = false;
		}
		else 
		{
			//If the player did not release the jump key, it will check if the button is being held down. If yes then isSuperJump is true
			if(InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_CurrentlyJumping == true)
			{
				isSuperJump = true;
			}
		}
		//then calls base Update script from BaseMovementAbility
>>>>>>> .r585
		base.Update ();
	
	}

	//Jump higher than normal
	protected override void Jump()
	{
		base.Jump ();
		m_VerticalVelocity = MOON_BOOTS_JUMP;
	}
}
