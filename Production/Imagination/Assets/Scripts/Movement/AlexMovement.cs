/*
 * Created by Greg
 * Date: 10/9/2014
 *  
 * 
 * This class is used by Alex, giving him a much larger jump height.
 * 
 * Overrides the base jump function to use a different constant
 * 
 * 
 * 19/9/2014 - Changed to currectly use the new base class functionality - Jason Hein
 * 27/11/2014 - Added getter function for jumping and falling variables - Jason Hein
 */


using UnityEngine;
using System.Collections;

//Normal movement with a double jump
public class AlexMovement : BaseMovementAbility 
{
    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

    const int MAX_AIR_JUMPS = 1;
    int m_TotalAirJumps = 0;

	float m_TimeinAir;

	// Initialization
	void Start () {
		base.start ();
	}
	// Just calls the base update
	void Update ()
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		//then calls base Update script from BaseMovementAbility
		base.UpdateVelocity ();	
	}

    protected override void AirMovement()
    {
		if(GetIsGrounded())
		{
			if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
			{
				//m_SFX.playSound(this.transform, Sounds.Collectable);
				m_SFX.playSound(this.transform, Sounds.AlexJump);
				Jump();
			}
		}
		else if(m_TotalAirJumps < MAX_AIR_JUMPS)//gets plus one since the input to jump will get read again when the jump is started
        {//jump if needed
            if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
            {
			//	m_SFX.playSound(this.transform, Sounds.Collectable);
				m_SFX.playSound(this.transform, Sounds.AlexJump);
				Jump();
                m_TotalAirJumps++;
            }
        }        

        base.AirMovement();
    }

    protected override void GroundMovement()
    {
        //we know were grounded here so we can reset our counter
        m_TotalAirJumps = 0;
        base.GroundMovement();
    }

	public override void CallBack(CallBackEvents callBack)
	{
		switch(callBack)
		{
		case CallBackEvents.FootStep_Alex:
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
