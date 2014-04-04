using UnityEngine;
using System.Collections;

public class DivingBoard : InteractableBaseClass 
{

	public SeeSaw seeSaw;
	public bool m_NeedsBlock;

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
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}

		if(obj.tag == m_Block.tag)
		{
			seeSaw.playerJumping(m_Block);
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
