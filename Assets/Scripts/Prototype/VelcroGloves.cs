/*
TO USE:

Attach this script to the player

*Uncomment the player class related comments, if the player class  is implemented*

Tag wall as a "VelcroWall"

This wall and the player must both have trigger box/capsule colliders, and the walls trigger box collider should be a little bit bigger than the wall


Created by Jason Hein on 3/23/2014
*/





using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(PlayerMovement))]
public class VelcroGloves : MonoBehaviour
{
	//Are we climbing?
	bool m_Enabled = false;

	//The player, so we can set flags
	//Player m_Player = null

	//Movement, so we can call climbing movement
	PlayerMovement m_Movement;

	//Angle of the new wall to climb
	float m_AngleOfNextWall;

	//Walls nearby
	List <GameObject> m_VelcroWalls = new List<GameObject>();

	//Initialization		
	void Start ()
	{
		//Get the components
		m_Movement = (PlayerMovement)gameObject.GetComponent<PlayerMovement>();    //Get component to move the player
		//m_Player = getComponent <Player>();        							   //Get player component to set player flags
	}

	//Every tick
	void Update ()
	{
		//If we are not near a window, do nothing
		if ( m_VelcroWalls.Count <= 0 )    //Not near a window
		{
			return;
		}

		//You are climbing
		else if ( m_Enabled )
		{
			//Fall button
			if (Input.GetButtonDown("Fire1"))
			{
				//We are no longer climbing
				m_Enabled = false; 

				//Set player flag
				//m_Player->setExitSecondItemFlag ();

				return;
			}

			//Move
			m_Movement.ClimbMovement();   //Otherwise we climb
		}

		//Grab button
		else if (Input.GetButtonDown("Fire1"))    //You grabbed the window
		{
			//Enable Climbing
			m_Enabled = true;

			//Set rotation of the player to face the wall
			transform.Rotate (0, m_AngleOfNextWall - this.transform.rotation.eulerAngles.y, 0);

			//Set flag
			//m_Player->setEnterSecondItemFlag();       //Set player flag
		}
	}

	//When something is nearby
	void OnTriggerEnter ( Collider collider )
	{
		//If it was a Velcro wall
		if ( collider.gameObject.CompareTag("VelcroWall") )
		{
			//Save angle of wall
			m_AngleOfNextWall = collider.transform.eulerAngles.y;

			//You are near this wall
			m_VelcroWalls.Add ( collider.gameObject );
		}
	}

	//When something is no longer nearby
	void OnTriggerExit ( Collider collider )
	{
		//If it was a Velcro wall
		if ( collider.gameObject.CompareTag("VelcroWall"))
		{
			//If we are climbing
			if (m_Enabled)
			{
				//If this was the only nearby window, you fall
				if ( m_VelcroWalls.Count <= 1 )
				{
					//You fall
					m_Enabled = false;

					//Set player flag
					//m_Player->setExitSecondItemFlag ();
				}

				//If we are leaving the first wall
				else if (collider.gameObject == m_VelcroWalls[0])	//if 
				{
					//Set rotation of the player to face the wall
					transform.Rotate (0, m_AngleOfNextWall - this.transform.rotation.eulerAngles.y, 0);
				}
			}

			//You are no longer near this wall
			m_VelcroWalls.Remove ( collider.gameObject );
		}
	}
}
