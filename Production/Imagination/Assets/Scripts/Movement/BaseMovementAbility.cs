using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BaseMovementAbility : MonoBehaviour 
{
	public CharacterController m_CharacterController;
	protected float m_VerticalVelocity;
	protected const float JUMP_SPEED = 15.0f;
	protected const float MAX_FALL_SPEED = 15.0f;
	protected const float FALL_ACCELERATION = 2.0f;
	private bool m_CurrentlyJumping;

	void Start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		m_VerticalVelocity = 0.0f;
		m_CurrentlyJumping = false;
	}

	void Update () 
	{
		if(InputManager.getJumpUp)
		{
			m_CurrentlyJumping = false;
		}

		if(GetIsGrounded())
		{
			if(InputManager.getJumpDown)
			{
				Jump();
				m_CurrentlyJumping = true;
			}
			else
			{
				m_VerticalVelocity = 0.0f;
			}
		}
		else
		{
			if(InputManager.getJump && m_CurrentlyJumping == true)
			{
				AirMovement();
			}
			else
			{
				m_VerticalVelocity = 0.0f;
			}
		}
	}

	protected abstract void Jump()
	{
		m_VerticalVelocity = JUMP_SPEED;
	}

	protected abstract void AirMovement()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			m_VerticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
		}

		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	public bool GetIsGrounded()
	{
		return m_CharacterController.isGrounded;
	}
}
