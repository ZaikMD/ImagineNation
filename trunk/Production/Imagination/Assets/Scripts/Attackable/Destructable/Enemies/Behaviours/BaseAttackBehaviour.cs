﻿/*
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
	// The components all attack behaviours must have
    public BaseCombat m_CombatComponent;
	public BaseTargeting m_TargetingComponent;
	public BaseMovement m_MovementComponent;

    public abstract void update();

	// The following functions must be called to update any of the components in order to make sure the 
	// enemy controller hasn't taken control of any of them
	protected virtual void Combat()
	{
		if (m_EnemyAI.m_UCombat)
		{
			if(m_CombatComponent != null)
				m_CombatComponent.Combat();
		}
	}
	protected virtual GameObject Target()
	{
		if (m_EnemyAI.m_UTargeting)
		{
			if(m_TargetingComponent != null)
				return m_TargetingComponent.CurrentTarget ();
		}

		return null;
	}
	protected virtual void Movement()
	{
		if (m_EnemyAI.m_UMovement)
		{
			if(m_MovementComponent != null)
				m_MovementComponent.Movement (m_Target);
		}
	}

}
