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

	public GameObject[] m_ForwardDestinations;
	public GameObject[] m_ReverseDestinations;
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
		if(CheckSwitches())
		{
			if(m_IsActive)
			{
				MoveToDestination();

				if(m_DistanceToNextPlatform <= 0.5f)
				{
					m_IsActive = false;
				}
			}
			else
			{
				m_AtDestinationTimer += Time.deltaTime;

				if(m_AtDestinationTimer >= m_TimerLimit)
				{
					m_AtDestinationTimer = 0.0f;
					m_DestinationCounter++;
					m_IsActive = true;
				}
			}
		}
	}

	void MoveToDestination()
	{
		Vector3 currentPosition = transform.position;
		Vector3 destinationPosition = m_ForwardDestinations [m_DestinationCounter].transform.position;

		Vector3 destinationDirection = destinationPosition - currentPosition;
		m_DistanceToNextPlatform = destinationDirection.magnitude;

		transform.position += destinationDirection.normalized * m_PlatformSpeed * Time.deltaTime;
	}

}
