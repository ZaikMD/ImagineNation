using UnityEngine;
using System.Collections;


public class LeverRotation : MonoBehaviour, Observer {
	
	//Trigger
	public Subject m_Subject;
	//Rotation Value
	public Vector3 m_RotationValue;
	
	//State Values
	bool m_IsPaused = false;
	bool m_ToggledOn = true;
	
	//Timer
	float m_RotateTimer = 0.0f;
	const float ROTATION_SPEED = 0.5f;
	
	
	// Use this for initialization
	void Start () 
	{
		// Adds to the list
		GameManager.Instance.addObserver (this);
		m_Subject.addObserver (this);

		//Set value to rotate each frame
		m_RotationValue /= ROTATION_SPEED;
	}
	
	// Rotate once per frame
	void Update () 
	{
		//If we are rotatin update rotation
		if (m_RotateTimer > 0.0f)
		{
			m_RotateTimer -= Time.deltaTime;

			if (m_RotateTimer < ROTATION_SPEED)
			{
				if (m_ToggledOn)
				{
					gameObject.transform.Rotate(m_RotationValue * Time.deltaTime);
				}
				else
				{
					gameObject.transform.Rotate(-m_RotationValue * Time.deltaTime);
				}
			}
		}
	}

	// Get events
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		// Pause
		if(recievedEvent == ObeserverEvents.PauseGame ||recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused; //toggles if you paused
		}

		//Toggle Lever Rotation
		if(recievedEvent == ObeserverEvents.Used)
		{
			if (m_ToggledOn && !m_IsPaused)
			{
				m_ToggledOn = false;
			}
			else if(!m_IsPaused)
			{
				m_ToggledOn = true;
			}

			//Fix rotation timer using lever mid toggle
			if (m_RotateTimer > 0.0f)
			{
				m_RotateTimer = ROTATION_SPEED - m_RotateTimer;
			}
			else
			{
				m_RotateTimer = ROTATION_SPEED;
			}
		}
	}
}