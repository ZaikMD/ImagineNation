using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : Subject, Observer
{

	int m_NumberOfTargetsHit = 0;

	//Change this to the object that you want to recieve an event from a 
	// target manager

	public Targets[] m_Targets;

	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i< m_Targets.Length; i++)
		{
			m_Targets[i].addObserver(this);
		}
	}	

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.NerfTargetHit)
		{
			Debug.Log ("Recieved Hit");
			m_NumberOfTargetsHit++;
			if(m_NumberOfTargetsHit >= m_Targets.Length)
			{
				sendEvent(ObeserverEvents.AllTargetTriggered);
				Debug.Log("All Targets Hit");
			}
		}
	}
}
