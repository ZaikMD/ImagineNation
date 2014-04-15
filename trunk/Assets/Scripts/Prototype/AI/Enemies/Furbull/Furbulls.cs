using UnityEngine;
using System.Collections;

public class Furbulls : BaseEnemy 
{
	NavMeshAgent m_Agent;

	GameObject m_ChargeTarget = null;

	bool m_HasTarget = false;
	bool m_HasReachedTarget = false;

	float m_ChargeSpeed = 18.0f;
	float m_ChargeDistance = 16.0f;

	float m_FightTimer = 0.0f;
	const float CHARGE_DELAY = 1.0f;

	// Use this for initialization
	protected override void start () 
	{
		m_Health.m_MaxHealth = 3;
		m_CombatRange = 6.0f;
		m_Agent = this.gameObject.GetComponent<NavMeshAgent>();
	}

	protected override void reset()
	{
		m_Health.m_MaxHealth = 3;
	}

	/// <summary>
	/// Override the die state from the BaseEnemy
	/// to set the die state for the Furbulls.
	/// </summary>
	protected override void die()
	{
		//TODO:play death animation and instantiate ragdoll
		for(int i = 0; i < m_Players.Length; i++)
		{		
			m_Players[i].GetComponent<PlayerAIStateMachine>().RemoveEnemy(this.gameObject);
		}
		sendEvent (ObeserverEvents.Destroyed);	
		Destroy (this.gameObject);
	}

	/// <summary>
	/// Abstract fight state that is overridden to do the
	/// logic of the Furbulls' fighting state
	/// </summary>
	protected override void fightState()
	{
		//TODO:play attack animation and attack sound
		//m_EnemyPathfinding.SetState (EnemyPathfindingStates.Combat);

		if(!m_HasTarget)
		{
			m_HasTarget = true;
			m_Agent.enabled = false;

			Vector3 direction = m_Target.position - transform.position;

			direction.Normalize ();

			direction *= m_ChargeDistance;

			m_ChargeTarget = new GameObject();
			m_ChargeTarget.transform.position = gameObject.transform.position + direction;

			m_ChargeTarget.tag = "ChargeTarget";
			SphereCollider sphereCollider = m_ChargeTarget.AddComponent<SphereCollider>();
			sphereCollider.radius = 1.0f;
			sphereCollider.isTrigger = true;
		}

		if(!m_HasReachedTarget)
		{
			transform.position = Vector3.MoveTowards(transform.position, m_ChargeTarget.transform.position, m_ChargeSpeed * Time.deltaTime);
		}
		else
		{
			//EXIT TIMER STUFF HERE
			//If timer done go back to default
			if(m_FightTimer >= CHARGE_DELAY)
			{
				m_FightTimer = 0;
				m_HasTarget = false;
				m_HasReachedTarget = false;
				m_Agent.enabled = true;
				m_State = States.Default;
			}
			else
			{
				m_FightTimer += Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			if(!m_HasReachedTarget)
			{
				PlayerState playerState = other.gameObject.GetComponent<PlayerState>();
				if (playerState.m_IsActive)
				{
					playerState.FlagDamage(1);
				}

				else 
				{
					other.gameObject.GetComponent<PlayerAIStateMachine>().ApplyDamage(1);
				}

				//Debug.Log("Hit Player");
				return;
			}
		}

		if(other.gameObject == m_ChargeTarget)
		{
			m_HasReachedTarget = true;
			Destroy(m_ChargeTarget);
			m_ChargeTarget = null;
		}
	}
}
