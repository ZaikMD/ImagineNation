using UnityEngine;
using System.Collections;

public class DerekMovementWallJump : BaseMovementAbility
{
	private const float MAX_WALL_HANG = 2.0f;
	private const float WALL_HANG_FALL_ACCELERATION = 3.0f;
	private float m_WallHangTimer = 0.0f;

	private bool m_OnWall = false;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_OnWall)
		{
			m_WallHangTimer += Time.deltaTime;
			HeldAirMovement();
			if(m_WallHangTimer >= MAX_WALL_HANG)
			{
				base.Update	();
			}
			else
			{
				if(InputManager.getJumpDown(/*m_AcceptInputFrom*/))
				{
					transform.Rotate(0.0f, 180.0f, 0.0f);
					Jump ();
				}
			}
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
			m_VerticalVelocity -= Time.deltaTime * WALL_HANG_FALL_ACCELERATION;
		}
		
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("Wall"))
		{
			m_OnWall = true;
		}
	}
}
