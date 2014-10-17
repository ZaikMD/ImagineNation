/*
*BaseMovement
*
*Responsible for basic vertical and horizontal movement and it is to be 
*inherited by the specific movement abilities for each character
*
*Created by: Matthew Whitlaw, Joe Burchill, Greg Fortier
*/

#region ChangeLog
/*
* 15/09/14 Edit: Fully Commented - Matthew Whitlaw.
*
* 24/09/14 Edit: Added PlatformMovement and Additional functionality
* to IsGrounded - Matthew Whitlaw
* 
* 16/10/2014 fixed moving platforma and added functionality for pushers and other various bugs - Kris Matis
*/
#endregion



using UnityEngine;
using System.Collections;

//BaseMovement will require a character controller in order to
//move the player accordingly, and it will require AcceptInputFrom, a class
//that will determine from where the input is being recieved, either keyboard
//or one of the four possible gamepads.
[RequireComponent(typeof(AnimationState))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AcceptInputFrom))]
[RequireComponent(typeof(AudioSource))]
public abstract class BaseMovementAbility : MonoBehaviour
{
	//Other objects this class needs
	public Transform m_Camera;
	public Animation m_Anim;
	protected AnimationState m_AnimState;
    protected SFXManager m_SFX;
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

	protected const float AIR_DECCELERATION_LERP_VALUE = 0.02f;

	//Distances
	protected const float GETGROUNDED_RAYCAST_DISTANCE = 0.135f;
	protected float m_PercentageOfVectorUp = GETGROUNDED_RAYCAST_DISTANCE - 0.1f;

	//movement that other classes have requested
	protected Vector3 m_ExternalMovement = Vector3.zero;


	//States
	protected bool m_CurrentlyJumping;
	protected bool m_IsOnMovingPlatform;


	//Called at the start of the program
	protected void Start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		//m_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		m_Anim = GetComponent<Animation>();

		m_AnimState = GetComponent<AnimationState>();

        m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();

		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();

		m_VerticalVelocity = -1.0f;
		m_CurrentlyJumping = false;
		m_IsOnMovingPlatform = false;

	}

	//The default update all characters should use
	protected void Update () 
	{
		//Plays a walking animation
      //  PlayAnimation();

		//If at any point the jump button is released the player is
		//no longer currently jumping
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_CurrentlyJumping = false;
		}
		GetIsGrounded ();
		//if(IsOnMovingPlatform ())
		{
			//PlatformMovement();
		}
		IsOnMovingPlatform ();
		externalMovement ();

		//If the player is grounded 
		if(GetIsGrounded())
		{

			//Check if we should start jumping
			if(InputManager.getJumpDown(m_AcceptInputFrom.ReadInputFrom))
			{
				m_AnimState.m_Grounded = false;
				Jump();
				AirMovement();
			}
			//Otherwise do normal ground movement, and reset our air movement
			else
			{
				m_AnimState.m_Grounded = true;
            	m_HorizontalAirVelocity = Vector2.zero;
				GroundMovement();
			}
		}
		//If we are not on the ground, we must be airborne, so do air movement
		else
		{
			m_AnimState.m_Grounded = false;
			AirMovement();			
		}

		m_Anim.Play (m_AnimState.GetAnimation());
	}

	public void requestMovement(Vector3 movement)
	{
		//add the requested movement to the current stored movement
		m_ExternalMovement += movement;
	}

	protected void externalMovement()
	{
		if(m_ExternalMovement.magnitude != 0.0f)
		{
			m_CharacterController.Move (m_ExternalMovement);
			m_ExternalMovement = Vector3.zero;
		}
	}

	//Moves the player based on the facing angle of the camera and the players input
	protected void GroundMovement()
	{
		OnGroundAnimLogic ();
		//if we do not have any values, no need to continue
		if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) == Vector2.zero)
		{
			return;
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


		Vector3 moveTo = transform.forward * m_GroundSpeed * Time.deltaTime;
		moveTo.y = m_VerticalVelocity;

		//First we look at the direction from GetProjection, our forward is now that direction, so we move forward. 
		transform.LookAt(transform.position + GetProjection());
		m_CharacterController.Move(moveTo);
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
		m_AnimState.AddAnimRequest (AnimationStates.Jump);
		m_AnimState.m_Jumping = true;
		m_VerticalVelocity = JUMP_SPEED;
		m_CurrentlyJumping = true;


		switch(this.gameObject.name)
		{
		case Constants.ALEX_WITH_MOVEMENT_STRING:
			m_SFX.playSound(this.gameObject, Sounds.AlexJump);
			break;
		case Constants.DEREK_WITH_MOVEMENT_STRING:
			m_SFX.playSound(this.gameObject, Sounds.DerekJump);
			break;
		case Constants.ZOE_WITH_MOVEMENT_STRING:
			m_SFX.playSound(this.gameObject, Sounds.ZoeyJump);
			break;
		}

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

	/*15/09/14 Edit: Added TrampolineJump script() - Greg Fortier
	*
	*/
	//Moves the player upwards when the function is called;
	public void TrampolineJump()
	{
		//Plays sound and animation when jumping on trampoline.
		m_SFX.playSound(this.gameObject, Sounds.JumpPad);
		m_AnimState.AddAnimRequest(AnimationStates.Jump);

		m_VerticalVelocity = 15.0f;
		Vector3 Movement = new Vector3 (0, (m_VerticalVelocity), 0);

		//Starts decreasing the velocity so that the player does not keep flying upwards
		if(m_VerticalVelocity > BASE_MAX_FALL_SPEED)
		{
			m_VerticalVelocity-= Time.deltaTime * FALL_ACCELERATION;
		}

		//Moves the player
		if(m_VerticalVelocity >=0)
		{
			m_CharacterController.Move(Movement * Time.deltaTime);
		}

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
		//if(!IsOnMovingPlatform())
		m_AnimState.AddAnimRequest (AnimationStates.Falling);

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
		else
		{
			m_HorizontalAirVelocity = Vector2.Lerp(m_HorizontalAirVelocity, Vector2.zero, AIR_DECCELERATION_LERP_VALUE);
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


		m_CharacterController.Move (Movement * Time.deltaTime);
		


	}

	//Getter for if the character is grounded based on character controller
	//
	//If we are supposed to still be grounded but aren't according to our character controller, we are still considered grounded due to a raycast downwards
	public bool GetIsGrounded()
	{
		RaycastHit hit;

		if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f) && m_VerticalVelocity == 0.0f) || m_CharacterController.isGrounded)
		{
			if(m_VerticalVelocity < 0.0f)
			{
				m_VerticalVelocity = 0.0f;
			}
			return true;
		}
		return false;

	}

	bool IsOnMovingPlatform()
	{
		RaycastHit hitInfo;
		return IsOnMovingPlatform (out hitInfo);
	}

	bool IsOnMovingPlatform(out RaycastHit hitInfo)
	{
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, 0.5f))
		{
			if(hitInfo.transform != null)
			{
				if(hitInfo.collider.gameObject.tag == Constants.MOVING_PLATFORM_TAG_STRING)
				{
					m_IsOnMovingPlatform = true;
					m_Platform = hitInfo.transform.gameObject.GetComponent<ActivatableMovingPlatform>();
					PlatformMovement();
					return true;
				}
				else
				{
					m_IsOnMovingPlatform = false;
				}
			}
		}
		return false;
	}

	void PlatformMovement()
	{
		requestMovement (m_Platform.GetAmountToMovePlayer ());
		//m_CharacterController.Move (m_Platform.GetAmountToMovePlayer());
		//m_CharacterController.Move (Vector3.down);
	}

	//Plays a walking animation
    void OnGroundAnimLogic()
    {
        if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) == Vector2.zero)
        {
			m_AnimState.AddAnimRequest(AnimationStates.Idle);           
        }
		else if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x < 0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x > -0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y < 0.3f
            && InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y > -0.3f)
        {
			m_AnimState.AddAnimRequest(AnimationStates.Walk);
			return;
        }
		else
		{
			m_AnimState.AddAnimRequest(AnimationStates.Run);
		}        
	}	
}