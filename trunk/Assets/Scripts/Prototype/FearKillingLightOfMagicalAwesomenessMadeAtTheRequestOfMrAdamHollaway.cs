/*

TO ADAM.

Attach this script to the light you want to use.
Having a public light provided to an empty game object was a waste.



Created by Kris



4/9/2014 - Jason "The Casual" Hein
	Now removes the darkness in the fear script
	Added comments
*/




using UnityEngine;
using System.Collections;

public class FearKillingLightOfMagicalAwesomenessMadeAtTheRequestOfMrAdamHollaway : MonoBehaviour, Observer 
{
	//Sender
	public Subject m_Sender;

	// Darkness to destroy
	public GameObject m_Darkness;

	// Start
	void Start () 
	{
		// So we do not delete non darkness or access null by accident
		if(m_Darkness.tag != "Darkness")
		{
			Destroy(this.gameObject);
		}

		//Default state
		light.enabled = false;
		m_Sender.addObserver (this);
	}

	// When triggered
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.Used && sender == m_Sender)
		{
			//Let their be light
			light.enabled = true;
			
			//Stop reference
			FearScript.Instance.removeDarknessFear(m_Darkness);
			GameObject.Destroy(m_Darkness);

			//Delete reference
			Destroy(this);
		}
	}
}
