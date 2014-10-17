﻿/*
 * Created by: Greg Fortier
 * The BaseGoal functions as a level goal(the end of the level). Currently when the player
 * reaches the end goal, he must wait for the second player before the level will finish.
 * 
 * So the level will only switch if both players are in the trigger
 * 
 */

#region ChangeLog
/*
* 8/10/2014 Edit: Greg Fortier
* Fully commented, removed the character controller components since the crawlspace will no longer automatically move the players when changing the level
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class BaseGoal : MonoBehaviour {

	//public Transform m_LevelEnd;
	public string m_NextScene;

	protected bool[] m_AtEnd = new bool[2];
	//protected float m_Speed;

	//CharacterController[] m_Players = new CharacterController[2];
	Vector3[] m_PlayerPositionHolder = new Vector3[2];

	int m_PlayerWaitingToExit = 0;
	int m_MaxPlayersPossible = 2;

	float m_DistanceToEnd = 1.0f;

	//Initialize values
	void Start()
	{
		m_PlayerWaitingToExit = 0;

		for(int i = 0; i < m_AtEnd.Length; i++)
		{
			m_AtEnd[i] = false;
		}
	}

	void Update()
	{
		//if element 1 of array m_AtEnd equals true than load the next level. Because element 1 would be player2
		if (m_AtEnd[1])
			{
				LoadNext();
				return;
			}
		}

	//if the player comes in contact with the level goal trigger. It will check if the first element of the m_AtEnd bool = false if yes then increment
	//the integer m_PlayerWaitingToExit and make m_AtEnd[0] true

	//if m_AtEnd[0] does not equal false then that means that m_AtEnd[1] must be true if someone walks in the trigger.
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == Constants.PLAYER_STRING)
		{
			if (m_AtEnd[0] != true)
			{
				AddWaitingPlayer();
				m_AtEnd[0] = true;

			}

			else 
			{
				AddWaitingPlayer();
				m_AtEnd[1] = true;
			}
		}
	}

	//When the player exits the trigger is makes sure the values are decremented so that the second player can not activate level goal alone
	void OnTriggerExit(Collider other)
	{
		m_PlayerWaitingToExit--;

		m_AtEnd[0] = false;

	}

	//Loads the next level
	public void LoadNext()
	{
		//Tell Game Data to load next level
		Application.LoadLevel (m_NextScene);
	}

	//Increment waiting player count so that it knows how many people are waiting to change level
	public void AddWaitingPlayer()
	{
		m_PlayerWaitingToExit++;
	}


}