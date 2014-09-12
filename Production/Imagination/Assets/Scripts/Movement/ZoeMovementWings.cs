using UnityEngine;
using System.Collections;

public class ZoeMovementWings : BaseMovementAbility {

	private bool m_CanGlide;
	private bool m_IsGliding;
	private int m_NumberOfJumps;
	private const float GLIDE_FALL_ACCELERATION = 2.0f;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
		m_CanGlide = true;
		m_IsGliding = false;
		m_NumberOfJumps = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{  

		if(GetIsGrounded())
		{
			m_CanGlide = true;
			m_NumberOfJumps = 0;
		}

		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_NumberOfJumps++;
			if(m_NumberOfJumps == 2)
			{
				m_IsGliding = true;
			}
			else if(m_NumberOfJumps >= 3)
			{
				m_CanGlide = false;
				m_IsGliding = false;
			}
		}

		if(m_IsGliding && InputManager.getJump(m_AcceptInputFrom.ReadInputFrom))
		{
			HeldAirMovement();
		}
		else
		{
			base.Update ();
		}
	}

	protected override void HeldAirMovement()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		}
		
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}


}
