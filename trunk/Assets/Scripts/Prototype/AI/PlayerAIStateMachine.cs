using UnityEngine;
using System.Collections;
using System.Collections.Generic;


////////////////////// ATTACK AND PATHFINDING LEFT TO DO!!!///
public class PlayerAIStateMachine : MonoBehaviour 
{
	//ENUMS

	//Enum for each of the AI states 
	enum PlayerAIState
	{
		Default = 0,
		Combat,
		InPuzzle,
		Following,
		Count,
		Unknown
	}

	//Enum for the CombatStates that will affect the AI when in the Combat state
	enum PlayerAICombatState
	{ 
		Default = 0, 
		Unable, 
		InRange, 
		OutOfRange, 
		Hiding,
		Count, 
		Unknown 
	}

	public enum InteractionTypes
	{
		PickUp = 0,
		SeesawBottom,
		SeesawJump,
		NPC,
		CrawlSpace,
		Count,
		unKnown
	}

	//MEMBER VARIABLES

	//An m_State and m_CombatState to change between the different AI states 

	PlayerAIState m_State; 
	PlayerAICombatState m_CombatState;
		
	//An m_Player GameObject for pathfinding purposes 
	GameObject m_Player; 
	
	//A list of GameObject enemies to determine which enemies the AI is interacting with in combat 
	List<GameObject> enemies;
	PlayerState m_playerStateMachine;
	
	//boolean for entering combat set by the addEnemy
	bool m_EnterCombatFlag;
	bool m_EnterPuzzle;

	public const int m_IdealRange = 10;
	public const int m_InRange = 15;

	//METHODS

	// Use this for initialization
	void Start () 
	{
		m_playerStateMachine = this.gameObject.GetComponent<PlayerState>();
	}

	/// <summary>
	/// Adds the combat enemy.
	/// This function would allow us to add an Enemy to our list by passing in the desired enemy to add, //giving our AI a target to engage.
	/// <param name="enemy">Enemy.</param>
	public void AddCombatEnemy(GameObject enemy) 		 
	{ 
		enemies.Add(enemy); 
		m_EnterCombatFlag = true;
	}

	/// <summary>
	/// Removes the enemy.
	/// Just as the addCombatEnemy adds an Enemy to our list, this will remove one suggesting that the //Enemy that was previously active, is being set to inactive
	/// <param name="enemy">Enemy.</param>
	public void RemoveEnemy(GameObject enemy)
	{ 
		enemies.Remove(enemy); 
		if(enemies.Count<=0)
		{
			m_EnterCombatFlag = false;
		}
	} 

	/// <summary>
	/// Gets the interacting.
	/// A function used to see if the AI is interacting or not, accessed from the Player
	/// <returns><c>true</c>, if interacting was gotten, <c>false</c> otherwise.</returns>
	bool GetInteracting()	 
	{ 
		return m_playerStateMachine.getInteracting();
	} 

	/// <summary>
	/// Gets the type of the interaction.
	/// // returns a reference to the game object being interacted with
	/// <returns>The interaction type.</returns>
 	 InteractionTypes GetInteractionType() 
	{ 
		return (InteractionTypes) m_playerStateMachine.interactionType();

	} 

	/// <summary>
	/// Paths the find to target.
	///Pathfinding is still a work in progress, but the function will return a bool to have the AI pathfind to 
	//a passed in target, this will be used for moving the AI and setting it InRange
	/// <returns><c>true</c>, if find to target was pathed, <c>false</c> otherwise.</returns>
	/// <param name="target">Target.</param>
	bool PathFindToTarget(GameObject target) 
	{ 
		//Move to desired location based on target passed in, using Pathfinding 
		//return true if the AI has found the target to pathfind to 

		//TODO pathfinding
		return false;
	} 
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if(m_State == PlayerAIState.Default)
		{
			Default();  
		}

		switch(m_State)
		{
		case PlayerAIState.InPuzzle:
			InPuzzle();
			break;
		case PlayerAIState.Following:
			Following();
			break;
		case PlayerAIState.Combat:
			Combat();
			break;
		//default:
			//assert

		}
	}

	/// <summary>
	/// Update loop for Default State
	/// </summary>
	void Default()
	{
		if(m_EnterCombatFlag)
		{
			m_State = PlayerAIState.Combat;
			m_CombatState = PlayerAICombatState.Default;
		}
		else if(m_EnterPuzzle)
		{
			m_State = PlayerAIState.InPuzzle;
		}
		else
		{
			m_State = PlayerAIState.Following;
		}
	}

	/// <summary>
	/// Update loop for Following State
	/// </summary>
	void Following()                                        
	{
			PathFindToTarget(m_Player);
			m_State = PlayerAIState.Default;
	}

	/// <summary>
	/// Update loop for InPuzzle State
	/// </summary>
	void InPuzzle()
	{
		if (GetInteracting ())
		{
			switch(GetInteractionType())
			{
			case InteractionTypes.PickUp:
				//exit Movable Block
				m_State = PlayerAIState.Default;
				break;

			case InteractionTypes.SeesawBottom:
				//exit Movable Block
				m_State = PlayerAIState.Default;
				break;

			case InteractionTypes.SeesawJump:
				//exit Movable Block
				m_State = PlayerAIState.Default;
				break;

			case InteractionTypes.NPC:
				//exit Movable Block
				m_State = PlayerAIState.Default;
				break;

			case InteractionTypes.CrawlSpace:
				//exit Movable Block
				m_State = PlayerAIState.Default;
				break;

			}
		}		

		else
		{
			m_State = PlayerAIState.Default;
		}
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		switch(other.name)
		{
		case "puzzleArea":
			m_EnterPuzzle = true;
			break;

		case "ennemy":
			AddCombatEnemy(other.gameObject);
			break;
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit(Collider other)
	{
		switch(other.name)
		{
		case "puzzleArea":
			m_EnterPuzzle = false;
			break;

		case "ennemy":
			RemoveEnemy(other.gameObject);
			break;
		}
	}

	/// <summary>
	/// Update loop for Combat state
	/// </summary>
	void Combat()
	{
		if(m_CombatState == PlayerAICombatState.Default)
		{
			CombatDefault();
		}
		
		switch(m_CombatState)
		{
		case PlayerAICombatState.Unable:
			CombatUnable();
			break;
		case PlayerAICombatState.InRange:
			CombatInRange();
			break;
		case PlayerAICombatState.OutOfRange:
			CombatOutOfRange();
			break;
		case PlayerAICombatState.Hiding:
			CombatHiding();
			break;
		//default:
			//assert();
		}
	}

	/// <summary>
	/// Update loop for Combat default in combat states
	/// </summary>
	void CombatDefault()
	{
		if(GetInteracting())
		{
			m_CombatState = PlayerAICombatState.Unable;
		}
		else if(inRange())
		{
			m_CombatState = PlayerAICombatState.InRange;
		}
		else 
		{
			m_CombatState = PlayerAICombatState.OutOfRange;
		}
//		if(ableToHide)
//		{
//			m_CombatState = PlayerAICombatState.Hiding;
//		}
	}

	/// <summary>
	///  Update loop for CombatUnable in combat states
	/// </summary>
	void CombatUnable()
	{
		if (GetInteracting())
		{
			//TODO Fill in what happens with each interactable
			switch(GetInteractionType())
			{
//				Check if the interactable is exitable if it is exit -> default
//				if it isn't exitable return to default
				case InteractionTypes.PickUp:
				
					m_State = PlayerAIState.Default;
					break;
				
				case InteractionTypes.SeesawBottom:
				
					m_State = PlayerAIState.Default;
					break;
				
				case InteractionTypes.SeesawJump:
				
					m_State = PlayerAIState.Default;
					break;
				
				case InteractionTypes.NPC:
				
					m_State = PlayerAIState.Default;
					break;
				
				case InteractionTypes.CrawlSpace:
				
					m_State = PlayerAIState.Default;
					break;

			} 
		}
	}

	/// <summary>
	/// Update loop for CombatInRange in combat states
	/// </summary>
	void CombatInRange()
	{

		if(ButterZone(FindClosestEnemy()))
		{
			Attack ();
			m_CombatState = PlayerAICombatState.Default;
		}
		else
		{
			Attack ();
//			PathFindToTarget(m_IdealRange);
			m_CombatState = PlayerAICombatState.Default;
		}
	}

	/// <summary>
	/// Checks to see if plaayer is in the ideal range
	/// </summary>
	bool ButterZone(GameObject enemy) 
		//This function checks if the playerAI is in the preferred position to attack the enemy target 
	{ 
		//Check if player is in Ideal range
		if (Vector3.Distance(this.transform.position, enemy.transform.position) == m_IdealRange)
		{
			return true;
		}

		else 
		{
			return false;
		}
	}

	/// <summary>
	///Applying damage to the desired enemy then setting the AI back to its default state 
	/// </summary>
	void Attack() 
	{ 
		// TODO Attack
	} 

	/// <summary>
	/// Update loop for CombatOutOfRange in combat states
	/// </summary>
	void CombatOutOfRange()
	{
		//loop through enemies to find the closest
		for(int i = 0; i < enemies.Count; i++)
		{
			//TODO pathfinding
			//pathfind to closest enemy
		}
		m_CombatState = PlayerAICombatState.Default;
	}

	/// <summary>
	/// Update loop for CombatHiding in combat states
	/// </summary>
	void CombatHiding()
	{
		//find hiding spot and pathfind to it
		m_CombatState = PlayerAICombatState.Default;
	}

//	/// <summary>
//	/// Finds the hiding spot.
//	/// </summary>
//	/// <returns><c>true</c>, if hiding spot was found, <c>false</c> otherwise.</returns>
//	/// <param name="hidingSpot">Hiding spot.</param>
//	bool FindHidingSpot(GameObject* hidingSpot) 
//		//This function will create an empty gameObject for the AI to hide in if they can't get InRange of 
//		//their target 
//	{ 
//		//Create hiding spot and pathfind to it and hide from enemy, if the AI cannot hide then return to //default 
//	} 

	bool inRange()
	{
	 foreach (GameObject enemy in enemies)
		{

			if (Vector3.Distance(this.transform.position, enemy.transform.position) < m_InRange)
			{
						return true;					

			}
		}
		return false;
	}

	GameObject FindClosestEnemy()
	{
		float closestEnemyDistance = 0;
		GameObject closestEnemy = enemies[0];
		float currentEnemyDistance = 0;
		foreach (GameObject enemy in enemies)
		{		
			currentEnemyDistance = Vector3.Distance(this.transform.position, enemy.transform.position);
			if (closestEnemyDistance > currentEnemyDistance)
			{
				closestEnemy = enemy;
				closestEnemyDistance = currentEnemyDistance;
			}

		}

		return closestEnemy;
	}

}
