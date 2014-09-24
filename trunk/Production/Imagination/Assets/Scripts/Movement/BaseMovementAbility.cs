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
//24/09/14 Edit: Added PlatformMovement and Additional functionality
// to IsGrounded - Matthew Whitlaw

using UnityEngine;
using System.Collections;

//BaseMovement will require a character controller in order to
//move the player accordingly, and it will require AcceptInputFrom, a class
//that will determine from where the input is being recieved, either keyboard
//or one of the four possible gamepads.

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AcceptInputFrom))]
[RequireComponent(typeof(AudioSource))]
public abstract class BaseMovementAbility : MonoBehaviour 
{
	//Other objects this class needs
	private Transform m_Camera;
	public Animation m_Anim;
    private SFXManager m_SFX;
	protected CharacterController m_CharacterController;
	protected AcceptInputFrom m_AcceptInputFrom;
	protected ActivatableMovingPlatform m_Platform;

	//Speed can be set by designers
	public float m_GroundSpeed = 5.0f;
	public float m_AirHorizontalAcceleration = 5.0f;

	//Current velocity
	protected Vector2 m_HorizontalAirVelocity = Vector2.zero;
	protected float m_VerticalVelocity;
	protected Vector2 m_CurrentHorizontalAirVelocity;

	//Maximum speeds
	protected const float MAX_HORIZONTAL_AIR_SPEED = 5.0f;
	protected const float BASE_MAX_FALL_SPEED = -15.0f;
	protected float m_MaxFallSpeed = BASE_MAX_FALL_SPEED;

	//Acceleration
	protected const float JUMP_SPEED = 7.5f;
	protected const float FALL_ACCELERATION = 20.0f;
	protected const float HELD_FALL_ACCELERATION = 15.0f;

	//States
	protected bool m_CurrentlyJumping;
	protected bool m_IsOnMovingPlatform;

	//Called at the start of the program
	protected void Start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		m_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		m_Anim = GetComponent<Animation>();

        m_SFX = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SFXManager>();

		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();

		m_VerticalVelocity = -1.0f;
		m_CurrentlyJumping = false;
		m_IsOnMovingPlatform = false;
	}

	//The default update all characters should use
	protected void Update () 
	{
		//Plays a walking animation
        PlayAnimation();

		//If at any point the jump button is released the player is
		//no longer currently jumping
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_CurrentlyJumping = false;
		}

		//If the player is grounded
		if(GetIsGrounded())
		{
			//Check if we should start jumping
			if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
			{
				Jump();
				AirMovement();
			}
			//Otherwise do normal ground movement, and reset our air movement
			else
			{
				m_VerticalVelocity = 0.0f;
                m_HorizontalAirVelocity = Vector2.zero;
				GroundMovement();

				if(m_IsOnMovingPlatform){
					PlatformMovement();}

			}


			//Gravity();
		}
		//If we are not on the ground, we must be airborne, so do air movement
		else
		{
			AirMovement();
		}
	}

	//Moves the player based on the facing angle of the camera and the players input
	protected void GroundMovement()
	{
		
		//if we do not have any values, no need to continue
		if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) == Vector2.zero)
		{
			return;
		}
		
		//First we look at the direction from GetProjection, our forward is now that direction, so we move forward. 
		transform.LookAt(transform.position + GetProjection());
		m_CharacterController.Move(transform.forward * m_GroundSpeed * Time.deltaTime);
	}

	//Gets a vector3 for the direction we should be getting input based of off the cameras facing angle
	protected Vector3 GetProjection()
	{
		Vector3 projection = m_Camera.forward * InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y;
		projection += m_Camera.right * InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x;
		
		projection.y = 0;
		return projection.normalized;
	}

	//Sets the vertical velocity to a pre-determined jump speed, and our horizontal air movement to our current running speed
	protected virtual void Jump()
	{
		m_VerticalVelocity = JUMP_SPEED;
		m_CurrentlyJumping = true;

		//We are running, we set our horizontal air speed to our running speed
		if (InputManager.getMove() != Vector2.zero)
		{
			transform.LookAt(transform.position + GetProjection());
			m_HorizontalAirVelocity = transform.forward * m_GroundSpeed;
		}
		//If we are not running, our current horizontal speed is zero
		else
		{
			m_HorizontalAirVelocity = Vector2.zero;
		}
	}

	/// <summary>
	/// Launches a player in a given Vector direction, the magnitude of which will be the speed
	/// </summary>
	/// <param name="jump">Jump.</param>
	public virtual void LaunchJump(Vector3 jump)
	{
		m_VerticalVelocity = jump.y;
		jump.y = 0.0f;

		m_CurrentlyJumping = true;
		transform.LookAt(transform.position + jump.normalized);
		m_HorizontalAirVelocity = new Vector2(jump.x, jump.z);

	}

	//Moves the player in all three directions
	//
	//Horizontal movement is added first, and is based off the previous horizontal speed with a minor change based on controller input, giving the player
	//minor control horizontally while airborne.
	//
	//We next add vertical speed, which is the previous veritcal speed minus a small amount based on our set falling acceleration.
	//
	//Then we move the player
	protected virtual void AirMovement()
	{
		Vector3 Movement = new Vector3(m_HorizontalAirVelocity.x, 0, m_HorizontalAirVelocity.y);
		if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) != Vector2.zero)
		{
			//Calc new velocity based on input
			Movement = new Vector3(Movement.x + (GetProjection().x * m_AirHorizontalAcceleration * Time.deltaTime),
			                       0,
			                       Movement.z + (GetProjection().z * m_AirHorizontalAcceleration * Time.deltaTime));
		
			//Calc the direction to look and move
			m_HorizontalAirVelocity = new Vector2(Movement.x, Movement.z);
			transform.LookAt(transform.position + GetProjection());
			
			//Cap the horizontal movement speed
			float horizontalVelocityMagnitude = Mathf.Abs(m_HorizontalAirVelocity.magnitude);
			if (horizontalVelocityMagnitude >= MAX_HORIZONTAL_AIR_SPEED) ///Should be max speed
			{
				Movement = Movement.normalized * MAX_HORIZONTAL_AIR_SPEED;
				m_HorizontalAirVelocity = new Vector2(Movement.x, Movement.z);
			}
		}

		//Cap the vertical fall speed
		if(m_VerticalVelocity > m_MaxFallSpeed)
		{
			//Constantly decrease velocity based on time passed by an deceleration
			if(InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_CurrentlyJumping == true)
			{
				m_VerticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
			}
			else
			{
				m_VerticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
			}
		}

		//Add our vertical movement to our move
		Movement.y = m_VerticalVelocity;

		//Move the character
		m_CharacterController.Move (Movement * Time.deltaTime);

	}

	//Getter for if the character is grounded based on character controller
	//
	//If we are supposed to still be grounded but aren't according to our character controller, we are still considered grounded due to a raycast downwards
	public bool GetIsGrounded()
	{
		RaycastHit hit;

		Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.down), Color.magenta);

		if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.1f) && m_VerticalVelocity == 0.0f) || m_CharacterController.isGrounded)
		{
			if(hit.transform != null)
			{
				if(hit.collider.gameObject.tag == "MovingPlatform")
				{
					m_IsOnMovingPlatform = true;
					m_Platform = hit.transform.gameObject.GetComponent<ActivatableMovingPlatform>();
				}
				else
				{
					m_IsOnMovingPlatform = false;
				}
			}
			return true;
		}
		return false;
	}

	void PlatformMovement()
	{
		transform.position += m_Platform.GetAmountToMovePlayer ();
	}

	//Plays a walking animation
    void PlayAnimation()
    {
        if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) == Vector2.zero)
        {
            m_Anim.Play("Idle");
            if (m_SFX != null)
            {
                m_SFX.stopSound(this.gameObject);
            }
                return;
        }

        if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x < 0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x > -0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y < 0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y > -0.3f)
        {

            m_Anim.Play("Walk");
            if (m_SFX != null)
            {
                m_SFX.playSound(this.gameObject, Sounds.Walk);
            }
                return;
        }

        m_Anim.Play("Run");
        if (m_SFX != null)
        {
            m_SFX.playSound(this.gameObject, Sounds.Run);
        }
    }
	
}