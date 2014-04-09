using UnityEngine;
using System.Collections;

public class Furbulls : BaseEnemy 
{
	public GameObject m_FurbullsProjectile;

	// Use this for initialization
	protected override void start () 
	{
		m_Health.m_MaxHealth = 3;
		m_CombatRange = 2.0f;
	}

	/// <summary>
	/// Override the die state from the BaseEnemy
	/// to set the die state for the Furbulls.
	/// </summary>
	protected override void die()
	{
		//TODO:play death animation and instantiate ragdoll
		Destroy (this.gameObject);
	}

	/// <summary>
	/// Abstract fight state that is overridden to do the
	/// logic of the Furbulls' fighting state
	/// </summary>
	protected override void fightState()
	{
		//TODO:play attack animation and attack sound
		Instantiate (m_FurbullsProjectile, this.transform.position, this.transform.rotation);
		m_EnemyPathfinding.SetState (EnemyPathfindingStates.Combat);
		m_State = States.Default;
	}
}
