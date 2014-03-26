/*




3/25/2014 - Jason Hein
	Added can move function to disable movement while in the character is busy
	Added moveRegular which is a test variable for now. You can remove it when real ground movement is added.
 */









using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public GameObject m_Player;
	CharacterController m_Controller;
	bool m_CanMove = true;


	//TEST VARIABLE BECAUSE I WANT TO MOVE - Jason
	bool moveRegular = true;


	void Start ()
	{
		//Get character controller
		m_Controller = m_Player.GetComponent<CharacterController>();
	}

	/// <summary>
	/// Sets if you can move.
	/// </summary>
	/// <param name="move">If set to <c>true</c> move.</param>
	public void setCanMove(bool move)
	{
		m_CanMove = move;
	}

	//MAKE YORE PUBLIC MOVEMENT FUNCTIONS HERE

	void Update ()
	{
		//Test code so we can walk
		if (moveRegular)
		{
			GroundMovement();
		}
	}

	/// <summary>
	/// Basic walking movement
	/// </summary>
	public void GroundMovement()
	{
		if (!m_CanMove)
		{
			return;
		}

		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
		{
			return;
		}

		//*********Replace the follwoing code, right now it's just their for movement testing************


		//Moves the player and looks where the player is going
		Vector3 lookAt = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		transform.LookAt (transform.position + lookAt);
		m_Controller.Move (transform.forward / 5);
	}

	/// <summary>
	/// Climbs the player.
	/// </summary>
	public void ClimbMovement()
	{
		if (!m_CanMove)
		{
			return;
		}

		moveRegular = false;

		//Do we move up?
		Vector3 move = new Vector3 (0, Input.GetAxis ("Vertical") / 5, 0);

		//Do we move left or right?
		move += Input.GetAxis ("Horizontal") / 5 * this.transform.right;

		//Move
		m_Controller.Move (move);
	}

	/// <summary>
	/// Glides the player.
	/// </summary>
	public void GlideMovement()
	{
		if (!m_CanMove)
		{
			return;
		}

		moveRegular = false;
	}

	// Checks to see if we are on the ground
	public bool IsGrounded()
	{
		return m_Controller.isGrounded;
	}
}
