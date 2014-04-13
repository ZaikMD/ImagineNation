using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class TrollMage : BaseEnemy
{
	public GameObject m_TrollMagePrefab;
	public GameObject m_TrollBeamPrefab;

	float m_CloneTimer = 0.0f;
	public const float CLONE_DELAY = 3.0f;
	float m_FireTimer = 0.0f;
	public const float FIRE_DELAY = 0.5f;

	NavMeshAgent m_Agent;

	List <TrollMage> m_Clones = new List<TrollMage>();

	bool m_IsClone = false; 

	 public Shield m_Shield;

	// Use this for initialization
	protected override void start () 
	{
		m_Health.resetHealth();
		
		m_CombatRange = 30.0f;
		
		m_Agent = this.gameObject.GetComponent<NavMeshAgent>();
	}

	protected override void die()
	{
		//TODO: Instantiate Ragdoll
		Destroy(this.gameObject);
	}

	protected override void fightState()
	{
		m_FireTimer += Time.deltaTime;
		m_CloneTimer += Time.deltaTime;

		if(!m_IsClone)
		{
			if(m_CloneTimer >= CLONE_DELAY)
			{
				m_CloneTimer = 0.0f;

				Vector3 offset = Random.insideUnitSphere;
				offset.y = 0;
				offset.Normalize();

				GameObject troll = (GameObject)(Instantiate(m_TrollMagePrefab, transform.position + (offset * 20), transform.rotation));
				troll.GetComponentInChildren<TrollMage>().turnIntoClone();
				m_Clones.Add(troll.GetComponentInChildren<TrollMage>());
			}
		}


		m_State = States.Default;
	}

	public override void applyDamage (int amount)
	{
		//TODO: damage shield / take damage
	}

	public void turnIntoClone()
	{
		m_IsClone = true;
		m_Shield.turnIntoClone ();

		Destroy(gameObject.GetComponentInChildren<CapsuleCollider>());
	}

	public void deleteClones()
	{
		for(int i = 0; i < m_Clones.Count; i++)
		{
			Destroy(m_Clones[i].gameObject);
		}

		m_Clones.Clear ();
	}

	protected override void followState()
	{
		base.followState ();
		if(!m_IsInCombat)
		{
			deleteClones ();
		}
	}
	
	/// <summary>
	/// sets the EnemyPathFinding state to Patrol
	/// </summary>
	protected override void patrolState()
	{
		base.patrolState ();
		if(!m_IsInCombat)
		{
			deleteClones ();
		}
	}
}
