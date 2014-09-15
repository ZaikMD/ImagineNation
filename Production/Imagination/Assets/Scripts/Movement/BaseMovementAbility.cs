//
//BaseMovementAbility
//
//Responsible for basic vertical movement and it is to be 
//inherited by the specific movement abilities for each character
//
//Created by: Matthew Whitlaw, Joe Burchill, Greg Fortier
//
//15/09/14 Edit: Fully Commented - Matthew Whitlaw.
//
//

using UnityEngine;
using System.Collections;

//BaseMovementAbility will require a character controller in order to
//move the player accordingly, and it will require AcceptInputFrom, a class
//that will determine from where the input is being recieved, either keyboard
//or one of the four possible gamepads.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AcceptInputFrom))]
public abstract class BaseMovementAbility : MonoBehaviour 
{
	public CharacterController m_CharacterController;
	protected float m_VerticalVelocity;
	protected const float JUMP_SPEED = 15.0f;
	protected const float MAX_FALL_SPEED = -15.0f;
	protected const float FALL_ACCELERATION = 20.0f;
	protected const float HELD_FALL_ACCELERATION = 10.0f;
	protected bool m_CurrentlyJumping;

	protected AcceptInputFrom m_AcceptInputFrom;

	protected void Start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		m_VerticalVelocity = 0.0f;
		m_CurrentlyJumping = false;

		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();
		Debug.Log (m_AcceptInputFrom);
	}

	//The default update all jumpining characters should use
	protected void Update () 
	{
		//If at any point the jump button is released the player is
		//no longer currently jumping
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_CurrentlyJumping = false;
		}

		//When the player is on the ground
		//if(GetIsGrounded())
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.0f))
		{
			//and the jump input is pressed then set the vertical velocity
			//and call air movement to move the character
			if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
			{
				Jump();
				m_CurrentlyJumping = true;
				AirMovement();
			}
			else
			{
				//If the character is on the ground and the jump input hasnt been 
				//pressed then ensure that the vertical velocity is zero.
				m_VerticalVelocity = 0.0f;
			}
		}
		else
		{
			//When the player is not grounded, then check if the jump input is being pressed and 
			//it hasnt been released, if so then call the held air movement for a higher jump
			if(InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_CurrentlyJumping == true)
			{
				HeldAirMovement();
			}
			else
			{
				//Otherwise call the regular air movement
				AirMovement();
			}
		}
	}

	//Simply setting the vertical velocity to a pre-determined speed
	protected virtual void Jump()
	{
		m_VerticalVelocity = JUMP_SPEED;
	}

	//Controls the vertical movement when off the ground
	protected virtual void AirMovement()
	{
		//Ensure that the vertical velocity cannot decrease infinitly
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			//Constantly decrease velocity based on time passed by an deceleration
			m_VerticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
		}

		//Once the appropriate velocity is determined move the character
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	//Controls the vertical movement when off the ground and the jump button is being held.
	protected virtual void HeldAirMovement()
	{
		if(m_VerticalVelocity > MAX_FALL_SPEED)
		{
			//When the button is being held decrease the amount of deceleration to allow
			//for a higher jump.
			m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
		}
		
		m_CharacterController.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
	}

	//Getter for if the character is grounded based on character controller
	public bool GetIsGrounded()
	{
		return m_CharacterController.isGrounded;
	}
}
