﻿/// 
/// Activatable moving platform.
/// Created by: Matthew Whitlaw
/// 
/// This is the moving platform class that inherits from activatable and is responsible
/// for moving a platform prefab along a series of destinations.
/// 
/// IMPORTANT: When setting up a moving platform, have a series of empty game objects that
/// will act as the destinations, tag the platform as MovingPlatform, and ensure that the player
/// is tagged as Player. if you would like the platform to calculate its own speed based on a time,
/// uncheck use constant speed, and input time in the time array. make the array the size of the amount
/// of destinations array. element 0 will be the time to return to the begining. 1 onwards there 
/// corosponding desitination.
/// 
///  
/// IPORTANT: If only one desitination, in cases like gate, use constant speed.
///  
/// The platform must also have the MovingPlatformLayer as it's layer, and must have a rigidbody component (turn off gravity and turn on all constraints).
///

#region Change Log 
/*
 * 11/19/2014 - Cleaned and optimized code - Jason Hein
 * 11/24/2014 - Added functionality to push to player - Jason Hein
 * 11/25/2014 - Fixed gate bug, fixed potential bug for player on top of a platform when it stops
 * 2/6/2015   - Added ability to calculate speed based off of time. - Kole 
 */
#endregion

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Activatable moving platform.
/// </summary>
public class ActivatableMovingPlatform : Activatable 
{
	//Constants
	const float MIN_DIST_TO_NEXT_PLATFORM = 0.3f;
	const float TIME_PAUSED_AT_PLATFORM_DESTINATION = 3.0f;

	public Transform[] m_Destinations;
	public bool m_Loops = false;
	public bool m_UseConstantSpeed = true;
	public float m_PlatformSpeed = 2.5f;

	public float[] m_TimeForPlatforms;


	//Only necassary for sound que (fix with a sound manager in future)
	public bool m_IsGate = false;

	//Timers
	float m_AtDestinationTimer = -1.0f;
	float m_DistanceToNextPlatform;

	//Indexs
	int m_PreviousDestinationIndex = 0;
	int m_DestinationIndex = 1;

	//Amount to move the player
	Vector3 m_AmountToMovePlayer = Vector3.zero;
	Vector3 m_TargetMove = Vector3.zero;
	public float i_MoveLerpAmount = 0.05f;


	const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.PauseMenu;

	//For moving the player
	struct PlayersToMove
	{
		public BaseMovementAbility movement;
		public Vector3 normal;
		public bool foundLastFrame;

		public PlayersToMove (BaseMovementAbility aMovement, Vector3 aNormal, bool wasFoundLastFrame)
		{
			movement = aMovement;
			normal = aNormal;
			foundLastFrame = wasFoundLastFrame;
		}
	}
	List<PlayersToMove> m_PlayersToMove;
	
	//Sound manager
	SFXManager m_SFX;

	//Initialization
	void Start () 
	{
        m_SFX = SFXManager.Instance;
		m_PlayersToMove = new List<PlayersToMove> ();

		if(m_Destinations.Length < 2)
		{
			m_DestinationIndex = 0;
		}
	}

	//Update the moving platform
	void Update () 
	{ 
		if (PauseScreen.shouldPause(PAUSE_LEVEL)){return;}

		//Platforms can only move if the switches are active
		if(CheckSwitches())
		{
			//If the platform is moving from one destination to another
			if(m_AtDestinationTimer < 0.0f)
			{
				CheckPlayGateSound();

				//Move the platform toward its destination
				MoveToDestination();

				//If the distance to the next platform is smaller than then the minimum required distance
				if(m_DistanceToNextPlatform < MIN_DIST_TO_NEXT_PLATFORM)
				{
					//Set the platform to be directly at the destination
					//Vector3 amountToMove = m_Destinations[m_DestinationIndex].position - transform.position;

					m_TargetMove = Vector3.zero;

					//transform.position += amountToMove;

					//Check if their are any players on the platform
					if (m_PlayersToMove.Count > 0)
					{
						for (int i = 0; i < m_PlayersToMove.Count; i++)
						{
							//Check if the player is on top of the platform
							if (Vector3.Dot(Vector3.up, m_PlayersToMove[i].normal) > 0.4f)
							{
								//Make absolutly sure that the player is still above the platform
								//amountToMove.y += 0.01f;

								//Move the player as much at this platform is moving
								m_PlayersToMove[i].movement.RequestInstantMovement(Vector3.up * 0.01f);
							}

						}
					}

					//Set the destination timer
					m_AtDestinationTimer = TIME_PAUSED_AT_PLATFORM_DESTINATION;

					//Check if we are looping, or if we should increment the destination
					if(m_DestinationIndex == (m_Destinations.Length -1) && m_Loops)
					{
						m_PreviousDestinationIndex = m_DestinationIndex;
						m_DestinationIndex = 0;
					}
					//Choose next destination
					else if (m_DestinationIndex < m_Destinations.Length -1)
					{
						m_PreviousDestinationIndex = m_DestinationIndex;
						m_DestinationIndex++;
					}
					//We are supposed to go no further
					else
					{
						//Stop the platform forever
						m_SFX.stopSound(transform, Sounds.GateOpen, true);
						Destroy(this);
					}
				}
			}

			//If the platform is at the destination
			else
			{
				//Increment the timer while at the destination
				m_AtDestinationTimer -= Time.deltaTime;

				//Do not move the player while the platform is not moving
				if (m_AmountToMovePlayer != Vector3.zero)
				{
					m_AmountToMovePlayer = Vector3.zero;
				}
			}
		}

		//If The switch has not on, do not move the player
		else if (m_AmountToMovePlayer != Vector3.zero)
		{
			m_AmountToMovePlayer = Vector3.zero;
		}
	}

	float CalculateSpeed(float timeToReach, int DestinationOne, int DestinationTwo)
	{
		float Distance = Vector3.Distance(m_Destinations[DestinationOne].position, m_Destinations[DestinationTwo].position);
		return Distance / timeToReach;
	}

	//Moves the platform towards the next destination
	void MoveToDestination()
	{
		//Get the the next destinations position
		Vector3 destinationPosition = m_Destinations [m_DestinationIndex].position;

		//Get the direction vector between them
		Vector3 destinationDirection = destinationPosition - transform.position;

		//Get the magnitude of that direction to use for a proximity check once close enough
		m_DistanceToNextPlatform = destinationDirection.magnitude;

		float Speed;

		if(m_UseConstantSpeed)
		{
			Speed = m_PlatformSpeed;
		}
		else
		{
			if(m_TimeForPlatforms.Length < m_DestinationIndex)
			{
				Debug.Log("Problem");
			}

			Speed = CalculateSpeed(m_TimeForPlatforms[m_DestinationIndex], m_DestinationIndex, m_PreviousDestinationIndex); 
		}

		//Move the platform along that direction over time
		m_AmountToMovePlayer = destinationDirection.normalized * Speed * Time.deltaTime;
		if(m_AmountToMovePlayer.magnitude > m_DistanceToNextPlatform)
		{
			m_AmountToMovePlayer = m_AmountToMovePlayer.normalized * m_DistanceToNextPlatform;
		}

		m_TargetMove += m_AmountToMovePlayer;

		Vector3 move = Vector3.Lerp(Vector3.zero, m_TargetMove, i_MoveLerpAmount);
		transform.position += move;
		m_TargetMove -= move;

		//Check if their are any players on the platform
		if (m_PlayersToMove.Count > 0)
		{
			for (int i = 0; i < m_PlayersToMove.Count; i++)
			{
				//Calculate the amount to move the player
				Vector3 amountToMove = Vector3.zero;

				//Get related variables from the struct
				BaseMovementAbility movement = m_PlayersToMove[i].movement;
				Vector3 normal = m_PlayersToMove[i].normal;
				
				//Check if the player is on top of the platform
				if (Vector3.Dot(Vector3.up, normal) > 0.5f)
				{
					//Move the player as much at this platform is moving
					amountToMove = move;
					amountToMove.y += 0.01f;
				}
				//Check if we are in front of the platform
				else if (Vector3.Dot(move, normal) > 0.0f)
				{
					//Move the layer based on how much we are overlapping the player
					Vector3 playerMovement = movement.GetMovementThisFrame();
					playerMovement.y = 0.0f;
					amountToMove = Vector3.Scale(normal, move - playerMovement);
					amountToMove.y = 0.0f;
				}
				
				//Move the player
				movement.RequestInstantMovement(amountToMove);

				if (m_PlayersToMove[i].foundLastFrame == false)
				{
					Debug.Log("removing: " + m_PlayersToMove[i].movement.gameObject.name);
					m_PlayersToMove.RemoveAt(i);
				}
			}
		}
	}

	/// <summary>
	/// Returns the amount that the player should move in order to follow the moving platform.
	/// </summary>
	/// <returns>The amount to move player.</returns>
	public Vector3 GetAmountToMovePlayer()
	{
		return m_AmountToMovePlayer;
	}

	//If this is a gate then play gate sound
	void CheckPlayGateSound()
	{
		if(m_IsGate)
		{
			m_SFX.playSound(transform, Sounds.GateOpen);
		}
	}

	//When the player collides with this platform
	void OnCollisionStay (Collision collision)
	{
		//Only move players
		if (collision.gameObject.tag != Constants.COLLIDE_WITH_MOVING_PLATFORM_STRING)
		{
			return;
		}

		//Null check for the movement ability
		BaseMovementAbility movement = (BaseMovementAbility)collision.transform.parent.gameObject.GetComponent<BaseMovementAbility> ();
		if (movement == null)
		{
			return;
		}

		//Get the average direction that the player has collided with the moving platform
		ContactPoint[] contacts = collision.contacts;
		Vector3 averageNormal = Vector3.zero;
		foreach (ContactPoint contactPoint in contacts)
		{
			averageNormal += contactPoint.normal;
			Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.white);
		}
		averageNormal /= -contacts.Length;

		//Check if the player should be added or changed
		bool found = false;
		for (int i =0; i < m_PlayersToMove.Count; i++)
		{
			if (movement == m_PlayersToMove[i].movement)
			{
				m_PlayersToMove[i] = new PlayersToMove(movement, averageNormal, true);
				found = true;
			}
		}
		if (found == false)
		{
			m_PlayersToMove.Add(new PlayersToMove(movement, averageNormal, true));	
			Debug.Log(m_PlayersToMove[m_PlayersToMove.Count-1].movement.gameObject.name+ "   added");
		}
	}

	void OnCollisionExit (Collision collision)
	{
		//Only move players
		if (collision.gameObject.tag != Constants.COLLIDE_WITH_MOVING_PLATFORM_STRING)
		{
			return;
		}
		
		//Null check for the movement ability
		BaseMovementAbility movement = (BaseMovementAbility)collision.transform.parent.gameObject.GetComponent<BaseMovementAbility> ();
		if (movement == null)
		{
			return;
		}
		
		//Check if the player should be added or changed
		for (int i =0; i < m_PlayersToMove.Count; i++)
		{
			if (movement == m_PlayersToMove[i].movement)
			{
				//Check if the player should be removed from the array or flaged to be removed next frame
				if (m_PlayersToMove[i].foundLastFrame == true)
				{
					m_PlayersToMove[i] = new PlayersToMove(m_PlayersToMove[i].movement, m_PlayersToMove[i].normal, false);
				}
				else
				{
					//Debug.Log(m_PlayersToMove[i].movement.gameObject.name+ "   removed");
					//m_PlayersToMove.RemoveAt(i);
				}
			}
		}
	}
}
