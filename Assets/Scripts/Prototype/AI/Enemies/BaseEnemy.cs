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

	public GameObject[] m_PatrolNodes;

	public const float EXIT_COMBAT_TIME = 3.0f;
	float m_Timer = EXIT_COMBAT_TIME;

	// Use this for initialization
	void Start () 
	{
		m_Health = gameObject.GetComponent<Health>();

		GameManager.Instance.addObserver(this);

		m_Players = GameObject.FindGameObjectsWithTag("Player");

		if(m_AggroRange <= m_CombatRange)
		{
			m_CombatRange = m_AggroRange;
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.PauseGame || recievedEvent == ObeserverEvents.StartGame)
		{
			m_IsEnabled = !m_IsEnabled;
		}
	}

	public void applyDamage(int amount)
	{
		m_Health.takeDamage(amount);
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

	void defaultState()
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
			}

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
		}

		//not in aggro range so patrol
		m_State = States.Patrol;

	}

	protected abstract void die();

	protected abstract void fightState();

	protected virtual void followState()
	{
		//TODO: Follow Code
	}

	protected virtual void patrolState()
	{
		//TODO: patrol code
	}
	

}