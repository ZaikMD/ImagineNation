using UnityEngine;
using System.Collections;

public class CrawlSpace : MonoBehaviour {
	
	private float m_CrawlDelay = 3.0f;
	private float m_CrawlDelayTwo = 3.0f;

	public GameObject m_OtherCrawlSpace;
	public GameObject m_TemporaryHidingSpot;

	CharacterController m_PlayerOne;
	CharacterController m_PlayerTwo;

	bool m_EnteredCrawlSpace;
	bool m_EnteredCrawlSpaceTwo;

	bool m_isPlayerOne;
	bool m_isPlayerTwo;


	// Use this for initialization
	void Start () {
	
		m_EnteredCrawlSpace = false;
		m_isPlayerOne = false;
		m_isPlayerTwo = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_isPlayerOne == true)
		{
			CrawlSpaceTravel (m_PlayerOne);
			if (m_PlayerOne.transform.position  == m_OtherCrawlSpace.transform.position)
			{
				m_isPlayerOne = false;
			}

		}

		if(m_isPlayerTwo == true)
		{
			CrawlSpaceTravel(m_PlayerTwo);
			if (m_PlayerTwo.transform.position == m_OtherCrawlSpace.transform.position)
			{
				m_isPlayerTwo = false;
			}
		}
	}

	void CrawlSpaceTravel(CharacterController player)
	{
		if(m_OtherCrawlSpace && m_TemporaryHidingSpot != null)
		{
			
			if (m_EnteredCrawlSpace || m_EnteredCrawlSpaceTwo)
			{	
				player.transform.position = m_TemporaryHidingSpot.transform.position;
				
				if(m_CrawlDelay >0)
				{
					m_CrawlDelay -= Time.deltaTime;
				}
				
				else if(m_CrawlDelay <= 0)
				{
					m_EnteredCrawlSpace = false;
					player.transform.position =  m_OtherCrawlSpace.transform.position;

				}

				if (m_CrawlDelayTwo >0)
				{
					m_CrawlDelayTwo -= Time.deltaTime;
				}

				else if (m_CrawlDelayTwo <= 0)
				{
					m_EnteredCrawlSpaceTwo = false;
					player.transform.position =  m_OtherCrawlSpace.transform.position;

				}
			}
			
		}
	}


	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			if (m_isPlayerOne == false && m_isPlayerTwo == false)
			{
				m_CrawlDelay = 3.0f;
				m_PlayerOne = (CharacterController)other.GetComponent(typeof (CharacterController));
				m_isPlayerOne = true;
				m_EnteredCrawlSpace = true;

				//return;
			}
		else if (m_isPlayerOne == true && m_isPlayerTwo == false)
			{
				m_CrawlDelayTwo = 3.0f;
				m_PlayerTwo = (CharacterController)other.GetComponent(typeof (CharacterController));
				m_isPlayerTwo = true;
				m_EnteredCrawlSpaceTwo = true;

				//return;
			}
					

		}
	}
}
