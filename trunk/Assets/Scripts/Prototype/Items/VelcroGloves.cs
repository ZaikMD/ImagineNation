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
	
	//Angle of the new wall to climb
	float m_AngleOfNextWall;
	
	//Walls nearby
	List <GameObject> m_VelcroWalls = new List<GameObject>();
	
	//Every tick
	void Update ()
	{
		//You are climbing
		if ( m_Enabled && m_Climbing )
		{
			//Fall button
			if (PlayerInput.Instance.getEnviromentInteraction() || PlayerInput.Instance.getJumpInput())
			{
				onExit();
				return;
			}
			
			//Move
			Move ();
		}
	}

	//Starts climbing
	public void onUse()
	{
		//Enable Climbing
		m_Climbing = true;
		
		//Player can not move normally while climbing
		m_PlayerMovement.setCanMove(false);
		
		//Set rotation of the player to face the wall
		transform.Rotate (0, m_AngleOfNextWall - this.transform.rotation.eulerAngles.y - 180, 0);
	}

	//Stops climbing
	public void onExit()
	{
		//You fall
		m_Climbing = false;
		
		//Player can now move normally again
		m_PlayerMovement.setCanMove (true);

		gameObject.GetComponent<PlayerState> ().setExitingSecond (true);
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

			m_Enabled = true;
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
					onExit();
					m_Enabled = false;
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
		return m_Enabled;
	}
}