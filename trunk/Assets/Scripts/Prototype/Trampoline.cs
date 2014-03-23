using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour 
{

		//Member Variables
	const int m_MoveSpeed = 2;
	const float m_JumpTime = 0.25f;
	float m_JumpTimer = 0;
	const float m_DoubleJumpTime = 4;
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
		if ( m_JumpTimer > 0)
		{
			m_Player.GetComponent<CharacterController>().Move(new Vector3(0,m_MoveSpeed,0));
			
			m_JumpTimer -= Time.deltaTime;
		}
		
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if  (other.tag == "Character")
		{
			m_Player = other.gameObject;
			// Check if we want to double jump
			if (m_DoubleJump)
				m_JumpTimer = m_DoubleJumpTime;
			
			else
				m_JumpTimer = m_JumpTime;
			
			//reset double jump flag
			m_DoubleJump = false;
					
		}
	}
}
