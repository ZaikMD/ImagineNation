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
/*
 * 11/19/2014 - Cleaned and optimized code - Jason Hein
 * 11/24/2014 - Added functionality to push to player - Jason Hein
 */
#endregion

using UnityEngine;
using System.Collections;

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
	public float m_PlatformSpeed = 2.5f;

	//Only necassary for sound que (fix with a sound manager in future)
	public bool m_IsGate = false;

	//Timers
	float m_AtDestinationTimer = -1.0f;
	float m_DistanceToNextPlatform;

	//GameObjects
	int m_DestinationIndex = 0;

	//Amount to move the player
	Vector3 m_AmountToMovePlayer = Vector3.zero;

	//Sound manager
	SFXManager m_SFX;

	//Initialization
	void Start () 
	{
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
	}

	//Update the moving platform
	void Update () 
	{
		//Play a stopping sound
		if(!CheckSwitches() || (m_Loops == false && m_DestinationIndex == m_Destinations.Length - 1))
		{
			m_SFX.stopSound(this.gameObject);
			return;
		}

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
					transform.position = m_Destinations[m_DestinationIndex].position;

					//Set the destination timer
					m_AtDestinationTimer = TIME_PAUSED_AT_PLATFORM_DESTINATION;

					//Check if we are looping, or if we should increment the destination
					if(m_DestinationIndex == (m_Destinations.Length -1) && m_Loops)
					{
						m_DestinationIndex = 0;
					}
					else if (m_DestinationIndex < m_Destinations.Length -1)
					{
						m_DestinationIndex++;
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

	//Moves the platform towards the next destination
	void MoveToDestination()
	{
		//Get the the next destinations position
		Vector3 destinationPosition = m_Destinations [m_DestinationIndex].position;

		//Get the direction vector between them
		Vector3 destinationDirection = destinationPosition - transform.position;

		//Get the magnitude of that direction to use for a proximity check once close enough
		m_DistanceToNextPlatform = destinationDirection.magnitude;

		//Move the platform along that direction over time
		m_AmountToMovePlayer = destinationDirection.normalized * m_PlatformSpeed * Time.deltaTime;
		transform.position += m_AmountToMovePlayer;
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
			m_SFX.playSound(this.gameObject, Sounds.GateOpen);
		}
	}

	//When the player collides with this platform
	void OnCollisionStay (Collision collision)
	{
		//Only move players
		if (collision.gameObject.tag != Constants.PLAYER_STRING)
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

		//Calculate the amount to move the player
		Vector3 amountToMove = Vector3.zero;

		//Check if the player is on top of the platform
		if (Vector3.Dot(Vector3.up, averageNormal) > 0.5f)
		{
			//Move the player as much at this platform is moving
			amountToMove = m_AmountToMovePlayer * 1.2f;
		}
		//Check if we are in front of the platform
		else if (Vector3.Dot(m_AmountToMovePlayer, averageNormal) > 0.0f)
		{
			//Move the layer based on how much we are overlapping the player
			Vector3 playerMovement = movement.GetMovementThisFrame();
			playerMovement.y = 0.0f;
			amountToMove = Vector3.Scale(averageNormal, m_AmountToMovePlayer * 1.2f - playerMovement);
			amountToMove.y = 0.0f;
		}

		//Move the player
		movement.RequestInstantMovement(amountToMove);
	}
}
