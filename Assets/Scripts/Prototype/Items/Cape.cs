using UnityEngine;
using System.Collections;

public class Cape : SecondairyBase 
{
			
	void Start ( )
		{
		}
	
	void Update ( )
	{
		//Ground
		if ( m_PlayerMovement.IsGrounded() )
		{
			if ( m_Enabled )
			{
				m_Enabled = false;    //If you are on the ground disable the cape
				m_PlayerMovement.setCanMove(true);
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
			}
		}
		else if ( PlayerInput.Instance.getJumpHeld() == true )
		{
			m_Enabled = true;    //Turn cape on
		}
	}

	public override void Move ()
	{
		m_PlayerMovement.Glide(); 
	}

}
