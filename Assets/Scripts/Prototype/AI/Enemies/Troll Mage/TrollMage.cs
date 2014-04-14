using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class TrollMage : BaseEnemy
{
	public GameObject m_TrollMagePrefab;
	public GameObject m_TrollBeamPrefab;

	float m_CloneTimer = 0.0f;
	public const float CLONE_DELAY = 2.0f;
	float m_FireTimer = 0.0f;
	public const float FIRE_DELAY = 4.0f;

	NavMeshAgent m_Agent;

	bool m_IsClone = false; 

	public Shield m_Shield;

	public const int MAX_CLONES = 2;

	GameObject[] m_Clones = new GameObject[MAX_CLONES];
	bool [] m_ActiveClones = new bool[MAX_CLONES];

	// Use this for initialization
	protected override void start () 
	{
		m_Health.resetHealth();
		
		m_CombatRange = 30.0f;
		
		m_Agent = this.gameObject.GetComponent<NavMeshAgent>();

		Invoke ("load", 0.5f);
	}

	void load()
	{
		if(!m_IsClone)
		{
			for( int i = 0; i < m_Clones.Length; i++)
			{
				m_Clones[i] = (GameObject)(Instantiate(m_TrollMagePrefab, transform.position, transform.rotation));
				m_Clones[i].GetComponentInChildren<TrollMage>().turnIntoClone();
				m_Clones[i].SetActive(false);
				m_ActiveClones[i] = false;
			}
		}
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

				for( int i = 0; i < m_Clones.Length; i++)
				{
					if(m_ActiveClones[i] != true)
					{
						Vector3 offset = Random.insideUnitSphere;
						offset.y = 0;
						offset.Normalize();

						m_ActiveClones[i] = true;
						m_Clones[i].SetActive(true);

						m_Clones[i].transform.position = transform.position + offset;
						break;
					}
				}
			}
		}

		if(m_FireTimer >= FIRE_DELAY)
		{
			m_FireTimer = 0.0f;

			GameObject beamObj = (GameObject)(Instantiate(m_TrollBeamPrefab, transform.position, transform.rotation));
			TrollBeam beam = beamObj.GetComponent<TrollBeam>();
			beam.m_Target = m_Target.gameObject;
			if(m_IsClone)
			{
				beam.turnIntoClone();
			}
		}

		if(!m_IsClone)
		{
			m_State = States.Default;
		}
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
		for(int i = 0; i < m_Clones.Length; i++)
		{
			if(m_Clones[i] != null)
			{
				m_Clones[i].SetActive(false);
			
				m_ActiveClones[i] = false;
			}
		}
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
