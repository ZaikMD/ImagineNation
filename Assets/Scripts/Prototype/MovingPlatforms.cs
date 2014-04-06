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
	private Vector3 m_InitialPosition;
	private bool m_IsPaused = false;
	//Bools
	private bool m_HasMoved = false;
	public  bool m_SwitchToggled = false;

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

	// Use this for initialization
	void Start () 
	{
		//Initialize everything, calculate the move percentages and movement time
		m_InitialPauseTime = m_PauseTime;
		m_InitialMoveTime = m_MoveTimeInSeconds;
		m_InitialPosition = transform.position;
		m_MoveTimeInMilliseconds = m_MoveTimeInSeconds * 60.0f;

		m_XDistance = m_Destination.transform.position.x - transform.position.x;
		m_YDistance = m_Destination.transform.position.y - transform.position.y;
		m_ZDistance = m_Destination.transform.position.z - transform.position.z;

		m_DestinationPosition = new Vector3 (transform.position.x + m_XDistance,transform.position.y + m_YDistance,transform.position.z + m_ZDistance);

		m_XMovePercent = m_XDistance/ m_MoveTimeInMilliseconds;
		m_YMovePercent = m_YDistance/ m_MoveTimeInMilliseconds;
		m_ZMovePercent = m_ZDistance/ m_MoveTimeInMilliseconds;

		GameManager.Instance.addObserver (this);

		m_Sender.addObserver (this);

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_IsPaused)
		{


				if (m_NeedsSwitch == false) //If it doesn't need a switch
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
							transform.Translate (m_XMovePercent, m_YMovePercent, m_ZMovePercent); //Move the platforms by the move percentage
							m_MoveTimeInSeconds -= Time.deltaTime; //Count down move time

							if (m_MoveTimeInSeconds < 0) 
							{
								m_PauseTime = m_InitialPauseTime; //Reset pause time
								m_HasMoved = true;
							} 
						} 
						else if (m_HasMoved == true && m_MovesOnce == false)  //Check to see if it needs to go back
						{
							transform.Translate (-1 * m_XMovePercent, -1 * m_YMovePercent, -1 * m_ZMovePercent); //Move the platform
							m_MoveTimeInSeconds -= Time.deltaTime;

							if (m_MoveTimeInSeconds < 0) 
							{
							m_PauseTime = m_InitialPauseTime; //Reset pause time
							m_HasMoved = false; // Has moved is now false
							}
						}
					}
				}
				else //Else it needs a switch
				{
					if(m_SwitchToggled == true) //Check to see if the switch is toggled
					{
						m_DelayTime -= Time.deltaTime; //Countdown delay timer
						if(m_DelayTime < 0)
						{

							if (m_PauseTime >= 0) //Check pause timer
							{
								m_PauseTime -= Time.deltaTime; //Countdown pause timer
								m_MoveTimeInSeconds = m_InitialMoveTime; //Set move time
							} 
							else if (m_HasMoved == false) 
							{
								transform.Translate (m_XMovePercent, m_YMovePercent, m_ZMovePercent); //Move platform by move percent
								m_MoveTimeInSeconds -= Time.deltaTime; //countdown move time
								
								if (m_MoveTimeInSeconds < 0)  //Check move time
								{
									m_PauseTime = m_InitialPauseTime; //reset pause time
									m_HasMoved = true; //Set has moved
								} 
							} 
							else if (m_HasMoved == true && m_MovesOnce == false) //Check to see if it needs to move again, and if it has moved already
							{
								transform.Translate (-1 * m_XMovePercent, -1 * m_YMovePercent, -1 * m_ZMovePercent); //Move back to original position
								m_MoveTimeInSeconds -= Time.deltaTime; //Countdown move time
								
								if (m_MoveTimeInSeconds < 0) 
								{
									m_PauseTime = m_InitialPauseTime; //reset pause time
									m_HasMoved = false; //Has moved is now false
								}
							}

					}
				}
			}


		}
	}

	void OnTriggerStay(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.transform.parent = this.transform; //Set the player as the child so it moves with the platform
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.transform.parent = null; //Delete the parent child relation
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
