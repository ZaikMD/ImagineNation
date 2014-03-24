using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public GameObject m_Player;
	CharacterController m_Controller;
	
	void Start ()
	{
		//Get character controller
		m_Controller = m_Player.GetComponent<CharacterController>();
	}

	//MAKE YORE PUBLIC MOVEMENT FUNCTIONS HERE

	//Climb
	public void ClimbMovement()
	{
		//Do we move up?
		Vector3 move = new Vector3 (0, Input.GetAxis ("Vertical") / 5, 0);

		//Do we move left or right?
		move += Input.GetAxis ("Horizontal") / 5 * this.transform.right;

		//Move
		m_Controller.Move (move);
	}

	public void GlideMovement()
	{

	}

	// Checks to see if we are on the ground
	public bool IsGrounded()
	{
		return m_Controller.isGrounded;
	}
}
