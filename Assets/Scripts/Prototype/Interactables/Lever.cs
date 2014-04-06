using UnityEngine;
using System.Collections;

public class Lever : InteractableBaseClass 
{
	//Bools
	private bool m_IsOn; 

	// Use this for initialization
	void Start () 
	{
		m_Type = InteractableType.Lever;
		m_IsExitable = false;
	}

	public bool getIsOn() //To return whether or not the switch is on
	{
		return m_IsOn;
	}

	public void toggleIsOn()
	{
		sendEvent (ObeserverEvents.Used); //Sends an event saying the switch was used
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this); //Adds this to the interactions in range of the player
		}

		if (obj.tag == "RCCar") 
		{
			obj.transform.parent.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionInRange(this); //Adds this to the interactions in range of the RCcar
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this); //Removes from interactions in range of player
		}

		if (obj.tag == "RCCar") 
		{
			obj.transform.parent.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionOutOfRange(this); //Removes from interactions in range of RCcar
		}
	}
}
