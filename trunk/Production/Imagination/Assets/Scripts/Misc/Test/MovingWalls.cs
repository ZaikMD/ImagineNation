/// 
/// Moving walls.
/// Created By: Matthew Whitlaw
/// 
/// A script that is responsible for the movement of Moving walls
/// also known as "pushers". It will also request from the players
/// base movement ability to move the player as much as the pusher moved.
/// 

#region ChangeLog
/*
 * 28/10/2014 - Added more comments to code
 */
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingWalls : MonoBehaviour 
{
	//Destination that can be set by designers
	public GameObject m_DestinationObject;

	//Speed of movement
	public float m_Speed = 2.0f;
	const float SPEED_MULTIPLIER = 1.2f;

	//Players being pushed
	List<BaseMovementAbility> m_Players;

	//Positions to lerp to
	private Vector3 m_OriginalPosition;
	private Vector3 m_DestinationPosition;

	//States
	private bool m_MovingForward;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.PauseMenu;

	//Initialization
	void Start () 
	{
		//A list of base movement abilities that will represent the 
		//the players in the trigger
		m_Players = new List<BaseMovementAbility>();

		m_OriginalPosition = transform.position;
		m_DestinationPosition = m_DestinationObject.transform.position;
		m_MovingForward = true;
	}

	//Update the pusher
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

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
		Vector3 distance = destination - transform.position;
		Vector3 speed = distance.normalized * m_Speed * Time.deltaTime;

		//If the distance is greater than one frame of movement
		if(distance.magnitude > speed.magnitude)
		{
			//Move the platform
			transform.position += speed;

			//Loop through the player list and request basemovementability to move the player
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
		//If the distance is less than one frame 
		else
		{
			//Set the destination to the destination and reverse the direction
			transform.position = destination;
			m_MovingForward = !m_MovingForward;

			//Empty the list, if players are still in the trigger they'll be readded
			m_Players.Clear();
		}
	}

	//Add the players currently in the trigger to the list of players.
	void OnTriggerStay(Collider other)
	{
		//Gets the basemovement ability component from the objects
		BaseMovementAbility player = other.gameObject.GetComponent<BaseMovementAbility> ();

		//If the component existsts then add it to the list
		if(player != null)
		{
			if(!m_Players.Contains(player))
			{
				m_Players.Add(player);
			}
		}
	}
}
