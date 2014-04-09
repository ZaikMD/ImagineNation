using UnityEngine;
using System.Collections;

// Place this script on your trampoline along with a trigger in order to make it a trampolime
// If you want a doublejump create another gameobject with another trigger in the area above the trampoline and place the TrampolineButton script on it.
// You must pass the appropriate trampoline to the public variable of trampolineButton

public class Trampoline : MonoBehaviour 
{
		//Member Variables
	const float m_MoveSpeed = 25.0f;
	const float m_DoubleMoveSpeed = 35.0f;
	float m_CurrentMoveSpeed = 0;

	public bool m_DoubleJump = false;
	GameObject m_Player;
			
			
	// Use this for initialization
	void start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{		
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if  (other.tag == "Player")
		{
			SoundManager.Instance.playSound(Sounds.Trampoline, this.transform.position);
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
