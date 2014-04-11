using UnityEngine;
using System.Collections;

public class Key : Subject 
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			sendEvent(ObeserverEvents.Used);
			Destroy(this.gameObject);
		}
	}
}
