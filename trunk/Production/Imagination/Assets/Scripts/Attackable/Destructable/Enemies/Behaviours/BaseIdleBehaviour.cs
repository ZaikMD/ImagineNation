﻿/*
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
	//Components all idle behaviours must have
	public BaseMovement m_MovementComponent;
	public BaseTargeting m_TargetingComponent;	
	public BaseEnterCombat m_EnterCombatComponent;


    public virtual void update()
	{
		EnemyController controller = m_EnemyAI.GetController();
		if (controller != null && m_ControllerSet == true)
			controller.AggroGroup (false, getTarget());
	}

	// The following functions must be called to update any of the components in order to make sure the 
	// enemy controller hasn't taken control of any of them
	protected virtual bool EnterCombat(Transform target)
	{
		if (m_EnemyAI.m_UEnterCombat)
		{
			if(m_EnterCombatComponent != null)
				return m_EnterCombatComponent.EnterCombat (target);
		}

		return false;
	}
	protected virtual void Movement()
	{
		if (m_EnemyAI.m_UMovement)
		{
			if(m_MovementComponent != null)
				m_MovementComponent.Movement (getTarget());
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

	public override void ComponentInfo (out string[] names, out BaseComponent[] components)
	{
		names = new string[3];
		components = new BaseComponent[3];
		
		names [0] = "Movement";
		components [0] = m_MovementComponent;
		
		names [1] = "Targeting";
		components [1] = m_TargetingComponent;
		
		names [2] = "Enter Combat";
		components [2] = m_EnterCombatComponent;
	}
}
