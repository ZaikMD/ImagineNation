//
//ZoeMovementAbility
//
//Responsible for the unique aspects of Zoe's vertical movement
//
//Created by: Matthew Whitlaw
//
//15/09/14 Edit: Fully Commented - Matthew Whitlaw.
//
//

using UnityEngine;
using System.Collections;

public class ZoeMovementWings : BaseMovementAbility {

	private bool m_CanGlide;
	private bool m_IsGliding;
	private int m_NumberOfJumps;
	private float m_Timer;
	private const float GLIDE_MAX_FALL_SPEED = -1.5f;
	private const float MAX_GLIDE_TIME = 4.0f;

	// Call the base start function and initialize all variables
	void Start () 
	{
		base.Start ();
		m_CanGlide = true;
		m_IsGliding = false;
		m_NumberOfJumps = 0;
		m_Timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{  
		//When grounded ensure that the variables CanGlide,
		//NumberOfJumps, and IsGliding are set appropriately
		if(GetIsGrounded())
		{
			m_CanGlide = true;
			m_NumberOfJumps = 0;
			m_IsGliding = false;
			m_Timer = 0.0f;
		}

		//When the jump input is pressed increment the number of jumps
		//and check how many jumps have been recieved
		if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
		{
			m_NumberOfJumps++;
			if(m_NumberOfJumps == 2 && m_CanGlide == true)
			{
				//Only on the second jump input recieved can Zoe use gliding
				m_IsGliding = true;
			}
			else if(m_NumberOfJumps >= 3)
			{
				//If the number of jump exceeds two and
				//Zoe is still off the ground disable the
				//ability to glide
				m_CanGlide = false;
				m_IsGliding = false;
			}
		}

		//When gliding is true and the button is being held call the overriden heldAirMovement
		if(m_IsGliding && InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_Timer < MAX_GLIDE_TIME)
		{
			m_Timer += Time.deltaTime;
			m_VerticalVelocity = GLIDE_MAX_FALL_SPEED;
			GlidingAirMovement();
		}
		else
		{
			//Otherwise just call the base update
     		base.Update ();
		}
	}

	//A function for the specific type of air movement for when Zoe is gliding
	void GlidingAirMovement()
	{
		//m_VerticalVelocity -= Time.deltaTime * GLIDE_FALL_ACCELERATION;
		
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}


}
