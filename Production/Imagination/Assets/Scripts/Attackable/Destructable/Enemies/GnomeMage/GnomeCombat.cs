using UnityEngine;
using System.Collections;
/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script handles the functionality of the Gnome Mage enemy
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion

public class GnomeCombat : BaseAttackBehaviour 
{
	enum CombatStates
	{
		Regular = 0,
		Cloning,
		Cloned
	}

	BaseTargeting m_Targeting;
	GameObject m_Target;

	BaseCombat m_RegularCombat;
	BaseMovement m_RegularMovement;

	BaseCombat m_ClonedCombat;
	BaseCombat m_ClonedMovement;

	// Use this for initialization
	void Start () 
	{
		m_Targeting.start (this);

		m_RegularCombat.start (this);
		m_RegularMovement.start (this);

		m_ClonedCombat.start (this);
		m_ClonedMovement.start (this);
	}
	
	// Update is called once per frame
	public override void update ()
	{
		m_Target = m_Targeting.CurrentTarget ();

		if (m_Target == null)
		{
			m_EnemyAI.SetState (EnemyAI.EnemyState.Idle);
			return;
		}

		float dist = Vector3.Distance (transform.position, m_Target.transform.position); 
		if (dist >= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);

		m_RegularCombat.Combat ();
	}
}
