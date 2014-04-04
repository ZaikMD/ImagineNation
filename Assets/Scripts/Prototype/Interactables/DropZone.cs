using UnityEngine;
using System.Collections;

public class DropZone : Subject 
{
	public PickUp m_PickUp;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.gameObject == m_PickUp.gameObject)
		{
			sendEvent(ObeserverEvents.PickUpIsAtDropZone);
			Destroy(this);
		}
	}
}
