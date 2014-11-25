/*
 * Created by Joe Burchill & Mathieu Elias November 14/2014
 * The Base Idle Behaviour which every other idle behaviour will
 * inherit from. Contains Component variables and abstract update
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public abstract class BaseIdleBehaviour : BaseBehaviour 
{
	public BaseMovement m_MovementComponent;
	public BaseEnterCombat m_EnterCombatComponent;
	public BaseTargeting m_TargetingComponent;	

    public abstract void update(); 

	protected virtual bool EnterCombat(Transform target)
	{
		if (m_EnemyAI.m_UEnterCombat)
			return m_EnterCombatComponent.EnterCombat (target);

		return false;
	}
	protected virtual void Movement()
	{
		if (m_EnemyAI.m_UMovement)
			m_MovementComponent.Movement (m_Target);
	}

	protected virtual GameObject Target()
	{
		if (m_EnemyAI.m_UTargeting)
			return m_TargetingComponent.CurrentTarget ();
		
		return null;
	}
}
