using UnityEngine;
using System.Collections;

public class TrampolineButton : MonoBehaviour 
{
	public Trampoline m_MyTrampoline;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" &&
		    other.gameObject.GetComponent<CharacterController>().velocity.y < 0 &&
		    PlayerInput.Instance.getEnviromentInteraction())
		{
			m_MyTrampoline.m_DoubleJump = true;
		}
	}
}
