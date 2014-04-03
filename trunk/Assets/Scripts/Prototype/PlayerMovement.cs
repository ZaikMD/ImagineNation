/*

TO USE

Attach this component to the player.

Create a camera with the camera controller.

Give player movement the camera's transform.





Created by Jason Hein 3/25/2014


3/25/2014
	Added can move function to disable movement while in the character is busy
	Added moveRegular which is a test variable for now. You can remove it when real ground movement is added.
3/28/2014
	Movement is now based on camera projection
3/29/2014
	Added basic jumping (still very buggy)
	Added air, jump, and gliding movement
3/30/2014
	Added movement for pushing blocks
	Now initializes Enviroment interaction script
	Added proper airborne movement
	Added launching
	Added gliding
4/2/2014
	Added maximum falling speed
 */









using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public Transform m_CameraTransform;

	CharacterController m_Controller;
	bool m_CanMove = true;
	float m_JumpNextTimer = 0.0f;
	const float JUMP_NEXT_TIMER = 1.0f;

	//Speeds
	const float MOVE_SPEED = 10.0f;
	const float CLIMB_SPEED = 3.0f;
	const float FALL_ACCLERATION = 40.0f;
	const float JUMP_SPEED = 15.0f;
	const float AIR_HORIZONTAL_MOVE_SPEED = 6.0f;
	const float GLIDING_FALL_SPEED = -1.0f;
	const float PUSHING_BLOCK_SPEED = 3.0f;
	const float AIMING_ROTATION_SPEED = 120.0f;
	const float MAXIMUM_FALLING_SPEED = 21.0f;
	float m_VerticalVelocity = 0.0f;


	void Start ()
	{
		//Get character controller
		m_Controller = gameObject.GetComponent<CharacterController>();
		//gameObject.AddComponent ("EnvironmentInteraction");
	}

	/// <summary>
	/// Gets the verticle speed of the player.
	/// </summary>
	/// <returns>The verticle speed.</returns>
	public float getVerticleSpeed()
	{
		return m_VerticalVelocity;
	}

	/// <summary>
	/// Allows you to enable and renable movement during interactions.
	/// </summary>
	public void setCanMove(bool move)
	{
		m_CanMove = move;

		//Reset air movement speed
		if (m_CanMove == false && !IsGrounded ())
		{
			m_VerticalVelocity = 0;
		}
	}

	/// <summary>
	/// Returns a normalized input vector based on camera's rotation.
	/// </summary>
	/// <returns>Controller input in relation to camera's rotation.</returns>
	public Vector3 getControllerProjection()
	{
		//movementInput move = PlayerInput.Instance.getCameraMovement();

		Vector3 projection = m_CameraTransform.forward * PlayerInput.Instance.getMovementInput().y;
		projection += m_CameraTransform.right * PlayerInput.Instance.getMovementInput().x;

		//Vector3 projection = m_CameraTransform.forward * move.y;
		//projection += m_CameraTransform.right * move.x;

		projection.y = 0;
		return projection.normalized;
	}
	
	void Update ()
	{
		//Temporary testing of movement
		if (IsGrounded ())
		{
			if (PlayerInput.Instance.getJumpInput() || PlayerInput.Instance.getJumpHeld() || m_JumpNextTimer > 0.0f)
			{
				Jump();
			}
			else
			{
				if (m_VerticalVelocity != 0)
				{
					m_VerticalVelocity = 0;
				}

				GroundMovement();
			}
			if (m_JumpNextTimer > 0.0f)
			{
				m_JumpNextTimer = -Time.deltaTime;
			}
		}
		else
		{
			AirMovement();

			if (PlayerInput.Instance.getJumpInput() || PlayerInput.Instance.getJumpHeld())
			{
				m_JumpNextTimer = JUMP_NEXT_TIMER;
			}
			else
			{
				if (m_JumpNextTimer > 0.0f)
				{
					m_JumpNextTimer = -Time.deltaTime;
				}
			}
		}
	}

	// Checks to see if we are on the ground
	public bool IsGrounded()
	{
		return m_Controller.isGrounded;
		//isGrounded = (characterController.Move (forwardDirection * (Time.deltaTime * movementSpeed)) & CollisionFlags.Below) != 0;
	}

	/// <summary>
	/// Basic walking movement
	/// </summary>
	public void GroundMovement()
	{
		if (!m_CanMove || PlayerInput.Instance.getMovementInput() == Vector2.zero)
		{
			return;
		}

		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());
		m_Controller.Move (transform.forward * MOVE_SPEED * Time.deltaTime);
	}

	/// <summary>
	/// Player can rotate while aiming.
	/// </summary>
	public void AimMovement()
	{
		if (PlayerInput.Instance.getCameraMovement().x == 0)
		{
			return;
		}
		transform.Rotate(new Vector3 (0.0f, PlayerInput.Instance.getCameraMovement().x * AIMING_ROTATION_SPEED * Time.deltaTime, 0.0f));
	}

	/// <summary>
	/// Climbs the player.
	/// </summary>
	public void ClimbMovement()
	{
		if ((PlayerInput.Instance.getMovementInput().x == 0 && PlayerInput.Instance.getMovementInput().y == 0))
		{
			return;
		}

		//Climbing up and down
		Vector3 move = new Vector3 (0, PlayerInput.Instance.getMovementInput().y * CLIMB_SPEED, 0);

		//Climbing left and right
		if (m_CameraTransform.forward.x > 0)
		{
			move += PlayerInput.Instance.getMovementInput().x * CLIMB_SPEED * transform.right;
		}
		else
		{
			move -= PlayerInput.Instance.getMovementInput().x * CLIMB_SPEED * transform.right;
		}

		//Move
		m_Controller.Move (move * Time.deltaTime);
	}

	/// <summary>
	/// Glides the player. You must call setCanMove(true); from the cape class when you touch the ground or exit gliding.
	/// </summary>
	public void Glide()
	{
		//Falling
		if (m_VerticalVelocity > GLIDING_FALL_SPEED)
		{
			m_VerticalVelocity -= Time.deltaTime * FALL_ACCLERATION;
		}
		m_Controller.Move (transform.up * m_VerticalVelocity * Time.deltaTime);

		if (PlayerInput.Instance.getMovementInput().x == 0 && PlayerInput.Instance.getMovementInput().y == 0)
		{
			return;
		}
		
		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());
		m_Controller.Move (transform.forward * AIR_HORIZONTAL_MOVE_SPEED * Time.deltaTime);
	}

	/// <summary>
	/// Movement while airborne.
	/// </summary>
	public void AirMovement()
	{
		if (!m_CanMove)
		{
			return;
		}

		//Falling
		m_VerticalVelocity -= Time.deltaTime * FALL_ACCLERATION;

		//There is a maximum falling speed
		if (m_VerticalVelocity < -MAXIMUM_FALLING_SPEED)
		{
			m_VerticalVelocity = -MAXIMUM_FALLING_SPEED;
		}

		//Fall
		m_Controller.Move (transform.up * m_VerticalVelocity * Time.deltaTime);
		
		if (PlayerInput.Instance.getMovementInput() == Vector2.zero)
		{
			return;
		}
		
		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());
		m_Controller.Move (transform.forward * AIR_HORIZONTAL_MOVE_SPEED * Time.deltaTime);
	}

	/// <summary>
	/// The player jumps.
	/// </summary>
	public void Jump()
	{
		if (!m_CanMove)
		{
			return;
		}
		m_VerticalVelocity = JUMP_SPEED;
		AirMovement ();
	}

	/// <summary>
	/// Laucnhes the player upward by a set speed.
	/// </summary>
	/// <param name="launchSpeed">Launch speed.</param>
	public void LaunchJump(float launchSpeed)
	{
		if (!m_CanMove)
		{
			return;
		}
		m_VerticalVelocity = launchSpeed;
		AirMovement ();
	}

	public void BlockHeldMovement (Size blockSize)
	{
		if (PlayerInput.Instance.getMovementInput().y == 0)
		{
			return;
		}

		//Smaller characters cannot move blocks too large to push
		if (blockSize == Size.Large && (gameObject.name == "Zoey" || gameObject.name == "Derek"))
		{
			return;
		}
		else if (blockSize == Size.Medium && gameObject.name == "Zoey")
		{
			return;
		}

		Vector3 move = Vector3.zero;
		if (m_CameraTransform.forward.z > 0)
		{
			move += PlayerInput.Instance.getMovementInput().y * transform.forward;
		}
		else
		{
			move -= PlayerInput.Instance.getMovementInput().y * transform.forward;
		}

		//Moves the player
		m_Controller.Move (move * PUSHING_BLOCK_SPEED * Time.deltaTime);
	}
}
