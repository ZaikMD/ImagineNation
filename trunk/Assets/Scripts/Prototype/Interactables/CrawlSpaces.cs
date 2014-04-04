/*

TO USE:

Add two of the prefab to world.

Set crawl space for each crawl space.

A crawl space not set will make that crawl space an exit only.





Created by Zach


3/29/2014
	Is no longer a script with two points.
	Now is attached to a crawlspace, and is provided the other crawl space object
	Now roates the player based on the exiting crawl spaces rotation
	Added state machine for animations
*/





using UnityEngine;
using System.Collections;

public class CrawlSpaces : InteractableBaseClass 
{
	//Timer
	float m_Timer = 0.0f;
	public float m_CrawlDelay = 3.0f;
	const float TIMER_ANIMATION = 1.0f;

	//Other crawl space to crawl to
	public GameObject m_OtherCrawlSpace;

	//Player
	GameObject m_Player;

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
		m_IsExitable = false;
		m_Type = InteractableType.CrawlSpace;
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

					//No longer interacting
					if (m_Player.CompareTag("Player"))
					{
						m_Player.GetComponent<PlayerState>().exitInteracting();
					}

					if (m_Player.CompareTag("RCCar"))
					{
						m_Player.gameObject.GetComponent<RCCarMovement>().m_CanMove = true;
						m_Player.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.ExitInteraction();
					}
				}
			}
			break;

			default:
				break;
			}
		}
	}
	
	public void OnUse(GameObject aObject) 
	{
		if (m_OtherCrawlSpace == null || ( (aObject.name != "Zoey")&&( aObject.tag != "RCCar")))
		{
			return;
		}

		m_Player = aObject;

		if (aObject.name == "Zoey")
		{
			PlayerMovement movement = (PlayerMovement)m_Player.GetComponent<PlayerMovement>();
			movement.setCanMove(false);
			m_Player.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}

		if(aObject.tag == "RCCar")
		{
			//PlayerMovement movement = (PlayerMovement)m_Player.GetComponent<PlayerMovement>();
			//GameObject parent = aObject.transform.parent.gameObject;

			//RCCarMovement rCCarMovement = parent.GetComponent<RCCarMovement>();


			m_Player.gameObject.GetComponent<RCCarMovement>().m_CanMove = false;
			m_Player.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionOutOfRange(this);
		}

		//Set state
		m_State = State.Entering;

		//Set amount of time to travel the crawl space
		m_Timer = TIMER_ANIMATION;
		
		//Play crawl animation

	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			if(obj.name == "Zoey")
			{
				obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
				//Debug.Log(this + " is in range");
			}
		}

		if(obj.tag == "RCCar")
		{
			obj.transform.parent.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			if(obj.name == "Zoey")
			{
				obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
			}
		}

		if(obj.tag == "RCCar")
		{
			obj.transform.parent.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionOutOfRange(this);
		}
	}
}