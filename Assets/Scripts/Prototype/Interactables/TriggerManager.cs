//hi
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Last updated 04/06/2014

public class TriggerManager : Subject, Observer
{

	int m_NumberOfTargetsHit = 0;
	public bool m_TriggerOneIsEnough = true;

	//Change this to the object that you want to recieve an event from a 
	// target manager

	public Subject[] m_Subjects;
	bool[] m_RecievedEvents;

	// Use this for initialization
	void Start () 
	{
		m_RecievedEvents = new bool[m_Subjects.Length];
		for(int i = 0; i< m_Subjects.Length; i++)
		{
			m_Subjects[i].addObserver(this);
			m_RecievedEvents[i] = false;
		}
	}	

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.Used)
		{
			//Debug.Log ("Recieved Hit");
			for(int i = 0; i < m_Subjects.Length; i++)
			{
				if(sender == m_Subjects[i])
				{
					if(m_RecievedEvents[i] == false)
					{
						m_RecievedEvents [i] = true;
						m_NumberOfTargetsHit++;

						if(m_TriggerOneIsEnough || m_NumberOfTargetsHit >= m_Subjects.Length)
						{
							sendEvent(ObeserverEvents.Used);
							//Debug.Log("All Targets Hit");
						}
					}
				}
			}
		}
	}
}
