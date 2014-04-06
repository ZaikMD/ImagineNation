using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PlayerAIStateMachine : MonoBehaviour, Observer 
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
		Count, 
		Unknown 
	}

	//MEMBER VARIABLES

	//An m_State and m_CombatState to change between the different AI states 

	PlayerAIState m_State; 
	PlayerAICombatState m_CombatState;
		
	//An m_Player GameObject for pathfinding purposes 
	GameObject m_Partner; // TODO set to current player
	PlayerPathfinding m_PathFinding;
	
	//A list of GameObject enemies to determine which enemies the AI is interacting with in combat 
	List<GameObject> m_enemies = new List<GameObject>();
	PlayerState m_playerStateMachine;
	GameObject m_CombatTarget;
	
	//boolean for entering combat set by the addEnemy
	bool m_EnterCombatFlag;
	bool m_EnterPuzzle;

	public const int m_IdealAttackRange = 10;
	public const int m_MaxAttackRange = 15;

	bool m_IsPaused = false;
	public bool m_IsActive;

	NavMeshAgent m_NavAgent;

	//METHODS

	// Use this for initialization
	void Start () 
	{
		m_playerStateMachine = this.gameObject.GetComponent<PlayerState>();
		m_PathFinding = this.gameObject.GetComponent<PlayerPathfinding>();

		GameManager.Instance.addObserver (this);
		CharacterSwitch.Instance.addObserver (this);


		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		if(players[0] != this.gameObject )
		{
			m_Partner = players[0];
		}
		else
		{
			m_Partner = players[1];
		}

		//------------------------------
		m_NavAgent = gameObject.GetComponent<NavMeshAgent> ();

		m_NavAgent.enabled = m_IsActive;
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if(m_IsActive)
		{
			if(!m_IsPaused)
			{
				if(m_State == PlayerAIState.Default)
				{
					Default();  
				}

				switch(m_State)
				{
					case PlayerAIState.InPuzzle:
					{
						InPuzzle();
					}
						break;

					case PlayerAIState.Following:
					{
						Following();
					}
						break;

					case PlayerAIState.Combat:
					{
						Combat();
					}
						break;
				}
			}
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
			return;
		}

		if(m_EnterPuzzle)
		{
			m_State = PlayerAIState.InPuzzle;
			return;
		}

		m_State = PlayerAIState.Following;

	}

	/// <summary>
	/// Update loop for Following State
	/// </summary>
	void Following()                                        
	{
		m_PathFinding.Following (m_Partner.transform);

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
				case InteractableType.MovingBlock:
				{
					//TODO: stop interacting
				}
					break;

				case InteractableType.PickUp:
				{
					//TODO: drop
				}
					break;

				case InteractableType.SeeSaw:
				{
				//stay
					
				}
					break;

			}
		}		
		m_State = PlayerAIState.Default;
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
			return;
		}
		
		if(inRange())
		{
			m_CombatState = PlayerAICombatState.InRange;
			return;
		}
		
		else 
		{
			m_CombatState = PlayerAICombatState.OutOfRange;
		}
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
				
			case InteractableType.Lever:
			{
				m_State = PlayerAIState.Default;
			}
				break;
				
			case InteractableType.MovingBlock:
			{
				m_State = PlayerAIState.Default;
			}
				break;		
				
			case InteractableType.PickUp:
			{
				m_State = PlayerAIState.Default;
			}
				break;
				
			case InteractableType.SeeSaw:
			{
				m_State = PlayerAIState.Default;
			}
				break;
				
				
			} 
		}
	}
	
	/// <summary>
	/// Update loop for CombatInRange in combat states
	/// </summary>
	void CombatInRange()	
	{
		m_CombatTarget = FindClosestEnemy();
		
		if(ButterZone(m_CombatTarget))
		{
			Attack (true);
			m_PathFinding.Combat(m_CombatTarget.transform,m_IdealAttackRange);
			m_CombatState = PlayerAICombatState.Default;
			m_State = PlayerAIState.Default;
		}
		
		else
		{
			Attack (false);
			m_PathFinding.Combat(m_CombatTarget.transform,m_IdealAttackRange);
			m_CombatState = PlayerAICombatState.Default;
			m_State = PlayerAIState.Default;
		}
	}

	
	/// <summary>
	/// Update loop for CombatOutOfRange in combat states
	/// </summary>
	void CombatOutOfRange()
	{
		m_CombatTarget = FindClosestEnemy ();
		m_PathFinding.Combat(m_CombatTarget.transform,m_MaxAttackRange);
		
		m_CombatState = PlayerAICombatState.Default;
		m_State = PlayerAIState.Default;
	}
	
	/// <summary>
	/// Checks to see if plaayer is in the ideal range
	/// </summary>
	bool ButterZone(GameObject enemy) 
		//This function checks if the playerAI is in the preferred position to attack the enemy target 
	{ 
		//Check if player is in Ideal range
		if (Vector3.Distance(this.transform.position, enemy.transform.position) == m_IdealAttackRange)
		{
			return true;
		}
		
		return false;		
	}
	
	/// <summary>
	///Applying damage to the desired enemy then setting the AI back to its default state 
	/// </summary>
	void Attack(bool inButterZone) 
	{ 
		// TODO Attack
		if (inButterZone)
		{
			//More Damage
			
		}
		
		else
		{
			
		}
		m_CombatState = PlayerAICombatState.Default;
	} 

	/// <summary>
	/// Checks to see if any enemies are in range  		
	/// </summary>
	/// <returns><c>true</c>, if range was ined, <c>false</c> otherwise.</returns>
	bool inRange()
	{
		foreach (GameObject enemy in m_enemies)
		{
			if (Vector3.Distance(this.transform.position, enemy.transform.position) < m_MaxAttackRange)
			{
				return true;
			}
		}
		
		return false;
	}
	
	/// <summary>
	/// Finds the closest enemy to you and returns it
	/// </summary>
	/// <returns>The closest enemy.</returns>
	GameObject FindClosestEnemy()
	{
		float closestEnemyDistance = 0;
		GameObject closestEnemy = m_enemies[0];
		float currentEnemyDistance = 0;
		foreach (GameObject enemy in m_enemies)
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


	/// <summary>
	/// Gets the interacting.
	/// A function used to see if the AI is interacting or not, accessed from the Player
	/// <returns><c>true</c>, if interacting was gotten, <c>false</c> otherwise.</returns>
	bool GetInteracting()	 
	{ 
		return m_playerStateMachine.getInteracting();
	} 

	/// <summary>
	/// Gets the interacting.
	/// A function used to see if the AI is interacting or not, accessed from the Player
	/// <returns><c>true</c>, if interacting was gotten, <c>false</c> otherwise.</returns>
	InteractableType GetInteractionType()	 
	{ 
		return m_playerStateMachine.interactionType();
	} 

	public void AddCombatEnemy(GameObject enemy) 		 
	{ 
		m_enemies.Add(enemy); 
		m_EnterCombatFlag = true;
	}
	

	public void RemoveEnemy(GameObject enemy)
	{ 
		//TODO if enemy dies is he removed from the list?
		m_enemies.Remove(enemy); 
		if(m_enemies.Count<=0)
		{
			m_EnterCombatFlag = false;
		}
	} 


	void OnTriggerEnter(Collider other)
	{
		switch(other.tag)
		{
		case "PuzzleArea":
			m_EnterPuzzle = true;
			m_PathFinding.Puzzle();
			break;
			
		case "Enemy":
			AddCombatEnemy(other.gameObject);
			break;
		}
	}

	void OnTriggerExit(Collider other)
	{
		switch(other.tag)
		{
		case "PuzzleArea":
			m_EnterPuzzle = false;
			m_PathFinding.Puzzle();
			break;
			
		case "Enemy":
			RemoveEnemy(other.gameObject);
			break;
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsPaused = !m_IsPaused;
			return;
		}

		if(recievedEvent == ObeserverEvents.CharacterSwitch)
		{
			m_IsActive = !m_IsActive;

			m_NavAgent.enabled = m_IsActive;

			return;
		}
	}
}
