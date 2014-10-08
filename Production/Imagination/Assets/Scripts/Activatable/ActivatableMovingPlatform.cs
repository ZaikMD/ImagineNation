/// 
/// Activatable moving platform.
/// Created by: Matthew Whitlaw
/// 
/// This is the moving platform class that inherits from activatable and is responsible
/// for moving a platform prefab along a series of destinations.
/// 
/// IMPORTANT: When setting up a moving platform, have a series of empty game objects that
/// will act as the destinations, tag the platform as MovingPlatform, and ensure that the player
/// is tagged as Player.
/// 
///

#region Change Log 
#endregion

using UnityEngine;
using System.Collections;

public class ActivatableMovingPlatform : Activatable 
{
	bool m_IsActive;

	bool m_AtFinalDestination;
	bool m_AtDestination;
	bool m_IsReversing;
	public bool m_CanReverse;
	bool m_AtLastDest;

	float m_AtDestinationTimer;
	float m_TimerLimit;

	float m_PlatformSpeed;
	float m_DistanceToNextPlatform;

	public GameObject[] m_Destinations;
	GameObject m_CurrentDestination;
	GameObject m_NextDestination;
	int m_DestinationCounter;

	Vector3 m_AmountToMovePlayer;

	void Start () 
	{
		m_PlatformSpeed = 3.0f;
		m_TimerLimit = 3.0f;
		m_DestinationCounter = 0;
		m_IsActive = true;
		m_AtLastDest = false;
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
						m_AtLastDest = true;
						if(m_CanReverse)
						{
							m_IsReversing = true;
						}
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
				if(m_AtLastDest == true && m_CanReverse == false)
				{
					return;
				}
				//Increment the timer while at the destination
				m_AtDestinationTimer += Time.deltaTime;

				m_AmountToMovePlayer = Vector3.zero;

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
		else if(m_IsActive)
		m_AmountToMovePlayer = Vector3.zero;
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
		m_AmountToMovePlayer = destinationDirection.normalized * m_PlatformSpeed * Time.deltaTime;

	}

	//A getter function that player will call in order to get the direction and amount to 
	//move the player when on a moving player
	public Vector3 GetAmountToMovePlayer()
	{
		return m_AmountToMovePlayer;
	}

}
