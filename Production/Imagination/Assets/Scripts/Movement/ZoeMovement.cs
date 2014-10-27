//
//ZoeMovement
//Created by: Matthew Whitlaw
//
//Allows the player to glide while airborne with a second and held jump,
// by reducing the maximum speed that you can fall.
//

#region Change Log
//19/9/2014 - Changed to currectly use the new base class functionality - Jason Hein
#endregion

using UnityEngine;
using System.Collections;

//Adds the ability to fall slower while airborne with some player input
public class ZoeMovement : BaseMovementAbility
{
	//After exiting glide, we enter a cannot glide state
	public bool m_CanGlide;

	//Int to keep track if we are entering or exiting glide based off of jump input
	private int m_NumberOfJumps;

	//Timer for how long we can glide
	public float m_Timer;
	private const float MAX_GLIDE_TIME = 2.0f;

	//Gliding fall speed
	private const float GLIDE_MAX_FALL_SPEED = -1.5f;


	// Call the base start function and initialize all variables
	void Start () 
	{
		base.start ();
		m_CanGlide = true;
		m_NumberOfJumps = 0;
		m_Timer = -2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{  
		//When grounded ensure that the variables CanGlide,
		//NumberOfJumps, and IsGliding are set appropriately
		if(GetIsGrounded())
		{
			if (m_Timer > 0.0f)
			{
				//Allow the player to glide again
				m_Timer = -2.0f;
				m_MaxFallSpeed = BASE_MAX_FALL_SPEED;
			}
			m_CanGlide = true;
			m_NumberOfJumps = 0;
		}

		//When the jump input is pressed increment the number of jumps and check how many jumps have been recieved
		if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
		{
			m_NumberOfJumps++;

			//Only on the second jump input recieved can Zoe use gliding
			if(m_NumberOfJumps == 2 && m_CanGlide == true)
			{
				m_MaxFallSpeed = GLIDE_MAX_FALL_SPEED;
				m_Timer = MAX_GLIDE_TIME;
			}

			//On the third jump we exit gliding
			else if(m_NumberOfJumps >= 3)
			{ 
				stopGlidingWhileAirborne();
			}
		}
		//If we are gliding
		if (m_Timer > 0)
		{
			//If we are not holding down the button, exit gliding
			if(!InputManager.getJump(m_AcceptInputFrom.ReadInputFrom))
			{
				stopGlidingWhileAirborne();
			}
			//Otherwise set gliding based falling variables
			else
			{
				m_Timer -= Time.deltaTime;
				m_VerticalVelocity = m_MaxFallSpeed - GetLaunchVelocity().y;
			}

			//Glide through the air
			AirMovement();
		}
		//If we have just exited glidings max timer
		else if (m_Timer > -1.0f)
		{
			stopGlidingWhileAirborne();
			base.update ();
		}
		//Otherwise we are not gliding, and we should move normally
		else
		{
			base.update ();
		}
	}

	//Set all gliding variables to a standard airborne state
	void stopGlidingWhileAirborne()
	{
		m_Timer = -2.0f;
		m_CanGlide = false;
		m_MaxFallSpeed = BASE_MAX_FALL_SPEED;
	}
}
