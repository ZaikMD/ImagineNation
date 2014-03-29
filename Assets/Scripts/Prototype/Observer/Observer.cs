using UnityEngine;
using System.Collections;

//inherits from mono develop since another class might need it later in the inheritance tree
public interface Observer
{
	void recieveEvent(Subject sender, ObeserverEvents recievedEvent);
}
