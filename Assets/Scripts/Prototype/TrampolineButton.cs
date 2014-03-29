using UnityEngine;
using System.Collections;

public class TrampolineButton : MonoBehaviour 
{
	public Trampoline m_MyTrampoline;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Character")
		{
			if (other.gameObject.GetComponent<CharacterController>().velocity.y < 0)
			{

				if (Input.GetKeyDown(KeyCode.B))
				{

					m_MyTrampoline.m_DoubleJump = true;
				
				}
			}
		}
	}
}
