/*
TO USE:

Attach this script to the player

Set Velcro Wall prefab into world.

Make sure that walls rotation is facing towards the player.






Created by Jason Hein on 3/23/2014


3/29/2014
	Now properly inherits from base secondary item
	Now properly enables normal player movement
*/





using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(PlayerMovement))]
public class VelcroGloves : SecondairyBase
{
	//Are we climbing?
	bool m_Climbing = false;
	
	//The player, so we can set flags
	//Player m_Player = null
	
	//Angle of the new wall to climb
	float m_AngleOfNextWall;
	
	//Walls nearby
	List <GameObject> m_VelcroWalls = new List<GameObject>();
	
	//Initialization		
	void Start ()
	{
		//Get the component
		//m_Player = getComponent <Player>();        							   //Get player component to set player flags
	}
	
	//Every tick
	void Update ()
	{
		//If we are not near a window, do nothing
		if ( m_VelcroWalls.Count <= 0 || !m_Enabled)    //Not near a window
		{
			return;
		}
		
		//You are climbing
		else if ( m_Climbing )
		{
			//Fall button
			if (PlayerInput.Instance.getEnviromentInteraction() || PlayerInput.Instance.getJumpInput())
			{
				//We are no longer climbing
				m_Climbing = false; 

				//Player can now move normally again
				m_PlayerMovement.setCanMove(true);
				
				//Set player flag
				//m_Player->setExitSecondItemFlag ();
				
				return;
			}
			
			//Move
			Move ();
		}
		
		//You grabbed the window
		else if (PlayerInput.Instance.getEnviromentInteraction()|| PlayerInput.Instance.getJumpInput())
		{
			//Enable Climbing
			m_Climbing = true;

			//Player can not move normally while climbing
			m_PlayerMovement.setCanMove(false);

			//Set rotation of the player to face the wall
			transform.Rotate (0, m_AngleOfNextWall - this.transform.rotation.eulerAngles.y - 180, 0);
			
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
			if (m_Climbing)
			{
				//If this was the only nearby window, you fall
				if ( m_VelcroWalls.Count <= 1 )
				{
					//You fall
					m_Climbing = false;

					//Player can now move normally again
					m_PlayerMovement.setCanMove(true);
					
					//Set player flag
					//m_Player->setExitSecondItemFlag ();
				}
				
				//If we are leaving the first wall
				else if (collider.gameObject == m_VelcroWalls[0])	//if 
				{
					//Set rotation of the player to face the wall
					transform.Rotate (0, m_AngleOfNextWall - this.transform.rotation.eulerAngles.y - 180, 0);
				}
			}
			
			//You are no longer near this wall
			m_VelcroWalls.Remove ( collider.gameObject );
		}
	}
	
	public override void Move()
	{
		m_PlayerMovement.ClimbMovement();   //Otherwise we climb
	}

	public override bool ableToBeUsed()
	{
		return true;
	}
}