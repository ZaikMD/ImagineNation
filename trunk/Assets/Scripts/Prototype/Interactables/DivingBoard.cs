using UnityEngine;
using System.Collections;

public class DivingBoard : InteractableBaseClass 
{

	public SeeSaw seeSaw; //The seesaw it's attached too
	public bool m_NeedsBlock; //If it takes a block

	public GameObject m_Block;

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
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this); //Put this in the interaction list
		}

		if(m_Block != null)
		{
			if(obj.tag == m_Block.tag)
			{
				seeSaw.playerJumping(m_Block); //Pass the block to the seesaw to start it
			}
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this); //Remove from the interaction list
		}
	}

}
