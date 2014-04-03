using UnityEngine;
using System.Collections;

public class SeeSaw : InteractableBaseClass, Observer
{

	//Players
	public GameObject m_SittingPlayer;
	GameObject m_JumpingPlayer;

	//Seesaw points
	public GameObject m_JumpPoint;
	public GameObject m_SitPoint;
	public GameObject m_JumpEndPoint;

	//States
	bool m_IsLerping;
	bool m_HasLaunchedPlayer;

	//Points
	Vector3 m_JumpPointPos;
	Vector3 m_SitPointPos;

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
				m_JumpPoint.transform.position = Vector3.Lerp(m_JumpPoint.transform.position, m_JumpEndPoint.transform.position, LERP_TIME);
				m_JumpingPlayer.transform.position = Vector3.Lerp (m_JumpingPlayer.transform.position, m_JumpPoint.transform.position , LERP_TIME); //Lerp m_JumpingPlayer to m_JumpPoint
				if(m_JumpingPlayer.transform.position.y <= m_JumpPoint.transform.position.y + 0.5f)
				{
					m_JumpingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting();

					//If the jumping player has reached the jump point, then notify the player and give back control
					m_JumpingPlayer.transform.parent = null;



					//Also m_IsLerping = false && call launchPlayer();
					m_IsLerping = false;
					launchPlayer();
				}
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
		//Set m_SittingPlayer to obj 
		m_SittingPlayer = obj.gameObject; 

		//Make obj the child of the SeeSaw
		m_SittingPlayer.transform.parent = this.transform;

		//Set the obj's position to m_SitPoint's position
		m_SittingPlayer.transform.position = m_SitPointPos;

	}

	//Launch
	void launchPlayer()
	{
		m_SittingPlayer.gameObject.GetComponent<PlayerState>().exitInteracting();

		m_SittingPlayer.gameObject.GetComponent<PlayerMovement>().LaunchJump(m_LaunchHeight);

		m_SitPoint.transform.Translate (0, 10.0f, 0.0f);
		m_SittingPlayer = null;
		m_HasLaunchedPlayer = true;
		//Terminate Parent-child relation between m_SittingPlayer and the SeeSaw
		m_SittingPlayer.transform.parent = null;

		//Apply force to m_SittingPlayer
		//m_SittingPlayer.transform.Translate (0, 10.0f, 0.0f);



		//Move the platform up as well


	}

	//This gets called by DivingBoard to jump
	public void playerJumping(GameObject obj)
	{
		//Set m_JumpingPlayer to obj
		m_JumpingPlayer = obj.gameObject;

		//Make obj the child of the SeeSaw
		m_JumpingPlayer.transform.parent = this.transform;
		m_JumpingPlayer.transform.Translate (1.0f, 1.0f, 0.0f);

		m_IsLerping = true; //Start the lerp to m_JumpPoint
	}

	//This gets called by the sitting player to exit
	void exitSeeSaw(GameObject obj)
	{
		//Terminate parent-child relation between m_SittingPlayer and SeeSaw
		m_SittingPlayer.transform.parent = null;

		//Clear m_SittingPlayer
		m_SittingPlayer = null;
	}

	//This should be called after the player is launched, resetting the SeeSaw back to it's original Position
	void reset()
	{
		//Reset points back to original positions
		m_SitPoint.transform.position = m_SitPointPos;
		m_JumpPoint.transform.position = m_JumpPointPos;

		//Terminate any Parent-Child relations
		m_JumpingPlayer.transform.parent = null;
		m_SittingPlayer.transform.parent = null;

		//Clear m_JumpingPlayer and m_SittingPlayer;
		m_JumpingPlayer = null;
		m_SittingPlayer = null;

		//Reset states and timer
		m_ResetTimer = 5.0f;
		m_HasLaunchedPlayer = false;
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
