using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour 
{

		//Member Variables
	const float m_MoveSpeed = 1;
	const float m_DoubleMoveSpeed = 1.5f;
	float m_CurrentMoveSpeed = 0;

	public bool m_DoubleJump = false;
	GameObject m_Player;
			
			
	// Use this for initialization
	void start()
	{
		// Assign values to all variables

	}
	
	// Update is called once per frame
	void Update()
	{
		// If timer is bigger then 0 continue players jump
		if ( m_CurrentMoveSpeed > 0)
		{
			m_Player.GetComponent<CharacterController>().Move(new Vector3(0,m_CurrentMoveSpeed,0));

			m_CurrentMoveSpeed -= 0.01f;

		}
		else
			m_CurrentMoveSpeed = 0;
		
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if  (other.tag == "Character")
		{
			m_Player = other.gameObject;
			// Check if we want to double jump
			if (m_DoubleJump)
				m_CurrentMoveSpeed = m_DoubleMoveSpeed;
			
			else
			{
				m_CurrentMoveSpeed = m_MoveSpeed;
			}
			
			//reset double jump flag
			m_DoubleJump = false;
					
		}
	}
}
