using UnityEngine;
using System.Collections;

public class CrawlSpace : MonoBehaviour 
{
	public float Delay = 3.0f;
	float[] m_CrawlDelay = new float[2];

	public GameObject m_OtherCrawlSpace;
	public GameObject m_TemporaryHidingSpot;

	CharacterController[] m_Players = new CharacterController[2];

	bool[] m_EnteredCrawlSpace = new bool[2];
	bool[] m_MovingPlayer = new bool[2];


	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < m_EnteredCrawlSpace.Length; i++)
		{
			m_MovingPlayer[i] = false;
			m_EnteredCrawlSpace[i] = false;
			m_CrawlDelay[i] = 0.0f;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_OtherCrawlSpace != null && m_TemporaryHidingSpot != null)
		{
			for(int i = 0; i < m_EnteredCrawlSpace.Length; i++)
			{
				if(m_EnteredCrawlSpace[i] == true)
				{
					m_Players[i].transform.position = m_TemporaryHidingSpot.transform.position;
					m_MovingPlayer[i] = true;
					
					if(m_CrawlDelay[i] > 0)
					{
						m_CrawlDelay[i] -= Time.deltaTime;
						continue;
					}

					m_EnteredCrawlSpace[i] = false;

					m_Players[i].transform.position =  m_OtherCrawlSpace.transform.position;
					m_Players[i] = null;
				}
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{

			if (other.tag == "Player")
			{
				if (m_Players[0] == null)
				{
					if(m_CrawlDelay[1] > 2.5f)
					{
						m_CrawlDelay[0] = Delay + 0.5f;
					}
					else
					{
						m_CrawlDelay[0] = Delay;
					}
				if (m_MovingPlayer[0] == false)
				{
					m_Players[0] = (CharacterController)other.GetComponent(typeof (CharacterController));
					m_EnteredCrawlSpace[0] = true;
				}
				}		
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

				if (m_MovingPlayer[1] == false)
				{
					m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
					m_EnteredCrawlSpace[1] = true;
				}
				}
			}
	}

	void OnTriggerExit(Collider other)
	{
		for(int i = 0; i < m_MovingPlayer.Length; i++)
		{
			if (m_MovingPlayer[i] == true)
			{
				m_MovingPlayer[i] = false;
			}
		}
	}
}
