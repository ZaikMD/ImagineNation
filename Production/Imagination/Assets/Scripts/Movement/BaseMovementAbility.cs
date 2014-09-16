//
//BaseMovement
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

//BaseMovement will require a character controller in order to
//move the player accordingly, and it will require AcceptInputFrom, a class
//that will determine from where the input is being recieved, either keyboard
//or one of the four possible gamepads.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AcceptInputFrom))]
public abstract class BaseMovementAbility : MonoBehaviour 
{
	public CharacterController m_CharacterController;
	public float m_Speed;
	private Transform m_Camera;
	private Animation m_Anim;

	protected float m_VerticalVelocity;
	protected const float JUMP_SPEED = 15.0f;
	protected const float MAX_FALL_SPEED = -15.0f;
	protected const float FALL_ACCELERATION = 20.0f;
	protected const float HELD_FALL_ACCELERATION = 10.0f;
	protected const float GRAVITY = -10.0f;
	protected bool m_CurrentlyJumping;
	private bool m_StartRayCasting;

	protected AcceptInputFrom m_AcceptInputFrom;

	protected void Start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		m_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		m_Anim = GetComponent<Animation>();

		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();

		m_VerticalVelocity = 0.0f;
		m_CurrentlyJumping = false;
		m_StartRayCasting = true;

	}

	//The default update all jumpining characters should use
	protected void Update () 
	{
		Movement ();
        PlayAnimation();

		//If at any point the jump button is released the player is
		//no longer currently jumping
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_CurrentlyJumping = false;
		}

		//If the player is grounded based on the character controller's
		//IsGrounded then start raycasting
		if(GetIsGrounded())
		{
			m_StartRayCasting = true;
		}


		//If the player should start raycasting
		if(m_StartRayCasting)
		{
			//Raycast downward for a collision with any object. This is essentially a second grounded check
			//however is avoids using the character controller's built in raycasting
			if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 2.0f))
			{	
				//If the jump input is pressed then set the vertical velocity,
				//call air movement to move the character, and stop raycasting
				if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
				{
					Jump();
					m_CurrentlyJumping = true;
					AirMovement();
					m_StartRayCasting = false;
				}
				else
				{
					//If the character is raycasting, they must be on the ground and if the jump input hasnt been 
					//pressed then ensure that the vertical velocity is zero.
					m_VerticalVelocity = 0.0f;
				}
				//We need to apply some sort of gravity because if the raycast returns true
				//and the jump wasnt pressed then we still need to move the character downward.
				Gravity();
			}
			else
			{
				AirMovement();
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

	protected void Movement()
	{
		
		//if we do not have any values, no need to continue
		if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) == Vector2.zero)
		{
			return;
		}
		
		//First we look at the direction from GetProjection, our forward is now that direction, so we move forward. 
		transform.LookAt(transform.position + GetProjection());
		m_CharacterController.Move(transform.forward * m_Speed * Time.deltaTime);
	}

	//This function get a vector3 for the direction we should be facing based of off the camera.
	Vector3 GetProjection()
	{
		Vector3 projection = m_Camera.forward * InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y;
		projection += m_Camera.right * InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x;
		
		projection.y = 0;
		return projection.normalized;
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

	private void Gravity()
	{
		m_CharacterController.Move (transform.up * GRAVITY * Time.deltaTime);
	}

    void PlayAnimation()
    {
        if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) == Vector2.zero)
        {
            m_Anim.Play("Idle");
            return;
        }

        if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x < 0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x > -0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y < 0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y > -0.3f)
        {

            m_Anim.Play("Walk");
            return;
        }

        m_Anim.Play("Run");


    }
	
}
