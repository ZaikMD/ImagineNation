//
//ZoeMovement
//Created by: Matthew Whitlaw
//
//Allows the player to glide while airborne with a second and held jump,
// by reducing the maximum speed that you can fall.
//

#region Change Log
/*19/9/2014 - Changed to currectly use the new base class functionality. - Jason Hein
 * 27/11/2014 - Added getter function for jumping and falling variables. - Jason Hein
 * 				Zoe now overrides a function GetVerticalMovementAfterFalling to set the players vertical gliding velocity.
 * 4/12/2014 - Changed gliding to only check jumping input while airborne. - Jason Hein
 * 
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

//Adds the ability to fall slower while airborne with some player input
public class ZoeMovement : BaseMovementAbility
{
	//After exiting glide, we enter a cannot glide state
	public bool m_CanGlide;

	//Int to keep track if we are entering or exiting glide based off of jump input
	private int m_NumberOfAirborneJumps;

	//Timer for how long we can glide
	public float m_Timer;
	private const float MAX_GLIDE_TIME = 2.0f;

	//Fall speeds
	private const float GLIDE_MAX_FALL_SPEED = -1.5f;
	private const float GLIDE_LERP_SPEED_PRE_DELTA = 2.4f;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Call the base start function and initialize all variables
	void Start () 
	{
		base.start ();
		m_CanGlide = true;
		m_NumberOfAirborneJumps = 0;
		m_Timer = -2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		//When grounded ensure that the variables CanGlide,
		//NumberOfJumps, and IsGliding are set appropriately
		if(GetIsGrounded())
		{
			if (m_Timer > 0.0f)
			{
				//Allow the player to glide again
				m_Timer = -MAX_GLIDE_TIME;
			}
			m_CanGlide = true;
			m_NumberOfAirborneJumps = 0;
		}

		//When the jump input is pressed increment the number of jumps and check how many jumps have been recieved
		if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
		{
			m_NumberOfAirborneJumps++;

			//When you press the jump button in the air, start gliding
			if(m_NumberOfAirborneJumps == 1 && m_CanGlide == true)
			{
				m_Timer = MAX_GLIDE_TIME;
				m_AnimatorController.playAnimation(AnimatorPlayers.Animations.Ability);
			}

			//On the third jump we exit gliding
			else if(m_NumberOfAirborneJumps >= 2)
			{ 
				StopGlidingWhileAirborne();
			}
		}
		//If we are gliding
		if (m_Timer > 0)
		{
			//If we are not holding down the button, exit gliding
			if(!InputManager.getJump(m_AcceptInputFrom.ReadInputFrom))
			{
				StopGlidingWhileAirborne();
			}
			//Otherwise set gliding based falling variables
			else
			{
				m_Timer -= Time.deltaTime;
			}
		}
		//If we have just exited glidings max timer
		else if (m_Timer > -1.0f)
		{
			StopGlidingWhileAirborne();
		}
		base.UpdateVelocity ();
	}

	//Set all gliding variables to a standard airborne state
	void StopGlidingWhileAirborne()
	{
		m_Timer = -MAX_GLIDE_TIME;
		m_CanGlide = false;
	}


	/// <summary>
	/// Returns a value for vertical movement after decelleration due to falling is calculated.
	/// </summary>
	protected override float GetVerticalMovementAfterFalling()
	{
		//Add our horizontal movement to our move
		float verticalVelocity = m_Velocity.y;

		if (m_Timer > -1.0f)
		{
			if (verticalVelocity != GLIDE_MAX_FALL_SPEED)
			{
				verticalVelocity = Mathf.Lerp(verticalVelocity, GLIDE_MAX_FALL_SPEED, Mathf.Min(Time.deltaTime * GLIDE_LERP_SPEED_PRE_DELTA, 1.0f));
			}
		}
		else
		{
			verticalVelocity = base.GetVerticalMovementAfterFalling();
		}
		return verticalVelocity;
	}

	public override void CallBack(CallBackEvents callBack)
	{
		switch(callBack)
		{
		case CallBackEvents.FootStep_Zoe:
			//Play footstep sound.
			if(!m_IsPlayingSound)
			{
				m_SFX.playSound(this.transform, Sounds.Run);
				m_IsPlayingSound = true;
			}			
			break;
		}	
	}
}
