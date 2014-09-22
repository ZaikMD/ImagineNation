using UnityEngine;
using System.Collections;

public class ActivatableMovingPlatform : Activatable 
{
	bool m_CanBeActivated;
	bool m_IsActive;

	bool m_AtFinalDestination;
	bool m_AtDestination;
	bool m_IsReversing;

	float m_AtDestinationTimer;
	public float m_TimerLimit;

	public float m_PlatformSpeed;
	float m_DistanceToNextPlatform;

	public GameObject[] m_Destinations;
	GameObject m_CurrentDestination;
	GameObject m_NextDestination;
	int m_DestinationCounter;

	void Start () 
	{
		m_PlatformSpeed = 3.0f;
		m_TimerLimit = 3.0f;
		m_DestinationCounter = 0;
		m_CanBeActivated = true;
		m_IsActive = true;
	}

	void Update () 
	{
		//Platforms can only move if the switches are active
		if(CheckSwitches())
		{
			//If the platform is moving from one destination to another
			if(m_IsActive)
			{
				MoveToDestination();//Move the platform toward its destination

				//If the distance to the next platform is smaller than then the minimum required distance
				if(m_DistanceToNextPlatform <= 0.5f)
				{
					//Check if the current destination is at the last element,
					//if it is start reversing.
					if(m_DestinationCounter == (m_Destinations.Length -1))
					{
						m_IsReversing = true;
					}

					//If the current destination is the first element,
					//then move the platform forwards through the array
					if(m_DestinationCounter == 0)
					{
						m_IsReversing = false;
					}

					//Set inactive, and allow the platform to pause at each destination
					m_IsActive = false;
				}
			}
			else //If the platform is at destination
			{
				//Increment the timer while at the destination
				m_AtDestinationTimer += Time.deltaTime;

				//If the timer reached the limit
				if(m_AtDestinationTimer >= m_TimerLimit)
				{
					//Reset the timer
					m_AtDestinationTimer = 0.0f;

					//Determine whether to increment or decrement
					//the destination based on whether the path is 
					//going forwards or reversing.
					if(!m_IsReversing)
					{
						m_DestinationCounter++;
					}
					else
					{
						m_DestinationCounter--;
					}

					//Set to active and start moving the platform to the next destination
					m_IsActive = true;
				}
			}
		}
	}

	void MoveToDestination()
	{
		//Get the two positions required for calculation, the platforms current position
		//and the next destinations position
		Vector3 currentPosition = transform.position;
		Vector3 destinationPosition = m_Destinations [m_DestinationCounter].transform.position;

		//Get the direction vector between them
		Vector3 destinationDirection = destinationPosition - currentPosition;
		//Get the magnitude of that direction to use for a proximity check once close enough
		m_DistanceToNextPlatform = destinationDirection.magnitude;

		//Move the platform along that direction over time.
		transform.position += destinationDirection.normalized * m_PlatformSpeed * Time.deltaTime;
	}

}
