using UnityEngine;
using System.Collections;

public class AlexMovementMoonBoots : BaseMovementAbility {

	//a boolean that checks if Alex is doing a super jump
	private bool isSuperJump;
	//value of super moon boot jump
	private const float MOON_BOOTS_JUMP = 5.0f;

	// Use this for initialization
	void Start () {
		//The isSuperJump starts as false and then calls the base start of BaseMovementAbility
		isSuperJump = false;
		base.Start ();

	}
	
	// Update is called once per frame
	void Update () {

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
		base.Update ();
	
	}
	
	//HeldAirMovement needs to be overriden because the values of held air movement of Alex is Higher than anyone else's
	protected override void HeldAirMovement ()
	{
		//When the m_VerticalVolicty is greater than MAX_FALL_SPEED (which it is from the start). A downwards force is applied. (Same as BaseMovementAbility script).
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{

			m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		}

		//If isSuperJump is true, the equation for his upwards velocity is increased by the value of MOON_BOOTS_JUMP). Currently the way BaseMovementAbility works,
		//isSuper will be true the second the button is pressed, so Alex will have a slightly higher jump than other players even though the button is only tapped
		if (isSuperJump == true)
		{
			m_CharacterController.Move (transform.up * (m_VerticalVelocity + MOON_BOOTS_JUMP) * Time.deltaTime);
		}

		//if isSuperJump is False then is does exactly what the BaseMovementAbility is sending the player upwards.
		else
		{
			m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
		}
	}
}
