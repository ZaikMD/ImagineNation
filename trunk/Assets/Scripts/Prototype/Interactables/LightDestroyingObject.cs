using UnityEngine;
using System.Collections;

public class LightDestroyingObject : MonoBehaviour , Observer
{
	//Destroyed Object
	public GameObject m_DestroyedObject;

	//Sender
	public Subject m_Sender;

	//Send self to sender
	void Start()
	{
		m_Sender.addObserver (this);
	}

	//Recieve used command
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.Used && sender == m_Sender)
		{
			light.enabled = true;
			GameObject.Destroy(m_DestroyedObject);
			Destroy(this);
		}
	}
}
