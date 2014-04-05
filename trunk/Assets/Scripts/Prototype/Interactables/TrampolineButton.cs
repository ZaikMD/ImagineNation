using UnityEngine;
using System.Collections;

//Place on a empty gameobject with a trigger above the desired trampoline. You must passed the trampoline to this script
//When the player Is moving downwards and is inside the trigger he can press the button to doublejump on the trampoline.

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
