﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AcceptInputFrom))]
public abstract class BaseMovementAbility : MonoBehaviour 
{
	public CharacterController m_CharacterController;
	protected float m_VerticalVelocity;
	protected const float JUMP_SPEED = 30.0f;
	protected const float MAX_FALL_SPEED = -30.0f;
	protected const float FALL_ACCELERATION = -20.0f;
	protected const float HELD_FALL_ACCELERATION = -10.0f;
	private bool m_CurrentlyJumping;

	protected AcceptInputFrom m_AcceptInputFrom;

	void Start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		m_VerticalVelocity = 0.0f;
		m_CurrentlyJumping = false;

		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();
	}

	protected void Update () 
	{
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_CurrentlyJumping = false;
		}

		if(GetIsGrounded())
		{
			if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
			{
				Jump();
				m_CurrentlyJumping = true;
				AirMovement();
			}
			else
			{
				m_VerticalVelocity = 0.0f;
			}
		}
		else
		{
			if(InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_CurrentlyJumping == true)
			{
				HeldAirMovement();
			}
			else
			{
				AirMovement();
			}
		}
	}

	protected virtual void Jump()
	{
		m_VerticalVelocity = JUMP_SPEED;
	}

	protected virtual void AirMovement()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			m_VerticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
		}

		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	protected virtual void HeldAirMovement()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		}
		
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	public bool GetIsGrounded()
	{
		return m_CharacterController.isGrounded;
	}
}
