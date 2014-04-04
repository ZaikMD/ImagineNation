using UnityEngine;
using System.Collections;

public class SeeSaw : InteractableBaseClass, Observer
{

	//Players
	private GameObject m_SittingPlayer;
	GameObject m_JumpingPlayer;

	//Seesaw points
	public GameObject m_JumpPoint;
	public GameObject m_SitPoint;
	public GameObject m_JumpEndPoint;

	//States
	bool m_IsLerping;
	bool m_HasLaunchedPlayer;
	bool m_IsPlatformLerping = false;

	bool m_PlayerHasJumped = false;

	//Points
	Vector3 m_JumpPointPos;
	Vector3 m_SitPointPos;

	Vector3 m_SitPointEndPos;

	//Timer
	private float m_ResetTimer = 5.0f;
	const float LERP_TIME = 0.05f;

	bool m_IsEnabled = true;

	public float m_LaunchHeight = 10.0f;

	//Initialization
	void Start()
	{
		//Initialize the jump and sit points.
		m_JumpPointPos = m_JumpPoint.transform.position;
		m_SitPointPos = m_SitPoint.transform.position;
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;

		m_SitPointEndPos = new Vector3 (m_SitPoint.transform.position.x, m_SitPoint.transform.position.y + m_LaunchHeight, m_SitPoint.transform.position.z);


		m_Type = InteractableType.SeeSaw;
		
		m_IsExitable = true;

		GameManager.Instance.addObserver (this);

	}

	//Launch
	void Update()
	{
		if(m_IsEnabled)
		{
			if(m_IsLerping)
			{

				//m_JumpingPlayer.transform.position = Vector3.Lerp (m_JumpingPlayer.transform.position, new Vector3(m_JumpPoint.transform.position.x,m_JumpPoint.transform.position.y + 1,m_JumpPoint.transform.position.z)  , LERP_TIME*2); //Lerp m_JumpingPlayer to m_JumpPoint
				if(!m_PlayerHasJumped)
				{
					m_JumpingPlayer.gameObject.GetComponent<PlayerMovement>().Jump();
				}

				if(m_PlayerHasJumped)
				{
					m_JumpingPlayer.transform.position = Vector3.Lerp(m_JumpingPlayer.transform.position, m_JumpEndPoint.transform.position, LERP_TIME);
				}

				if(m_JumpingPlayer.transform.position.y >= m_JumpPoint.transform.position.y + 0.5f)
				{
				
					m_IsPlatformLerping = true;
					//If the jumping player has reached the jump point, then notify the player and give back control

					if(m_JumpPoint.transform.position.y <= m_JumpEndPoint.transform.position.y)
					{
						m_JumpingPlayer.transform.parent = null;
						m_JumpingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting();
						m_IsLerping = false;
						Debug.Log("PLayerLaunched");
					}
				}
			}

			if(m_IsPlatformLerping == true)
			{
				m_SitPoint.transform.position = Vector3.Lerp(m_SitPoint.transform.position, m_SitPointEndPos, LERP_TIME);
				m_JumpPoint.transform.position = Vector3.Lerp(m_JumpPoint.transform.position, m_JumpEndPoint.transform.position, LERP_TIME);

				launchPlayer();
			}
			if(m_HasLaunchedPlayer == true)
			{
				m_ResetTimer -= Time.deltaTime;

			}
			if(m_ResetTimer < 0.0f)
			{
				reset();
			}

		}
	}

	//Sit
	public void makeChild(GameObject obj)  //Called by player
	{
		if(obj != null)
		{
			//Set m_SittingPlayer to obj 
			m_SittingPlayer = obj.gameObject; 
			
			//Make obj the child of the SeeSaw
			m_SittingPlayer.transform.parent = this.transform;
			
			//Set the obj's position to m_SitPoint's position
			m_SittingPlayer.transform.position = m_SitPointPos;
		} 

		if(obj == null)
		{
			m_SittingPlayer = null;
		}


	}

	//Launch
	void launchPlayer()
	{
		if(!m_HasLaunchedPlayer)
		{
			if(m_SittingPlayer != null)
			{
				m_SittingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting();

				m_SittingPlayer.gameObject.GetComponent<PlayerMovement>().LaunchJump(50);

				m_SittingPlayer.transform.parent = null;
				m_SittingPlayer = null;
				m_HasLaunchedPlayer = true; 
				//Terminate Parent-child relation between m_SittingPlayer and the SeeSaw
			}

		}

	}

	//This gets called by DivingBoard to jump
	public void playerJumping(GameObject obj)
	{
		//Set m_JumpingPlayer to obj
		m_JumpingPlayer = obj.gameObject;

		//Make obj the child of the SeeSaw
		m_JumpingPlayer.transform.parent = this.transform;
		m_IsLerping = true; //Start the lerp to m_JumpPoint
	}

	//This gets called by the sitting player to exit
	/*void exitSeeSaw(GameObject obj)
	{
		//Terminate parent-child relation between m_SittingPlayer and SeeSaw
		m_SittingPlayer.transform.parent = null;

		//Clear m_SittingPlayer
		m_SittingPlayer = null;
	} */

	//This should be called after the player is launched, resetting the SeeSaw back to it's original Position
	void reset()
	{
		//Reset points back to original positions
		m_SitPoint.transform.position = m_SitPointPos;
		m_JumpPoint.transform.position = m_JumpPointPos;


		//Clear m_JumpingPlayer and m_SittingPlayer;
		if(m_JumpingPlayer != null)
		{
			m_JumpingPlayer.transform.parent = null;
			m_JumpingPlayer = null;
		}

		if(m_SittingPlayer!= null)
		{
			m_SittingPlayer.transform.parent = null;
			m_SittingPlayer = null;
		}

		//Reset states and timer
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;
		m_IsPlatformLerping = false;
		m_PlayerHasJumped = false;
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame ||recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsEnabled = !m_IsEnabled;
		}
	}
}
