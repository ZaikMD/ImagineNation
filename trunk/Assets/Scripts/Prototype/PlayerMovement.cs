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
 */









using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public GameObject m_Player;
	public Transform m_CameraTransform;

	CharacterController m_Controller;
	bool m_CanMove = true;

	const float MOVE_SPEED = 6.0f;
	const float FALL_SPEED = 15.0f;
	const float AIR_MOVE_SPEED = 3.0f;
	const float GLIDING_FALL_SPEED = 6.0f;


	//TEMPORARY FLOAT THAT SHOULD BE MOVED TO STATE MACHINE
	const float JUMP_TIME = 0.2f;
	const float JUMP_SPEED = 15.0f;
	float m_JumperTimer = 0.0f;


	void Start ()
	{
		//Get character controller
		m_Controller = m_Player.GetComponent<CharacterController>();
		m_Player.AddComponent ("EnviromentInteraction");
	}



	//CAN REMOVE BELOW FUNCTION ONCE WE HOOK UP STATE MACHINE


	/// <summary>
	/// Allows you to enable and renable movement during interactions.
	/// </summary>
	public void setCanMove(bool move)
	{
		m_CanMove = move;
	}

	/// <summary>
	/// Returns a normalized input vector based on camera's rotation.
	/// </summary>
	/// <returns>Controller input in relation to camera's rotation.</returns>
	Vector3 getControllerProjection()
	{
		//movementInput move = PlayerInput.Instance.getCameraMovement();

		Vector3 projection = m_CameraTransform.forward * Input.GetAxis("Vertical");
		projection += m_CameraTransform.right * Input.GetAxis("Horizontal");

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
			if (m_JumperTimer > 0.0f)
			{
				m_JumperTimer = 0.0f;
			}

			if (Input.GetButtonDown("Jump"))
			{
				JumpMovement();
			}
			else
			{
				GroundMovement();
			}
		}
		else if (m_JumperTimer > 0.0f)
		{
			JumpMovement();
		}
		else
		{
			AirMovement();
		}
	}

	// Checks to see if we are on the ground
	public bool IsGrounded()
	{
		return m_Controller.isGrounded;
		//isGrounded = (characterController.Move (forwardDirection * (Time.deltaTime * movementSpeed)) & CollisionFlags.Below) != 0;
	}


	//MAKE YORE PUBLIC MOVEMENT FUNCTIONS HERE






	/// <summary>
	/// Basic walking movement
	/// </summary>
	public void GroundMovement()
	{
		if (!m_CanMove || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
		{
			return;
		}

		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());
		m_Controller.Move (transform.forward * MOVE_SPEED * Time.deltaTime);
	}

	/// <summary>
	/// Climbs the player.
	/// </summary>
	public void ClimbMovement()
	{
		if ((Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
		{
			return;
		}

		//Climbing up and down
		Vector3 move = new Vector3 (0, Input.GetAxis ("Vertical") * MOVE_SPEED, 0);

		//Climbing left and right
		if (m_CameraTransform.forward.x > 0)
		{
			move += Input.GetAxis ("Horizontal") * MOVE_SPEED * this.transform.right;
		}
		else
		{
			move -= Input.GetAxis ("Horizontal") * MOVE_SPEED * this.transform.right;
		}

		//Move
		m_Controller.Move (move * Time.deltaTime);
	}

	/// <summary>
	/// Glides the player.
	/// </summary>
	public void GlideMovement()
	{
		//Falling
		m_Controller.Move (-transform.up * GLIDING_FALL_SPEED * Time.deltaTime);

		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
		{
			return;
		}
		
		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());
		m_Controller.Move (transform.forward * MOVE_SPEED * Time.deltaTime);
	}

	public void AirMovement()
	{
		if (!m_CanMove)
		{
			return;
		}

		//Falling
		m_Controller.Move (-transform.up * GLIDING_FALL_SPEED * Time.deltaTime);
		
		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
		{
			return;
		}
		
		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());

		m_Controller.Move (transform.forward * MOVE_SPEED * Time.deltaTime);
	}

	public void JumpMovement()
	{
		if (!m_CanMove || m_JumperTimer > JUMP_TIME)
		{
			m_JumperTimer = 0.0f;
			return;
		}
		m_JumperTimer += Time.deltaTime;
		
		//Jumping up
		if (m_JumperTimer > JUMP_TIME / 1.5f)
		{
			m_Controller.Move (transform.up * (JUMP_SPEED / 2 * Time.deltaTime));
			m_JumperTimer += Time.deltaTime;
		}
		else
		{
			m_Controller.Move (transform.up * (JUMP_SPEED * Time.deltaTime));
		}
		
		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
		{
			return;
		}
		
		//Moves the player and looks where the player is going
		transform.LookAt (transform.position + getControllerProjection());
		m_Controller.Move (transform.forward * AIR_MOVE_SPEED * Time.deltaTime);
	}

	public void BlockHeldMovement ()
	{
		if (!m_CanMove || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
		{
			return;
		}

		//Move
	}
}
