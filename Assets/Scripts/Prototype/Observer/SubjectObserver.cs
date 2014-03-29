using UnityEngine;
using System.Collections;

public abstract class SubjectObserver : Subject 
{
	public abstract void recieveEvent(Subject sender, ObeserverEvents recievedEvent);
}
