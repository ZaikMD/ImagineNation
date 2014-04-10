using UnityEngine;
using System.Collections;


// Will Require modification to every script called playerstatemachine to whatever our player script is
public class CheckpointManager : MonoBehaviour 
{

	public Checkpoint m_CurrentCheckPoint;
	
	bool m_DoesAlexHaveSecondItem;
	bool m_DoesDerekHaveSecondItem;
	bool m_DoesZoeyHaveSecondItem;

	public GameObject m_CurrentPlayer;
	
	//bool m_CollectiblesFound[amount of collectibles in level];
	
	//setting up class as singleton
	public static CheckpointManager m_Instance{ get; private set; }
	
	//assigning the instance to our variable
	void Awake()
	{
		if(m_Instance != null)
		{
			if (m_Instance != this)
			{
				Destroy(gameObject);
			}
		}
		m_Instance = this;
	}	
	
	void Start()
	{
	}
	
	//Code possibly be used for saving or states. 
	public Checkpoint getCurrentCheckPoint()
	{
		return m_CurrentCheckPoint;
	}
	
//	public void setCurrentCheckPoint(Checkpoint newCheckPoint, PlayerScript playerScript) 
//	{
//		// in here we will set all the items checkpoint manager keeps track using the 
//		// passed in variables
//		
//		m_CurrentCheckPoint = newCheckPoint;
//		// Requires function from the player
//		//m_DoesAlexHaveSecondItem = playerScript.m_HaveSecondItem;
//		//m_DoesAlexHaveSecondItem = playerScript.m_HaveSecondItem;
//		//m_DoesAlexHaveSecondItem = playerScript.m_HaveSecondItem;
//	}
	
	//public void RespawnPlayer(GameObject player)
//	{
//		PlayerScript playerScript = player.GetComponent(PlayerScript);
//		
//		if(player->gameObject.name == "Zoey")
//		{
//			playerScript.m_HaveSecond = m_DoesZoeyHaveSecondItem;
//		}
//		if(player->gameObject.name == "Derek")
//		{
//			playerScript.m_HaveSecond = m_DoesDerekHaveSecondItem;
//		}
//		if(player->gameObject.name == "Alex")
//		{
//			playerScript.m_HaveSecond = m_DoesAlexHaveSecondItem;
//		}
//
//		//check to see if player one;
//		if(m_CurrentPlayer.GetComponent<PlayerState>().m_IsActive)
//		{
//			m_CurrentPlayer.transform.position = m_CurrentCheckPoint.transform.position;
//			//m_CurrentPlayer..GetComponent<PlayerState>().m
//
//		}
//		else
//		{			//if not player one spawn them slightly over so they don't spawn in the same spot.
//			player->transform.position = m_CurrentCheckPoint.transform.position + vec3(0.5, 0, 0);
//		}
////		
//	}

}
