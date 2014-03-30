/*

TO USE:


Created by Zach

3/29/2014
	Is no longer a script with two points.
	Now is attached to a crawlspace, and is provided the other crawl space object
	Now roates the player based on the exiting crawl spaces rotation
	Added state machine for animations
*/





using UnityEngine;
using System.Collections;

public class CrawlSpaces : MonoBehaviour 
{
	//Timer
	float m_Timer = 0.0f;
	public float m_CrawlDelay = 1.0f;
	const float TIMER_ANIMATION = 1.0f;

	//Other crawl space to crawl to
	public GameObject m_OtherCrawlSpace;

	//Player
	GameObject m_Player;
	PlayerMovement m_Movement;

	//State of crawling
	enum State
	{
		Default = 0,
		Entering,
		Inside,
		Exiting
	}
	State m_State = State.Default;

	//Initialization
	void Start ()
	{
		m_Player = GameObject.Find ("Zoey");
		m_Movement = (PlayerMovement)m_Player.GetComponent<PlayerMovement>();
	}

	//Crawling
	void Update()
	{
		//Timer is going
		if(m_Timer > 0) 
		{ 
			//Count down the timer with Delta time
			m_Timer -= Time.deltaTime;

			//Crawl Animation is playing


			switch(m_State)
			{
			case State.Entering:
			{
				if (m_Timer <= 0)
				{
					m_State = State.Inside;
					m_Timer = m_CrawlDelay;
					
					//Remove player for now
					m_Player.SetActive (false);

					//Rotate to face out of exiting crawl space
					m_Player.transform.LookAt(m_Player.transform.position + m_OtherCrawlSpace.transform.forward);

					//Move the player a little ahead of the crawl space
					m_Player.transform.position = m_OtherCrawlSpace.transform.position + transform.forward;

				}
			}
			break;
			case State.Inside:
			{
				if (m_Timer <= 0)
				{
					m_State = State.Exiting;
					m_Timer = TIMER_ANIMATION;
					
					//Player re-appears
					m_Player.SetActive (true);
				}
			}
			break;
			case State.Exiting:
			{
				//Crawl Animation is playing

				if (m_Timer <= 0)
				{
					m_State = State.Default;

					//Renable movement
					m_Movement.setCanMove(true);

					//No longer interacting
				}
			}
			break;

			default:
				break;
			}
		}
	}
	
	public void OnUse() 
	{
		if (m_OtherCrawlSpace == null || m_Player == null || m_Movement == null)
		{
			return;
		}

		//Set state
		m_State = State.Entering;

		//Set amount of time to travel the crawl space
		m_Timer = TIMER_ANIMATION;

		//Disable other player movement
		m_Movement.setCanMove(false);
		
		//Play crawl animation

	}
}