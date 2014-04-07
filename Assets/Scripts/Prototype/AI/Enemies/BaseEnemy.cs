using UnityEngine;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour, Observer
{
	protected enum States
	{
		Default,
		Fight,
		Follow,
		Patrol
	}

	States m_State = States.Default;

	bool m_IsInCombat = false;

	bool m_IsEnabled = true;

	public float m_AggroRange = 20.0f;
	protected float m_CombatRange;

	protected Health m_Health;

	public GameObject m_Ragdoll;

	GameObject[] m_Players;
	Transform m_Target;

	public const float EXIT_COMBAT_TIME = 3.0f;
	float m_Timer = EXIT_COMBAT_TIME;

	EnemyPathfinding m_EnemyPathfinding;

	// Use this for initialization
	void Start () 
	{
		m_Health = gameObject.GetComponent<Health>();

		m_EnemyPathfinding = gameObject.GetComponent<EnemyPathfinding>();

		GameManager.Instance.addObserver(this);

		m_Players = GameObject.FindGameObjectsWithTag("Player");

		if(m_AggroRange <= m_CombatRange)
		{
			m_CombatRange = m_AggroRange;
		}
	}

	/// <summary>
	/// Checks if the game is paused and sets the m_IsEnabled
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="recievedEvent">Recieved event.</param>
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsEnabled = !m_IsEnabled;
		}
	}

	/// <summary>
	/// Applies damage to the enemy by a passed in
	/// amount.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void applyDamage(int amount)
	{
		m_Health.takeDamage(amount);
	}

	/// <summary>
	/// Reset variables back to a default state for spawning
	/// </summary>
	public void reset()
	{
		m_IsInCombat = false;
		m_IsEnabled = true;
		m_Target = null;
		m_Timer = EXIT_COMBAT_TIME;
		m_State = States.Default;
	}

	// Update is called once per frame
	void Update () 
	{
		if(m_IsEnabled)
		{


			if(m_State == States.Default)
			{
				defaultState();
			}

			switch(m_State)
			{
				case States.Fight:
				{
					updateCombat();
					fightState();
					break;
				}

				case States.Patrol:
				{
					patrolState();
					break;
				}

				case States.Follow:
				{
					followState();
					break;
				}
			}
		}
	}

	/// <summary>
	/// Used to do any specific combat update, able to be
	/// overridden for inheriting classes
	/// </summary>
	protected virtual void updateCombat ()
	{
	}

	/// <summary>
	/// Makes sure enemy has health, then checks with various
	/// ranges and timers to go into the next state
	/// </summary>
	protected virtual void defaultState()
	{
		//first check if enemy dead
		if(m_Health.getHealth() <= 0)
		{
			die();
			return;
		}

		//enemy is not dead so check if in combat
		if(m_IsInCombat)
		{
			/*
			for(int i =0; i < m_Players.Length; i++)
			{
				float distance = Vector3.Distance(gameObject.transform.position, m_Players[i].transform.position);
				if(distance <= m_AggroRange)
				{
					m_Timer = EXIT_COMBAT_TIME;

					//in range to aggro so check if in combat range
					if(distance <= m_CombatRange)
					{
						m_State = States.Fight;
					}
					else
					{
						m_State = States.Follow;
					}
					return;
				}
			}*/

			float distance = Vector3.Distance(gameObject.transform.position, m_Target.transform.position );

			if(distance <= m_AggroRange)
			{
				//is target in combat range
				if(distance <= m_CombatRange)
				{
					// if yes go to fight state and Reset exit combat timer and return
					m_State = States.Fight;
					m_Timer = EXIT_COMBAT_TIME;
					return;
				}

				//if yes go to follow state and Reset exit combat timer and return
				m_State = States.Follow;
				m_Timer = EXIT_COMBAT_TIME;
				return;
			}

			for(int i =0; i < m_Players.Length; i++)
			{
				if(m_Players[i] != m_Target)
				{
					m_EnemyPathfinding.setTarget(m_Players[i].gameObject);
					m_Target = m_EnemyPathfinding.getTarget();
				}
			}

			distance = Vector3.Distance(gameObject.transform.position, m_Target.transform.position );
			//is the target in aggro range
			if(distance <= m_AggroRange)
			{
				if(distance <= m_CombatRange)
				{
					// if yes go to fight state and Reset exit combat timer and return
					m_State = States.Fight;
					m_Timer = EXIT_COMBAT_TIME;
					return;
				}
				//if yes go to follow state and Reset exit combat timer and return
				m_State = States.Follow;
				m_Timer = EXIT_COMBAT_TIME;
				return;
			}

			for(int i =0; i < m_Players.Length; i++)
			{
				if(m_Players[i] != m_Target)
				{
					m_EnemyPathfinding.setTarget(m_Players[i].gameObject);
					m_Target = m_EnemyPathfinding.getTarget();
				}
			}

			distance = Vector3.Distance(gameObject.transform.position, m_Target.transform.position );
			if(distance <= m_AggroRange)
			{
				//if yes go to follow state and Reset exit combat timer and return
				m_State = States.Follow;
				m_Timer = EXIT_COMBAT_TIME;
				return;
			}
			//is the other player in aggro range
			//if yes change target and go to follow state and Reset exit combat timer and return

			if(m_Timer <= 0)
			{
				m_Timer = EXIT_COMBAT_TIME;
				m_IsInCombat = false;
				m_State = States.Patrol;
			}
			else
			{
				m_Timer -= Time.deltaTime;
				m_State = States.Follow;
			}
			//check timer
			return;
		}

		//not in combat so check if a player is in aggro range
		for(int i =0; i < m_Players.Length; i++)
		{
			float distance = Vector3.Distance(gameObject.transform.position, m_Players[i].transform.position);
			if(distance <= m_AggroRange)
			{
				m_IsInCombat = true;
				m_EnemyPathfinding.setTarget(m_Players[i].gameObject);
				m_Target = m_EnemyPathfinding.getTarget();
				return;
			}
		}

		//not in aggro range so patrol
		m_State = States.Patrol;

	}

	/// <summary>
	/// abstract die state which is different for each
	/// enemy that inherits
	/// </summary>
	protected abstract void die();

	/// <summary>
	/// abstract fightState that can be overridden because
	/// each enemy will fight differently
	/// </summary>
	protected abstract void fightState();

	/// <summary>
	/// Calls the pursue state of EnemyPathFinding
	/// </summary>
	protected virtual void followState()
	{
		m_EnemyPathfinding.SetState (EnemyPathfindingStates.Pursue);
	}

	/// <summary>
	/// sets the EnemyPathFinding state to Patrol
	/// </summary>
	protected virtual void patrolState()
	{
		//TODO: patrol code
		m_EnemyPathfinding.SetState (EnemyPathfindingStates.Patrol);
	}
	

}