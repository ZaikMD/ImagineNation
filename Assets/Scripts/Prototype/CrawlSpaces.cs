using UnityEngine;
using System.Collections;

public class CrawlSpaces : MonoBehaviour 
{
	float m_Timer;
	Vector3 m_PlayerPos;
	public GameObject m_PointAPos;
	public GameObject m_PointBPos;
	bool m_PointABool;
	bool m_PointBBool;
	GameObject m_Player;


	// Use this for initialization
	void Start ()
	{
		m_Timer = 0.0f;
		m_PlayerPos = Vector3.zero;
		m_PointABool = false;
		m_PointBBool = false;
	}
	
	void Update()
	{
		if(m_Timer > 0) 
		{ 
			m_Timer -= Time.deltaTime;//Count down the timer with Delta time
				
			if( m_Timer <= 0)
			{
				if (m_PointABool == true)
				{
					m_Player.transform.position = m_PointBPos.transform.position;//Player position = Crawl Space B
						//Play crawling animation
					m_PointABool = false;
				}
				
				if (m_PointBBool == true)
				{
					m_Player.transform.position = m_PointAPos.transform.position;//Player position = Crawl SpaceA
						//Play crawling animation
					m_PointBBool = false;
				}
				
			}
		}
	}
	
	public void OnUse() 
	{
		Vector3 distA =  m_Player.transform.position - m_PointAPos.transform.position;//calculate the distance between the player and point a 
		Vector3 distB = m_Player.transform.position - m_PointBPos.transform.position;
		if(distA.magnitude < distB.magnitude)
		{
			m_Timer = 5.0f;   //set amount of time to travel the crawl space
			m_Player.transform.position = new Vector3 (0.0f, -10000.0f, 0.0f);		//	stick player under the map
			m_PointABool = true;
		}
		else
		{
			m_Timer = 5.0f;   //set amount of time to travel the crawl space
			m_Player.transform.position = new Vector3 (0.0f, -10000.0f, 0.0f);
			//	use coroutine to delay timer being set
					
				//	play animation crawl
			m_PointBBool = true;
		}
		
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			m_Player = obj.gameObject;
		}
	}

}