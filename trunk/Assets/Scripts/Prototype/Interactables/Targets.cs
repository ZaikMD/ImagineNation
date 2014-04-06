using UnityEngine;
using System.Collections;

//Last updated 04/06/2014

public class Targets : Subject 
{
	//Ensure that targets with this script are tagged appropriately

	//This is a script that will be attached to 
	// a target gameobject in unity and will be
	// used to record whether this specific target
	// has been triggered and relay that information
	// to the corresponding triggerManager

	//In unity set the appropriate triggerManager

	public bool m_Active = true;

	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the target becomes inactive send the used event to all observers
		if(!m_Active)
		{
			sendEvent (ObeserverEvents.Used);
			//Debug.Log ("HIT");
			gameObject.SetActive(false);
		}
	}
}
