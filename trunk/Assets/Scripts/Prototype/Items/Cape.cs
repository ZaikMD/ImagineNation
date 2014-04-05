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

	/// <summary>
	/// Move the player according to this item's interaction
	/// </summary>
	public override void Move ()
	{
		m_PlayerMovement.Glide(); 
	}

	/// <summary>
	/// Returns wether or not the Item can be used
	/// </summary>
	/// <returns><c>true</c>, if to be used was abled, <c>false</c> otherwise.</returns>
	public override bool ableToBeUsed ()
	{
		if (m_PlayerMovement.IsGrounded() == true)
						return false;
		else 
			return true;
	}

	/// <summary>
	/// Starts the gliding.
	/// </summary>
	public void StartGliding()
	{
		m_Enabled = true;
	}

}
