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
	//The components all chase behaviours must have
	public BaseMovement m_MovementComponent;
	public BaseTargeting m_TargetingComponent;
	public BaseLeavingCombat m_LeavingCombatComponent;

    public virtual void update()
	{
		EnemyController controller = m_EnemyAI.GetController();
		if (controller != null && m_ControllerSet == false)
			controller.AggroGroup (true, getTarget());
	}

	// The following functions must be called to update any of the components in order to make sure the 
	// enemy controller hasn't taken control of any of them
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
	protected virtual bool LeaveCombat(Transform target)
	{
		if (m_EnemyAI.m_ULeaveCombat)
		{
			if(m_LeavingCombatComponent != null)
				return m_LeavingCombatComponent.LeaveCombat (target);
		}

		return false;
	}

	public override void ComponentInfo (out string[] names, out BaseComponent[] components)
	{
		names = new string[3];
		components = new BaseComponent[3];
		
		names [0] = "Movement";
		components [0] = m_MovementComponent;
		
		names [1] = "Targeting";
		components [1] = m_TargetingComponent;
		
		names [2] = "Leave Combat";
		components [2] = m_LeavingCombatComponent;
	}

	public override string BehaviourType()
	{
		return "Chase Behaviour";
	}
}
