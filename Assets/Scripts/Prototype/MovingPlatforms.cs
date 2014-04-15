/*


Created by some douche


4/13/2014
	Fixed falling through platform
*/ 



using UnityEngine;
using System.Collections;

public class MovingPlatforms : MonoBehaviour , Observer
{

	//Public floats for designers
	public float m_PauseTime;
	public float m_MoveTimeInSeconds;
	public float m_DelayTime;

	//Bools for designers
	public bool m_NeedsSwitch;
	public bool m_MovesOnce;

	//The destination
	public GameObject m_Destination;

	//Floats
	private float m_InitialPauseTime;
	private float m_InitialMoveTime;
	private float m_MoveTimeInMilliseconds;

	//Positions
	private Vector3 m_DestinationPosition;
	private bool m_IsPaused = false;
	//Bools
	private bool m_HasMoved = false;
	public  bool m_SwitchToggled = true;

	//Distances
	private float m_XDistance;
	private float m_YDistance;
	private float m_ZDistance;

	//Move percentages per second
	private float m_XMovePercent;
	private float m_YMovePercent;
	private float m_ZMovePercent;

	//Subject
	public Subject m_Sender;

	// Platforms Movement Vector
	Vector3 m_Movement;

	GameObject m_Player;

	// Use this for initialization
	void Start () 
	{
		//Initialize everything, calculate the move percentages and movement time
		m_InitialPauseTime = m_PauseTime;
		m_InitialMoveTime = m_MoveTimeInSeconds;
		m_MoveTimeInMilliseconds = m_MoveTimeInSeconds * 60.0f;

		m_XDistance = m_Destination.transform.position.x - transform.position.x;
		m_YDistance = m_Destination.transform.position.y - transform.position.y;
		m_ZDistance = m_Destination.transform.position.z - transform.position.z;

		m_DestinationPosition = new Vector3 (transform.position.x + m_XDistance,transform.position.y + m_YDistance,transform.position.z + m_ZDistance);

		m_XMovePercent = m_XDistance/ m_MoveTimeInMilliseconds;
		m_YMovePercent = m_YDistance/ m_MoveTimeInMilliseconds;
		m_ZMovePercent = m_ZDistance/ m_MoveTimeInMilliseconds;

		GameManager.Instance.addObserver (this);

		if (m_Sender)
		{
			m_Sender.addObserver (this);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_IsPaused)
		{
			if (m_SwitchToggled == true) //If it doesn't need a switch
			{
				m_DelayTime -= Time.deltaTime; //Count down the delay timer

				if(m_DelayTime <= 0)
				{
					if (m_PauseTime >= 0) //Check the pause timers
					{
						m_PauseTime -= Time.deltaTime; //Count down pause timer and set the move time
						m_MoveTimeInSeconds = m_InitialMoveTime;
					} 
					else if (m_HasMoved == false) //Check to see if it's moved already
					{
						m_Movement = new Vector3(m_XMovePercent, m_YMovePercent, m_ZMovePercent);
						if(m_Player != null)
						{
							m_Player.GetComponent<PlayerMovement>().Move(m_Movement + transform.up * 0.01f);
							m_Player.GetComponent<PlayerMovement>().Move(transform.up * -0.02f);
						}
						transform.Translate(m_Movement); ; //Move the platforms by the move percentage
						m_MoveTimeInSeconds -= Time.deltaTime; //Count down move time

						if (m_MoveTimeInSeconds < 0) 
						{
							m_PauseTime = m_InitialPauseTime; //Reset pause time
							m_HasMoved = true;
						} 
					} 
					else if (m_HasMoved == true && m_MovesOnce == false)  //Check to see if it needs to go back
					{
						m_Movement = new Vector3(-1 * m_XMovePercent, -1 * m_YMovePercent, -1 * m_ZMovePercent);
						if(m_Player != null)
						{
							m_Player.GetComponent<PlayerMovement>().Move(m_Movement + transform.up * 0.01f);
							m_Player.GetComponent<PlayerMovement>().Move(transform.up * -0.02f);
						}
						transform.Translate (m_Movement); //Move the platform
						m_MoveTimeInSeconds -= Time.deltaTime;
			

						if (m_MoveTimeInSeconds < 0) 
						{
							m_PauseTime = m_InitialPauseTime; //Reset pause time
							m_HasMoved = false; // Has moved is now false
						}
					}
				}
			}
		}
	}


	void OnTriggerStay(Collider obj)
	{
		if (obj.tag == "Player") 
		{
			m_Player = obj.gameObject;
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if (obj.tag == "Player") 
		{
			if(m_Player != null)
			{
				m_Player = null;
			}
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused; //For activating the platform is the game isn't paused
		}

		if(recievedEvent == ObeserverEvents.Used && sender == m_Sender)
		{
			m_SwitchToggled = !m_SwitchToggled; //For activating the platform if it needs a switch
        }
	}
}
