using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public enum ObeserverEvents
{
	CharacterSwitch
}

public class Subject : MonoBehaviour 
{
	List <Observer> m_Observers = new List<Observer>();

	public void addObserver(Observer observer)
	{
		m_Observers.Add (observer);
	}

	public void removeObserver(Observer observer)
	{
		for(int i = 0; i< m_Observers.Count; i++)
		{
			if( m_Observers[i] == observer)
			{
				m_Observers.RemoveAt(i);
				break;
			}
		}
	}

	protected void sendEvent(ObeserverEvents sendEvent)
	{
		for(int i = 0; i < m_Observers.Count; i++)
		{
			m_Observers[i].recieveEvent(this, sendEvent);
		}
	}
}
