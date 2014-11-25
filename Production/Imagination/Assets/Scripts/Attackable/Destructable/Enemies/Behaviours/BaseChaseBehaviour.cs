/*
 * Created by Joe Burchill & Mathieu Elias November 14/2014
 * The Base Chase Behaviour which every other chase behaviour will
 * inherit from. Contains Component variables and abstract update
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseChaseBehaviour : BaseBehaviour 
{
	public BaseMovement m_MovementComponent;
	public BaseTargeting m_TargetingComponent;
	public BaseLeavingCombat m_LeavingCombatComponent;

    public abstract void update();

	protected virtual void Movement()
	{
		if (m_EnemyAI.m_UMovement)
			m_MovementComponent.Movement ();
	}
	protected virtual GameObject Target()
	{
		if (m_EnemyAI.m_UTargeting)
			return m_TargetingComponent.CurrentTarget ();

		return null;
	}
	protected virtual bool LeaveCombat(Transform target)
	{
		if (m_EnemyAI.m_ULeaveCombat)
			return m_LeavingCombatComponent.LeaveCombat (target);

		return false;
	}
}
