using UnityEngine;
using System.Collections;

public class DropZone : Subject 
{
	public PickUp m_PickUp; //The pickup it needs
	
	// Use this for initialization
	void Start () 
	{
	
	}
	void OnTriggerEnter(Collider obj)
	{
		if(obj.gameObject == m_PickUp.gameObject)
		{
			sendEvent(ObeserverEvents.PickUpIsAtDropZone); //Send the event to the observers saying the pickup is in range
			Destroy(this); //Delete the dropzone
		}
	}
}
