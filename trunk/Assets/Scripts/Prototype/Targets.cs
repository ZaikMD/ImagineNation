﻿using UnityEngine;
using System.Collections;

public class Targets : Subject 
{
	//Ensure that targets with this script are tagged appropriately

	//This is a script that will be attached to 
	// a target gameobject in unity and will be
	// used to record whether this specific target
	// has been triggered and relay that information
	// to the targetManager

	//In unity set the appropriate target manager
	public TargetManager m_TargetManager;

	bool m_Active = true;

	void Start () 
	{
		//Add the manager as an observer so that it will be notified
		// when the nerf target has been hit
		addObserver (m_TargetManager);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Will need to add an if statement around sendEvent
		//It will be the condition that the nerf target is hit,

		//Undetermined how the nerf projectile script will 
		//communicate with the target

		//Update: Currently have the nerf projectile setting the
		//target to inactive and based on that relaying the send event
		// This is subject to change...
		if(this.gameObject.active == false)
		{
			sendEvent (ObeserverEvents.NerfTargetHit);
		}
	}
}
