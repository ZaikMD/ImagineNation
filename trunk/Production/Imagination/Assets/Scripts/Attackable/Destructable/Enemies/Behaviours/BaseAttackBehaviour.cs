/*
 * Created by Joe Burchill & Mathieu Elias November 14/2014
 * The Base Attack Behaviour which every other attack behaviour will
 * inherit from. Contains Component variables and abstract update
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseAttackBehaviour : BaseBehaviour 
{
    public BaseCombat m_CombatComponent;
	public BaseTargeting m_TargetingComponent;
	public BaseMovement m_MovementComponent;

    public abstract void update();

	protected virtual void Combat()
	{
		if (m_EnemyAI.m_UCombat)
			m_CombatComponent.Combat();
	}

	protected virtual GameObject Target()
	{
		if (m_EnemyAI.m_UTargeting)
			return m_TargetingComponent.CurrentTarget ();

		return null;
	}

	protected virtual void Movement()
	{
		if (m_EnemyAI.m_UMovement)
			m_MovementComponent.Movement (m_Target);
	}

}
