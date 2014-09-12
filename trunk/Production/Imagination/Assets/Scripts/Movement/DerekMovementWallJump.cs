using UnityEngine;
using System.Collections;

public class DerekMovementWallJump : BaseMovementAbility
{
	private const float MAX_WALL_HANG = 2.0f;
	private const float WALL_MAX_FALL_SPEED = -5.0f;
	private const float WALL_JUMP_SPEED_VERTICAL = 25.0f;
	private float m_WallHangTimer = 0.0f;
	private GameObject[] m_Player;

	private bool m_OnWall = false;

	// Use this for initialization
	void Start () 
	{
		m_Player = GameObject.FindGameObjectsWithTag ("Player");

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
				m_WallHangTimer = 0.0f;
				m_OnWall = false;
			}
			else
			{
				if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
				{
					JumpOffWall ();
					m_OnWall = false;
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
		if(m_VerticalVelocity > WALL_MAX_FALL_SPEED)
		{
			m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		}
		
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	void OnControllerColliderHit(ControllerColliderHit other)
	{
		if(other.gameObject.CompareTag("Wall") && !m_CharacterController.isGrounded)
		{
			m_OnWall = true;
		}
	}

	void JumpOffWall()
	{
		m_VerticalVelocity = WALL_JUMP_SPEED_VERTICAL;
		transform.Rotate(0.0f, 180.0f, 0.0f);
	}
}
