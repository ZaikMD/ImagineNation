/*
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
* 17/10/2014 Edit: Zach Dubuc
* Added in checkpoints being reset when a new scene loads
* 
* 9/3/2015 Edit: Kole Tackney
* Added auto continue, and 
*/
#endregion

using UnityEngine;
using System.Collections;

public class BaseGoal : MonoBehaviour 
{
	public Levels m_NextLevel;
	public Sections m_NextSection;

	public float m_AutoContinueTime;

	public bool m_AutoContinueOnTime;
	public bool m_PlayerInGoalPrompt;
	public bool m_PlayerOutOfGoalPrompt;

	protected bool[] m_AtEnd = new bool[2];
	//protected float m_Speed;

	int m_PlayerWaitingToExit = 0;
	
    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	float m_CurrentContinueTimer;

	protected PlayerInput m_PlayerInGoal;
	protected PlayerInput m_PlayerOutOfGoal;

	//Initialize values
	void Start()
	{
		m_PlayerWaitingToExit = 0;

		m_CurrentContinueTimer = m_AutoContinueTime;

		for(int i = 0; i < m_AtEnd.Length; i++)
		{
			m_AtEnd[i] = false;
		}
	}

	void Update()
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		//if element 1 of array m_AtEnd equals true than load the next level. Because element 1 would be player2
		if (m_AtEnd[1])
		{
			LoadNext();
			return;
		}

		if(m_PlayerWaitingToExit > 0) //If player is in the goal.
		{
			m_CurrentContinueTimer -= Time.deltaTime;

			if(m_CurrentContinueTimer < 0)
			{
				if(m_AutoContinueOnTime)
				{	
					LoadNext();
					return;
				}

				if(m_PlayerInGoalPrompt)
				{
					if(InputManager.getPause(m_PlayerInGoal))
					{
						LoadNext();
						return;
					}
				}

				if(m_PlayerOutOfGoalPrompt)
				{
					if(InputManager.getPause(m_PlayerOutOfGoal))
					{
						LoadNext();
						return;
					}
				}
			}
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

				m_PlayerInGoal = other.GetComponent<AcceptInputFrom>().ReadInputFrom;

				if( m_PlayerInGoal == GameData.Instance.m_PlayerOneInput)
				{
					m_PlayerOutOfGoal = GameData.Instance.m_PlayerTwoInput;
				}
				else
				{
					m_PlayerOutOfGoal = GameData.Instance.m_PlayerOneInput;
				}
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

		m_CurrentContinueTimer = m_AutoContinueTime; //Reset timer.

	}

	//Loads the next level
	public void LoadNext()
	{
		GameData.Instance.CurrentLevel = m_NextLevel;
		GameData.Instance.CurrentSection = m_NextSection;
		//Tell Game Data to load next level
		GameData.Instance.resetCheckPoint (); //Reset the checkpoint
		GameData.Instance.FirstTimePlayingLevel = true;// reset first time playing

		Application.LoadLevel (Constants.LOADING_SCREEN); // load the next level
	}

	//Increment waiting player count so that it knows how many people are waiting to change level
	public void AddWaitingPlayer()
	{
		m_PlayerWaitingToExit++;
	}

	public void OnGUI()
	{
		Rect posRect = new Rect(Screen.width / 8, Screen.height / 5 * 3, Screen.width / 8 * 6, Screen.height / 2.5f );

		if(m_PlayerWaitingToExit > 0)
		{
			if(m_CurrentContinueTimer > 0)
			{
				GUI.Label(posRect, "Waiting for other Player: " + m_CurrentContinueTimer.ToString("#.00"));
			}
			else
			{			
				if(m_PlayerInGoalPrompt && m_PlayerOutOfGoalPrompt)
				{
					GUI.Label(posRect, "Press Start to continue to next level");
					return;
				}

				if(m_PlayerInGoalPrompt)
				{
					GUI.Label(posRect, "Press Start to continue to next level");
					return;
				}

				if(m_PlayerOutOfGoalPrompt)
				{
					GUI.Label(posRect, "Press Start to continue to next level");
					return;
				}
			}
		}
	}
}
