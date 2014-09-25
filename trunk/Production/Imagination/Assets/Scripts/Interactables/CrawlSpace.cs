using UnityEngine;
using System.Collections;

public class CrawlSpace : MonoBehaviour 
{
	public float Delay = 3.0f;
	float[] m_CrawlDelay = new float[2];

	public CrawlSpace m_OtherCrawlSpace;
	public GameObject m_TemporaryHidingSpot;

	CharacterController[] m_Players = new CharacterController[2];
	
	//bool[] m_MovingPlayer = new bool[2];
	//bool[] m_IncomingPlayer = new bool[2];

	int m_InComingPlayers = 0;

	public void addIncomingPlayer()
	{
		m_InComingPlayers++;
		//Debug.Log (gameObject.name + "    " + m_InComingPlayers);
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
		if(m_OtherCrawlSpace != null && m_TemporaryHidingSpot != null)
		{
			for(int i = 0; i < m_Players.Length; i++)
			{
				if(m_Players[i] != null)
				{
					m_Players[i].transform.position = m_TemporaryHidingSpot.transform.position;
				
					
					if(m_CrawlDelay[i] > 0)
					{
						m_CrawlDelay[i] -= Time.deltaTime;
						continue;
					}
					m_Players[i].transform.position =  m_OtherCrawlSpace.gameObject.transform.position;
					m_Players[i] = null;
				}
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if(m_InComingPlayers == 0)
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

					m_Players[0] = (CharacterController)other.GetComponent(typeof (CharacterController));
					m_OtherCrawlSpace.addIncomingPlayer();
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
					m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
					m_OtherCrawlSpace.addIncomingPlayer();
				}
			}
		}
	}

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
