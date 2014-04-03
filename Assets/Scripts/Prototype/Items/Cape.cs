using UnityEngine;
using System.Collections;

public class Cape : SecondairyBase 
{
	
	void Update ( )
	{
		//Ground
		if ( m_PlayerMovement.IsGrounded() )
		{
			if ( m_Enabled )
			{
				m_Enabled = false;    //If you are on the ground disable the cape
				m_PlayerMovement.setCanMove(true);
				gameObject.GetComponent<ZoeyPlayerState>().setExitingSecond(true);
			}
			return;
		}


		//Airborne
		if ( m_Enabled )
		{
			  //Glide
			Move();

			//Turn cape off
			if ( PlayerInput.Instance.getJumpHeld() == false )
			{
				m_Enabled = false;
				m_PlayerMovement.setCanMove(true);
				gameObject.GetComponent<ZoeyPlayerState>().setExitingSecond(true);
			}
		}

	}

	public override void Move ()
	{
		m_PlayerMovement.Glide(); 
	}

	public override bool ableToBeUsed ()
	{
		if (m_PlayerMovement.IsGrounded() == true)
						return false;
		else 
			return true;
	}

	public void StartGliding()
	{
		m_Enabled = true;
	}

}
