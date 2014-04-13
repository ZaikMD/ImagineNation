/*


4/13/2014
	Now has a cape object in game
	Cape flows in the wind
*/ 


using UnityEngine;
using System.Collections;

public class Cape : SecondairyBase 
{
	public Transform m_Cape;
	Vector3 m_Rotation = new Vector3(40.0f,0.0f,0.0f);
	
	void Update ( )
	{
		//Ground
		if ( m_PlayerMovement.IsGrounded() )
		{
			//Cape flows in the wind
			if (m_PlayerMovement.getControllerProjection() != Vector3.zero)
			{
				m_Cape.Rotate (Mathf.Sin(Time.realtimeSinceStartup * 8.0f) * 0.03f * m_Rotation);
			}
			else
			{
				m_Cape.Rotate (Mathf.Sin(Time.realtimeSinceStartup) * 0.005f * m_Rotation);
			}

			if ( m_Enabled )
			{
				m_Enabled = false;    //If you are on the ground disable the cape
				m_PlayerMovement.DisableCape();
				m_Cape.Rotate (-m_Rotation);
				gameObject.GetComponent<ZoeyPlayerState>().setExitingSecond(true);
			}
			return;
		}


		//Airborne
		if ( m_Enabled )
		{
			//Cape flows in the wind
			m_Cape.Rotate (Mathf.Sin(Time.realtimeSinceStartup * 10.0f) * 0.025f * m_Rotation);

			//Turn cape off
			if ( PlayerInput.Instance.getJumpHeld() == false )
			{
				m_Enabled = false;
				m_PlayerMovement.DisableCape();
				m_Cape.Rotate (-m_Rotation);
				gameObject.GetComponent<ZoeyPlayerState>().setExitingSecond(true);
			}
		}
	}

	/// <summary>
	/// Move the player according to this item's interaction
	/// </summary>
	public override void Move ()
	{
		//
	}

	/// <summary>
	/// Returns wether or not the Item can be used
	/// </summary>
	/// <returns><c>true</c>, if to be used was abled, <c>false</c> otherwise.</returns>
	public override bool ableToBeUsed ()
	{
		if (m_PlayerMovement.IsGrounded() == true || m_PlayerMovement.getVerticleSpeed() >= PlayerMovement.JUMP_SPEED - 1.0f)
		{
			return false;
		}
		else 
		{
			return true;
		}
	}

	/// <summary>
	/// Starts the gliding.
	/// </summary>
	public void StartGliding()
	{
		m_Enabled = true;
		m_PlayerMovement.ActivateCape ();
		m_PlayerMovement.setCanMove (true);
		m_Cape.Rotate (m_Rotation);
	}

}
