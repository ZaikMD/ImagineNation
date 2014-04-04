using UnityEngine;
using System.Collections;

public class MovingPlatforms : MonoBehaviour , Observer
{


	public float m_PauseTime;
	public float m_MoveTimeInSeconds;
	public float m_DelayTime;

	public bool m_NeedsSwitch;
	public bool m_MovesOnce;

	public GameObject m_Destination;
	
	private float m_InitialPauseTime;
	private float m_InitialMoveTime;
	private float m_MoveTimeInMilliseconds;

	private Vector3 m_DestinationPosition;
	private Vector3 m_InitialPosition;

	private bool m_HasMoved = false;
	public  bool m_SwitchToggled = false;

	private float m_XDistance;
	private float m_YDistance;
	private float m_ZDistance;

	private float m_XMovePercent;
	private float m_YMovePercent;
	private float m_ZMovePercent;

	bool m_IsPaused = false;

	public Subject m_Sender;

	// Use this for initialization
	void Start () 
	{
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


				if (m_NeedsSwitch == false) 
				{
					m_DelayTime -= Time.deltaTime;

					if(m_DelayTime < 0)
					{
						if (m_PauseTime >= 0) 
						{
							m_PauseTime -= Time.deltaTime;
							m_MoveTimeInSeconds = m_InitialMoveTime;
						} 
						else if (m_HasMoved == false) 
						{

							//transform.position = Vector3.Lerp(transform.position, m_LerpPosition, m_LerpTime);
							transform.Translate (m_XMovePercent, m_YMovePercent, m_ZMovePercent);
							m_MoveTimeInSeconds -= Time.deltaTime;

							if (m_MoveTimeInSeconds < 0) 
							{

								m_PauseTime = m_InitialPauseTime;
								m_HasMoved = true;
							} 
						} 
						else if (m_HasMoved == true && m_MovesOnce == false) 
						{
							//transform.position = Vector3.Lerp(transform.position, m_InitialPosition, m_LerpTime);
							transform.Translate (-1 * m_XMovePercent, -1 * m_YMovePercent, -1 * m_ZMovePercent);
							m_MoveTimeInSeconds -= Time.deltaTime;

							if (m_MoveTimeInSeconds < 0) 
							{
							m_PauseTime = m_InitialPauseTime;
							m_HasMoved = false;
							}
						}
					}
				}
				else
				{
					if(m_SwitchToggled == true)
					{
						m_DelayTime -= Time.deltaTime;
						if(m_DelayTime < 0)
						{

						if (m_PauseTime >= 0) 
						{
							m_PauseTime -= Time.deltaTime;
							m_MoveTimeInSeconds = m_InitialMoveTime;
						} 
						else if (m_HasMoved == false) 
						{
							//transform.position = Vector3.Lerp(transform.position, m_LerpPosition, m_LerpTime);
							transform.Translate (m_XMovePercent, m_YMovePercent, m_ZMovePercent);
							m_MoveTimeInSeconds -= Time.deltaTime;
							
							if (m_MoveTimeInSeconds < 0) 
							{
								
								m_PauseTime = m_InitialPauseTime;
								m_HasMoved = true;
							} 
						} 
						else if (m_HasMoved == true && m_MovesOnce == false) 
						{
							//transform.position = Vector3.Lerp(transform.position, m_InitialPosition, m_LerpTime);
							transform.Translate (-1 * m_XMovePercent, -1 * m_YMovePercent, -1 * m_ZMovePercent);
							m_MoveTimeInSeconds -= Time.deltaTime;
							
							if (m_MoveTimeInSeconds < 0) 
							{
								m_PauseTime = m_InitialPauseTime;
								m_HasMoved = false;
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
			obj.transform.parent = this.transform;
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.transform.parent = null;
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused;
		}

		if(recievedEvent == ObeserverEvents.Used && sender == m_Sender)
		{
			m_SwitchToggled = !m_SwitchToggled;
        }
	}
}
