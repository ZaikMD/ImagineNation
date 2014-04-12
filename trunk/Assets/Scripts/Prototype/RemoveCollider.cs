using UnityEngine;
using System.Collections;

public class RemoveCollider : MonoBehaviour , Observer
{
	public Subject m_Subject; //This is what will trigger the removall
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_Subject.addObserver (this);
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.Used)
		{
			Destroy(this.gameObject);
		}
	}
}
