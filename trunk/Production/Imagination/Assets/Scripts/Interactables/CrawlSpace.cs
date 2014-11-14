/*
 * Created by Greg Fortier
 * Date: Sept 24th, 2014
 *  
 * This script checks when the player enters the crawlspace, 
 * then teleports the player to a hidden area (while the player is teleporting there should be an animation of the player entering the crawlspace.
 * It then teleports the player again to the other end of the crawlspace
 * 
 * 
 *10/10/2014 Edit: Commented and cleaned code - Greg Fortier
 *Oct 17th 2014 Edit: Now rotates the player based on the rotation of the game object; - Greg Fortier
 *Oct 24th 2014 Edit: Adding the CameraSnap public function from TPCamera, so the camera snaps to behind the player when the player exits a crawl space. -Greg Fortier
 *Oct 29th 2014 Edit: Added 1 public GameObject called m_ExitPoint, exit point will be used to tell the player where to exit since during playtesting, players thaught the exit point was
 *too close to the tube itself -Greg Fortier
 *Oct 31st Edit: redesign of how crawlspace works, players now exit further out of the exit -Greg Fortier
*/

using UnityEngine;
using System.Collections;

public class CrawlSpace : MonoBehaviour 
{
	//Arrays are for member variables that need to be different from each other because they are used for individual players, so Element 0 is for player 1
	//used to determine if the player should be delayed when going through crawlspace
	float m_DelayChecker = 2.5f;
	float m_DelayValue = 0.5f;
	
	public float m_DelayTimer = 3.0f;
	public float[] m_CrawlDelay = new float[2];

	public CrawlSpace m_OtherCrawlSpace;
	public GameObject m_TemporaryHidingSpot;
	public GameObject m_OtherCrawlModelRotation;
	public GameObject m_ExitPoint;

	CharacterController[] m_Players = new CharacterController[2];

	int m_InComingPlayers = 0;

	TPCamera m_CameraSnapper;


	//increments m_InComingPlayer
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
					m_Players[i].transform.position = m_ExitPoint.gameObject.transform.position;
	
					//Decrements IncomingPlayer value on the other crawlspace
					m_OtherCrawlSpace.DecrementIncomingPlayer();

					if (m_OtherCrawlModelRotation != null)
					{
						//only moves on the y axis so that the player only rotates the way he's facing
						m_Players[i].transform.eulerAngles= new Vector3(0, m_OtherCrawlModelRotation.gameObject.transform.eulerAngles.y, 0);

						//camera snap is used to force the camera to be behind the player when the player exits a crawlspace
						m_Players[i].transform.parent.gameObject.GetComponentInChildren<TPCamera>().CameraSnap(true);
					}

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
			if (other.tag == Constants.PLAYER_STRING)
			{
				//Null check on the player CharacterController, happens if the first element is NULL
				if (m_Players[0] == null)
				{
					//if timer number 2 has more than 2.5 seconds left, Delay the timer incase of problems where players spawn in other players
					if(m_CrawlDelay[1] > m_DelayChecker)
					{
						m_CrawlDelay[0] = m_DelayTimer + m_DelayValue;
					}
					else
					{
						m_CrawlDelay[0] = m_DelayTimer;
					}

					//sets the first character controller element to the typeof charactercontroller
					m_Players[0] = (CharacterController)other.GetComponent(typeof (CharacterController));
					//increment incoming player
					m_OtherCrawlSpace.addIncomingPlayer();
				}

				//Happens when the first m_player controller is not null, (if the first element is not null we know its a second player entering the crawlspace
				else
				{
					if(m_CrawlDelay[0] > m_DelayChecker)
					{
						m_CrawlDelay[1] = m_DelayTimer + m_DelayValue;
					}
					else
					{
						m_CrawlDelay[1] = m_DelayTimer;
					}
					//sets the second character controller element to the typeof charactercontroller (which player it is)
					m_Players[1] = (CharacterController)other.GetComponent(typeof (CharacterController));
					m_OtherCrawlSpace.addIncomingPlayer();
				}
			}
		}
	}

	//Decrements m_InComingPlayer 
	public void DecrementIncomingPlayer()
	{
		if(m_InComingPlayers > 0)
		{
			m_InComingPlayers--;
		}
	}
}
