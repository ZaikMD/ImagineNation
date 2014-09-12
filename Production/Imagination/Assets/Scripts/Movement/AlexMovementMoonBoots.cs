using UnityEngine;
using System.Collections;

public class AlexMovementMoonBoots : BaseMovementAbility {


	private bool isSuperJump;
	private const float MOON_BOOTS_JUMP = 5.0f;
	// Use this for initialization
	void Start () {
		isSuperJump = false;
		base.Start ();

	}
	
	// Update is called once per frame
	void Update () {

		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			isSuperJump = false;
		}
		else 
		{
			if(InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_CurrentlyJumping == true)
			{
				isSuperJump = true;
			}
		}
		base.Update ();
	
	}
	

	protected override void HeldAirMovement ()
	{
		//isSuperJump = false;
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{

			m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		}

		if (isSuperJump == true)
		{
			m_CharacterController.Move (transform.up * (m_VerticalVelocity + MOON_BOOTS_JUMP) * Time.deltaTime);
		}

		else
		{
			m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
		}
	}
}
