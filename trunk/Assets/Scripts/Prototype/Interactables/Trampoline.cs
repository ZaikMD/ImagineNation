using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour 
{
		//Member Variables
	const float m_MoveSpeed = 35.0f;
	const float m_DoubleMoveSpeed = 65.0f;
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


			m_Player.GetComponent<PlayerMovement>().LaunchJump(m_CurrentMoveSpeed);
			//reset double jump flag
			m_DoubleJump = false;
					
		}
	}
}
