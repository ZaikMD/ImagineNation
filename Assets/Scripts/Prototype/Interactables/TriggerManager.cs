//hi
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerManager : Subject, Observer
{

	int m_NumberOfTargetsHit = 0;
	public bool m_TriggerOneIsEnough = true;

	//Set the number of targets you desire and reference them in unity
	public Subject[] m_Subjects;
	bool[] m_RecievedEvents;

	// Use this for initialization
	void Start () 
	{
		//Add this trigger manager as an observer to all it's subjects
		m_RecievedEvents = new bool[m_Subjects.Length];
		for(int i = 0; i< m_Subjects.Length; i++)
		{
			m_Subjects[i].addObserver(this);
			m_RecievedEvents[i] = false;
		}
	}	

	/// <summary>
	/// The recieveEvent function implemented from the Observer interface
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="recievedEvent">Recieved event.</param>
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		//If the event recieve is "used" 
		if(recievedEvent == ObeserverEvents.Used)
		{
			//Debug.Log ("Recieved Hit");
			//Loop through all the subjects
			for(int i = 0; i < m_Subjects.Length; i++)
			{
				//if the sender is the current subject in the loop
				if(sender == m_Subjects[i])
				{
					//and if the current subject in the loop is false
					if(m_RecievedEvents[i] == false)
					{
						//Then set it to true and increase the number of targets hit
						m_RecievedEvents [i] = true;
						m_NumberOfTargetsHit++;

						//Once all the subjects are hit send the used event to another observer to perform an action in unity
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
