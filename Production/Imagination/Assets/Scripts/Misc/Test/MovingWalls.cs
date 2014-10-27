/// 
/// Moving walls.
/// Created By: Matthew Whitlaw
/// 
/// A script that is responsible for the movement of Moving walls
/// also known as "pushers". It will also request from the players
/// base movement ability to move the player as much as the pusher moved.
/// 

#region ChangeLog
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingWalls : MonoBehaviour 
{
	public GameObject m_DestinationObject;
	public float m_Speed;

	const float SPEED_MULTIPLIER = 1.2f;

	List<BaseMovementAbility> m_Players;

	private Vector3 m_OriginalPosition;
	private Vector3 m_DestinationPosition;
	private bool m_MovingForward;
	private float m_PushPlayerSpeed;

	
	void Start () 
	{
		//A list of base movement abilities that will represent the 
		//the players in the trigger
		m_Players = new List<BaseMovementAbility>();

		m_OriginalPosition = transform.position;
		m_DestinationPosition = m_DestinationObject.transform.position;
		m_MovingForward = true;
	}

	void Update () 
	{
		//Either the platform is moving forward or backward
		//pass in the correct destination based on current direction
		if(m_MovingForward)
		{
			Move (m_DestinationPosition);
		}
		else
		{
			Move(m_OriginalPosition);
		}
	}

	//The main function that moves the platform and requests the player movement
	void Move (Vector3 destination)
	{
		//Get the speed and distance between the pusher and the destination
		Vector3 speed = ((destination - transform.position).normalized * Time.deltaTime * m_Speed);
		Vector3 distance = transform.position - destination;

		if(distance.magnitude > speed.magnitude)
		{
			//Move the platform if the distance is greater than one frame of movement
			transform.position += speed;

			//If the pusher is moving forward then loop through the player list
			//and request basemovementability to move the player
			if(m_MovingForward)
			{
				for(int i = 0; i < m_Players.Count; i++)
				{
					m_Players[i].RequestInstantMovement(speed * SPEED_MULTIPLIER);
				}
				//Empty the list, if players are still in the trigger they'll be readded
				m_Players.Clear();
			}
		}
		else
		{
			//if the distance is less than one frame then set the destination to the
			//destination and reverse the direction
			transform.position = destination;
			m_MovingForward = !m_MovingForward;

			m_Players.Clear();
		}
	}

	//Add the players currently in the trigger to the list of players.
	void OnTriggerStay(Collider other)
	{
		BaseMovementAbility player = other.gameObject.GetComponent<BaseMovementAbility> ();

		if(player != null)
		{
			if(!m_Players.Contains(player))
			{
				m_Players.Add(player);
			}
		}
	}
}
