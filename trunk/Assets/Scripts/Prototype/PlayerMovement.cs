using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public GameObject m_Player;
	CharacterController m_Controller;


	//TEST VARIABLE BECAUSE I WANT TO MOVE - Jason
	bool moveRegular = true;
	
	void Start ()
	{
		//Get character controller
		m_Controller = m_Player.GetComponent<CharacterController>();
	}

	//MAKE YORE PUBLIC MOVEMENT FUNCTIONS HERE

	void Update ()
	{
		if (moveRegular)
		{
			Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) / 5;
			m_Controller.Move (move);
		}
	}


	//Climb
	public void ClimbMovement()
	{
		moveRegular = false;

		//Do we move up?
		Vector3 move = new Vector3 (0, Input.GetAxis ("Vertical") / 5, 0);

		//Do we move left or right?
		move += Input.GetAxis ("Horizontal") / 5 * this.transform.right;

		//Move
		m_Controller.Move (move);
	}

	public void GlideMovement()
	{
		moveRegular = false;
	}

	// Checks to see if we are on the ground
	public bool IsGrounded()
	{
		return m_Controller.isGrounded;
	}
}
