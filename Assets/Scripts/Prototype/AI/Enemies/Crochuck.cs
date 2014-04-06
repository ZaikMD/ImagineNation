using UnityEngine;
using System.Collections;

//Last updated 04/06/2014

[RequireComponent(typeof(EnemyPathfinding))]
public class Crochuck : BaseEnemy 
{
	//Literally a furbull 
	public Transform m_FurbullPrefab;

	public GameObject m_FurbullPrefabSpawnPoint;

	public PlayerState m_Player;

	EnemyPathfinding m_EnemyPathfinding;

	float m_CurrentFurblowTimer = 0.0f;
	const float m_FurblowTimer = 10.0f;

	float m_CurrentMeleeAttackCooldown = 0.0f;
	const float m_MeleeAttackCooldown = 3.0f;

	bool m_TimeToFurblow = false;

	// Use this for initialization
	void Start () 
	{
		m_Health.m_MaxHealth = 10;
		m_CombatRange = 1.0f;
		m_Player = m_Player.GetComponent<PlayerState> ();
		m_EnemyPathfinding = this.gameObject.GetComponent<EnemyPathfinding> ();
	}

	protected override void die()
	{
		//TODO:play death animation and instantiate ragdoll
		Destroy (this.gameObject);
	}

	protected override void fightState()
	{
		if(m_CurrentFurblowTimer >= m_FurblowTimer)
		{
			//TODO:play attack animation and attack sounds
			Transform tempFurbull;
			tempFurbull = (Transform) Instantiate(m_FurbullPrefab,
			                                      m_FurbullPrefabSpawnPoint.transform.position,
			                                      Quaternion.identity);

			tempFurbull.transform.rotation = m_FurbullPrefabSpawnPoint.transform.rotation;
			m_CurrentFurblowTimer = 0.0f;
			m_CurrentMeleeAttackCooldown = 0.0f;

		}
		else if(m_CurrentMeleeAttackCooldown >= m_MeleeAttackCooldown)
		{
			//TODO:play attack animation and attack sounds
			//Switch the playerstate to taking damage
			m_CurrentMeleeAttackCooldown = 0.0f;
		}
	}
	
	// Update is called once per frame
	protected override void updateCombat () 
	{
		m_CurrentFurblowTimer += Time.deltaTime;
		m_CurrentMeleeAttackCooldown += Time.deltaTime;
	}
}
