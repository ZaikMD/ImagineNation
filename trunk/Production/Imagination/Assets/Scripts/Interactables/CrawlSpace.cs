﻿/*
 * Created by Greg Fortier
 * Date: Sept 24th, 2014
 *  
 * This script checks when the player enters the crawlspace, 
 * then teleports the player to a hidden area (while the player is teleporting there should be an animation of the player entering the crawlspace.
 * It then teleports the player again to the other end of the crawlspace
 * 
 * 
 *10/10/2014 Edit: Commented and cleaned code - Greg Fortier
*/

using UnityEngine;
using System.Collections;

public class CrawlSpace : MonoBehaviour 
{
	//Arrays are for member variables that need to be different from each other because they are used for individual players, so Element 0 is for player 1
	public float Delay = 3.0f;
	public float[] m_CrawlDelay = new float[2];

	public CrawlSpace m_OtherCrawlSpace;
	public GameObject m_TemporaryHidingSpot;

	CharacterController[] m_Players = new CharacterController[2];

	int m_InComingPlayers = 0;

	public void addIncomingPlayer()
	{
		m_InComingPlayers++;
	}

	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < m_Players.Length; i++)
		{
			m_CrawlDelay[i] = 0.0f;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//null check on hiding spot and m_otherCrawlSpace
		if(m_OtherCrawlSpace != null && m_TemporaryHidingSpot != null)
		{
			// checks to the player int size, if 1 there is one player in the crawlspace and needs to move the second player and not the first that is already in the crawlspace
			for(int i = 0; i < m_Players.Length; i++)
			{
				if(m_Players[i] != null)
				{
					m_Players[i].transform.position = m_TemporaryHidingSpot.transform.position;
				

					//if the timer is not over continue decrementing
					if(m_CrawlDelay[i] > 0)
					{
						m_CrawlDelay[i] -= Time.deltaTime;
						continue;
					}
					//When the timer is over transform the player's position to the other crawl space's
					m_Players[i].transform.position =  m_OtherCrawlSpace.gameObject.transform.position;
					m_Players[i] = null;
				}
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		//there are no players already in the crawlspace
		if(m_InComingPlayers == 0)
		{
			//if the OnTrigger enter was a player
			if (other.tag == "Player")
			{
				//Null check on the player CharacterController, happens if the first element is NULL
				if (m_Players[0] == null)
				{
					//if timer number 2 has more than 2.5 seconds left, Delay the timer incase of problems where players spawn in other players
					if(m_CrawlDelay[1] > 2.5f)
					{
						m_CrawlDelay[0] = Delay + 0.5f;
					}
					else
					{
						m_CrawlDelay[0] = Delay;
					}

					//sets the first character controller element to the typeof charactercontroller
					m_Players[0] = (CharacterController)other.GetComponent(typeof (CharacterController));
					//increment incoming player
					m_OtherCrawlSpace.addIncomingPlayer();
				}

				//Happens when the first m_player controller is not null, (if the first element is not null we know its a second player entering the crawlspace
				else
				{
					if(m_CrawlDelay[0] > 2.5f)
					{
						m_CrawlDelay[1] = Delay + 0.5f;
					}
					else
					{
						m_CrawlDelay[1] = Delay;
					}
					//sets the second character controller element to the typeof charactercontroller (which player it is)
					m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
					m_OtherCrawlSpace.addIncomingPlayer();
				}
			}
		}
	}

	//when the player exits the trigger  incoming player will decrement
	void OnTriggerExit(Collider other)
	{
		if(m_Players[0] == null && m_Players[1] ==null)
		{
			if(other.gameObject.tag == "Player")
			{
				if(m_InComingPlayers > 0)
				{
					m_InComingPlayers--;
					//Debug.Log(gameObject.name + "    " + m_InComingPlayers);
				}
			}
		}
	}
}
