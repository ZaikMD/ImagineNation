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
* 
* 24/10/2014 Edit: Now works with multiple timed launches, reduced control during initial launch, and added horiozntal launches. - Jason Hein
* 				   Fixed falling on hills bug.
* 27/10/2014 Edit: Fixed side jumping bug, and the bug related to gliding and trampolines.
* 27/10/2014 Edit: Cleaned up movement code, and increase difference between held air and non-held air acceleration.
* 				   Changed falling and jumping constants to getter function in inheriting classes.
* 				   Seperated falling calculation into a virtual function GetVerticalMovementAfterFalling()
* 28/10/2014 Edit: Changed a number of constants and added a setPausedMovement function.
* 
* 
* 
*/
#endregion



using UnityEngine;
using System.Collections.Generic;

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

	//Movement control
	protected const float AIR_HORIZONTAL_CONTROL = 25.0f;

	//Current velocity
	protected Vector3 m_Velocity = new Vector3(0.0f, -1.0f, 0.0f);

	//Maximum speeds
	protected const float MAX_GROUND_RUNSPEED = 5.0f;
	protected const float MAX_HORIZONTAL_AIR_SPEED = 5.5f;
	protected const float MAX_FALL_SPEED = -15.0f;
	protected const float SPEED_MINIMUM_VALUE_TO_CLAMP = 0.5f;

	//Acceleration
	protected const float FALL_ACCELERATION = 20.0f;
	protected const float HELD_FALL_ACCELERATION = 12.0f;
	protected const float AIR_DECCELERATION_LERP_VALUE_PREDELTA = 0.25f;
	protected const float GROUND_DECCELERATION_LERP_VALUE_PREDELTA = 6.5f;

	//Distances
	protected const float GETGROUNDED_RAYCAST_DISTANCE = 0.80f;

	//Movement that other classes have requested
	protected Vector3 m_InstantExternalMovement = Vector3.zero;

	//List of Laanch movements
	protected List<LaunchMovement> m_LaunchExternalMovement = new List<LaunchMovement>();
		
	//States
	protected bool m_CurrentlyJumping = false;
	public bool m_PausedMovement = false;


	public bool m_IsAttacking;



	//Intitialization

	//Called at the start of the program
	protected void start () 
	{
		m_CharacterController = GetComponent<CharacterController> ();
		m_Anim = GetComponent<Animation>();

		m_AnimState = GetComponent<AnimationState>();

        m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();

		m_AcceptInputFrom = gameObject.GetComponent<AcceptInputFrom> ();

		Physics.IgnoreLayerCollision (LayerMask.NameToLayer (Constants.PLAYER_STRING),
		                              (LayerMask.NameToLayer (Constants.PLAYER_STRING)));

		//Moving platform collision

		//Players ignores moving platform collider inside each other
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer (Constants.PLAYER_STRING),
		                              LayerMask.NameToLayer (Constants.COLLIDE_WITH_MOVING_PLATFORM_LAYER_STRING));
	}
	


	//Update movement

	//The default update all characters should use
	protected void UpdateVelocity () 
	{ 
        if (PauseScreen.IsGamePaused)
		{
			m_Anim.enabled = false;
			return;
		}
		else
		{
			m_Anim.enabled = true;
		}

		//Do not move if we are paused
		if (m_PausedMovement)
		{
			return;
		}

		//If at any point the jump button is released the player is no longer currently jumping
		if(InputManager.getJumpUp(m_AcceptInputFrom.ReadInputFrom))
		{
			m_CurrentlyJumping = false;
		}

		//Initialize states
		GetIsGrounded ();

		//External movement
		InstantExternalMovement ();
		HandleExternalLaunchTimers ();

		//If the player is grounded 
		if(GetIsGrounded())
		{
			//Reset any launch movement that reset when we touch the ground
			ResetGroundedLaunchMovement();

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
				if(!m_IsAttacking)
				{
					m_AnimState.m_Grounded = true;
					GroundMovement();
				}

				else
				{
					m_Velocity = Vector3.zero;
				}
			}
		}
		//If we are not on the ground, we must be airborne, so do air movement
		else
		{
			m_AnimState.m_Grounded = false;
			AirMovement();			
		}

		m_CharacterController.Move ((m_Velocity + GetLaunchVelocity()) * Time.deltaTime);

		m_Anim.Play (m_AnimState.GetAnimation());
	}

	//Moves the player based on the facing angle of the camera and the players input
	protected void GroundMovement()
	{
		//Do animation logic
		OnGroundAnimLogic ();

		//Horizontal movement
		Vector2 horizontalVelocity = new Vector2 (m_Velocity.x, m_Velocity.z);
		if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) != Vector2.zero)
		{
			//Get the current projection from input and camera
			Vector3 projection = GetProjection();
			
			//Calc the direction to look and move
			horizontalVelocity = new Vector2(projection.x * MAX_GROUND_RUNSPEED,
			                                 projection.z * MAX_GROUND_RUNSPEED);

			//Have the player look where the player is going
			transform.LookAt(transform.position + projection);
		}
		else
		{
			if (horizontalVelocity.magnitude < SPEED_MINIMUM_VALUE_TO_CLAMP)
			{
				horizontalVelocity = Vector2.zero;
			}
			else
			{
				horizontalVelocity = Vector2.Lerp(horizontalVelocity, Vector2.zero, Mathf.Min(GROUND_DECCELERATION_LERP_VALUE_PREDELTA * Time.deltaTime, 1.0f));
			}
		}

		//Set our new velocity
		m_Velocity = new Vector3(horizontalVelocity.x, 0.0f, horizontalVelocity.y);
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

		//Horizontal movement
		Vector2 horizontalAirVelocity = new Vector2 (m_Velocity.x, m_Velocity.z);
		if (InputManager.getMove(m_AcceptInputFrom.ReadInputFrom) != Vector2.zero)
		{
			//Get the current projection from input and camera
			Vector3 projection = GetProjection();
			
			//Calc the direction to look and move
			horizontalAirVelocity += new Vector2(projection.x * AIR_HORIZONTAL_CONTROL * Time.deltaTime,
			                                     projection.z * AIR_HORIZONTAL_CONTROL * Time.deltaTime);

			//Have the player look where the player is going
			transform.LookAt(transform.position + projection);
			
			//Cap the horizontal movement speed
			float horizontalVelocityMagnitude = Mathf.Abs(horizontalAirVelocity.magnitude);
			if (horizontalVelocityMagnitude >= MAX_HORIZONTAL_AIR_SPEED) ///Should be max speed
			{
				horizontalAirVelocity = horizontalAirVelocity.normalized * MAX_HORIZONTAL_AIR_SPEED;
			}
		}
		else
		{
			if (horizontalAirVelocity.magnitude < SPEED_MINIMUM_VALUE_TO_CLAMP)
			{
				horizontalAirVelocity = Vector2.zero;
			}
			else
			{
				horizontalAirVelocity = Vector2.Lerp(horizontalAirVelocity, Vector2.zero, Mathf.Min(AIR_DECCELERATION_LERP_VALUE_PREDELTA * Time.deltaTime, 1.0f));
			}
		}
		
		//Set our new velocity
		m_Velocity = new Vector3(horizontalAirVelocity.x, GetVerticalMovementAfterFalling(), horizontalAirVelocity.y);
	}

	/// <summary>
	/// Returns a value for vertical movement after decelleration due to falling is calculated.
	/// </summary>
	protected virtual float GetVerticalMovementAfterFalling()
	{
		//Add our horizontal movement to our move
		float verticalVelocity = m_Velocity.y;
		
		//If we are above the max falling speed, we fall faster
		if(verticalVelocity > MAX_FALL_SPEED)
		{
			//Constantly decrease velocity based on time passed by an deceleration
			if(InputManager.getJump(m_AcceptInputFrom.ReadInputFrom) && m_CurrentlyJumping == true)
			{
				verticalVelocity -= Time.deltaTime * HELD_FALL_ACCELERATION;
			}
			else
			{
				verticalVelocity -= Time.deltaTime * FALL_ACCELERATION;
			}
		}
		return verticalVelocity;
	}



	//External movement

	//Sets additional movement next frame
	public void RequestInstantMovement(Vector3 movement)
	{
		//add the requested movement to the current stored movement
		m_InstantExternalMovement += movement;
	}

	//Moves the player by the set external movement
	protected void InstantExternalMovement()
	{
		if(m_InstantExternalMovement.magnitude != 0.0f)
		{
			m_CharacterController.Move (m_InstantExternalMovement);
			m_InstantExternalMovement = Vector3.zero;
		}
	}

	/// <summary>
	/// Gets the total launch velocity
	/// </summary>
	/// <returns>The launch velocity.</returns>
	public Vector3 GetLaunchVelocity ()
	{	
		//Check if there is any launch movement
		if (m_LaunchExternalMovement.Count == 0)
		{
			return Vector3.zero;
		}

		//Otherwise return our total launch movement
		Vector3 launchVelocity = Vector3.zero;
		for (int index = 0; index < m_LaunchExternalMovement.Count; index++)
		{
			launchVelocity += m_LaunchExternalMovement[index].launch;
		}
		return launchVelocity;
	}
	
	/// <summary>
	/// Handles the timers of all external launch timers
	/// </summary>
	protected void HandleExternalLaunchTimers()
	{
		float deltaTime = Time.deltaTime;

		//Decrement timers
		for (int index = 0; index < m_LaunchExternalMovement.Count; index++)
		{
			m_LaunchExternalMovement[index].timer -= deltaTime;

			//If the timer is up, give the player control over this movement, and then remove it
			if (m_LaunchExternalMovement[index].timer < 0.0f)
			{
				m_Velocity += m_LaunchExternalMovement[index].launch;
				m_LaunchExternalMovement.RemoveAt(index);
			}
		}
	}

	/// <summary>
	/// Resets the launch movement.
	/// </summary>
	public void ResetLaunchMovement()
	{
		if (m_LaunchExternalMovement.Count > 0)
		{
			m_LaunchExternalMovement.Clear ();
		}
	}

	/// <summary>
	/// Resets the launch movement.
	/// </summary>
	public void ResetGroundedLaunchMovement()
	{
		//Do nothing if we have no launch movement
		if (m_LaunchExternalMovement.Count == 0)
		{
			return;
		}

		//Remove launch movement that resets on the ground
		for (int index = 0; index < m_LaunchExternalMovement.Count; index++)
		{
			if (m_LaunchExternalMovement[index].resetOnGrounded)
			{
				m_LaunchExternalMovement.RemoveAt(index);
			}
		}
	}



	//Helper functions

	//Gets a vector3 for the direction we should be getting input based of off the cameras facing angle
	protected Vector3 GetProjection()
	{
		Vector3 projection = m_Camera.forward * InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).y;
		projection += m_Camera.right * InputManager.getMove(m_AcceptInputFrom.ReadInputFrom).x;
		
		projection.y = 0;
		return projection.normalized;
	}

	//Getter for if the character is grounded based on character controller
	//
	//If we are supposed to still be grounded but aren't according to our character controller, we are still considered grounded due to a raycast downwards
	public bool GetIsGrounded()
	{
		RaycastHit hit;

		//If we should be grounded, set our vertical velocity to 0
		if (m_CharacterController.isGrounded)
		{
			if(m_Velocity.y < 0.0f)
			{
				m_Velocity.y = 0.0f;
			}
			return true;
		}
		//If we should be grounded but are above the ground, we teleport down to match the ground
		else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, GETGROUNDED_RAYCAST_DISTANCE) && m_Velocity.y == 0.0f)
		{
			m_CharacterController.Move(hit.point - transform.position);
			return true;
		}

		//Otherwise return that we are airborne
		return false;
	}

	/// <summary>
	/// Gets the players jump speed. Must be overrided by inheriting classes in order to jump.
	/// </summary>
	protected virtual float GetJumpSpeed()
	{
		return 0.0f;
	}

	/// <summary>
	/// Allows other scripts to momentarily stop all movement
	/// </summary>
	/// <param name="paused">If set to <c>true</c> paused.</param>
	public void setMovementPaused(bool paused)
	{
		m_PausedMovement = paused;
	}



	//Jumping functions

	//Sets the vertical velocity to a pre-determined jump speed, and our horizontal air movement to our current running speed
	protected virtual void Jump()
	{
		m_AnimState.AddAnimRequest (AnimationStates.Jump);
		m_AnimState.m_Jumping = true;
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

		//We are now jumping
		m_CurrentlyJumping = true;

		//Set our new velocity
		m_Velocity.y = GetJumpSpeed();
	}

	/// <summary>
	/// Launches a player in a given Vector direction, the magnitude of which will be the speed
	/// </summary>
	/// <param name="jump">Jump.</param>
	public void Launch(Vector3 jump, float timeToForce, bool cancelOnGrounded)
	{
		m_CurrentlyJumping = true;
		m_LaunchExternalMovement.Add (new LaunchMovement (jump, timeToForce, cancelOnGrounded));
	}

	/// <summary>
	/// Sets jump related variables, then calls launch
	/// </summary>
	public virtual void LaunchJump(Vector3 jump, float launchTimer)
	{
		//Animation and sound
		m_SFX.playSound(this.gameObject, Sounds.JumpPad);
		m_AnimState.AddAnimRequest(AnimationStates.Jump);

		//Jump
		m_CurrentlyJumping = true;
		m_Velocity.y = 1.0f;
		Launch(jump, launchTimer, true);
	}



	//Animation

	//Plays a walking animation
    void OnGroundAnimLogic()
    {
		Vector2 horizontalVelocity = new Vector2 (m_Velocity.x, m_Velocity.z);

		if (horizontalVelocity == Vector2.zero)
        {
			m_AnimState.AddAnimRequest(AnimationStates.Idle);           
        }
		else if (horizontalVelocity.magnitude < (MAX_GROUND_RUNSPEED / 2.0f))
        {
			m_AnimState.AddAnimRequest(AnimationStates.Walk);
        }
		else
		{
			m_AnimState.AddAnimRequest(AnimationStates.Run);
		}        
	}

	/// <summary>
	/// Gets the velocity of the player.
	/// </summary>
	public Vector3 GetVelocity()
	{
		return m_Velocity;
	}

	/// <summary>
	/// Gets the players movement for this frame.
	/// </summary>
	public Vector3 GetMovementThisFrame()
	{
		return m_Velocity * Time.deltaTime;
	}
}