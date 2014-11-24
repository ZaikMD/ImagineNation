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
    protected BaseMovement m_MovementComponent;
    protected BaseTargeting m_TargetingComponent;
    protected BaseLeavingCombat m_LeavingCombatComponent;

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
	protected virtual bool LeaveCombat()
	{
		if (m_EnemyAI.m_ULeaveCombat)
			return m_LeavingCombatComponent.LeaveCombat ();

		return false;
	}
}
