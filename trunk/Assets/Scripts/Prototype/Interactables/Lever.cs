﻿using UnityEngine;
using System.Collections;

public class Lever : InteractableBaseClass 
{

	private bool m_IsOn;
	// Use this for initialization
	void Start () 
	{
		m_Type = InteractableType.Lever;
		m_IsExitable = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool getIsOn()
	{
		return m_IsOn;
	}

	public void toggleIsOn()
	{
		sendEvent (ObeserverEvents.Used);
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}

		if (obj.tag == "RCCar") 
		{
			obj.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}

		if (obj.tag == "RCCar") 
		{
			obj.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionOutInRange(this);
		}
	}
}
