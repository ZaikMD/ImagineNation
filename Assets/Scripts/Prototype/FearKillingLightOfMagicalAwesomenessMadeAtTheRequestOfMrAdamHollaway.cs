using UnityEngine;
using System.Collections;

public class FearKillingLightOfMagicalAwesomenessMadeAtTheRequestOfMrAdamHollaway : MonoBehaviour, Observer 
{
	public Subject m_Sender;

	public Light m_Light;
	public GameObject m_Darkness;

	Collider[] m_ZoeyColliders;

	// Use this for initialization
	void Start () 
	{
		if(m_Darkness.tag != "Darkness")
		{
			Destroy(this.gameObject);
		}

		m_Light.enabled = false;

		m_Sender.addObserver (this);

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		for(int i = 0; i < players.Length; i++)
		{
			if(players[i].name == "Zoey")
			{
				m_ZoeyColliders = players[i].GetComponents<Collider>();
				break;
			}
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if (recievedEvent == ObeserverEvents.Used) 
		{
			m_Light.enabled = true;

			for(int i = 0; i < m_ZoeyColliders.Length; i++)
			{
				FearScript.Instance.setIgnoreDarkness(m_ZoeyColliders[i]);
			}
		}
	}
}
