using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : Subject, Observer
{
	List <Subject> m_Subjects = new List<Subject>();
	public int m_NumberOfSubjects;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Subjects.Count == m_NumberOfSubjects)
		{
			sendEvent(ObeserverEvents.AllTargetTriggered);
		}
	}

	public void addSubject(Subject subject)
	{
		m_Subjects.Add (subject);
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.NerfTargetHit)
		{
			addSubject(sender);
		}
	}
}
