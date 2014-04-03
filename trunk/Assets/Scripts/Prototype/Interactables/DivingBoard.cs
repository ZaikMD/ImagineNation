using UnityEngine;
using System.Collections;

public class DivingBoard : InteractableBaseClass 
{

	public SeeSaw seeSaw;

	void Start()
	{
		m_Type = InteractableType.DivingBoard;
		
		m_IsExitable = false;
	}

	public void notifySeeSaw(GameObject obj)
	{
		//Notify the SeeSaw of the player interaction
		seeSaw.playerJumping (obj.gameObject);
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}
	}

}
