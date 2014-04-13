using UnityEngine;
using System.Collections;

public class TrollMage : BaseEnemy
{
	int m_ShieldHealth = 100;

	NavMeshAgent m_Agent;

	// Use this for initialization
	void start () 
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

	}

	public override void applyDamage (int amount)
	{
		//TODO: damage shield / take damage
	}
}
