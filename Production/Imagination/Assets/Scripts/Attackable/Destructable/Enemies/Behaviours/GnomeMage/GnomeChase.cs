using UnityEngine;
using System.Collections;
/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script handles the Chase functionality of the Gnome Mage enemy
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion

public class GnomeChase : BaseChaseBehaviour 
{
	public BaseCombat m_CombatComponent;

	const float m_TimeBetweenShots = 1.5f;
	float m_ShotTimer;



	// Use this for initialization
	protected override void start ()
	{
		//initialise all the components
		m_CombatComponent.start (this);
		m_LeavingCombatComponent.start (this);
		m_MovementComponent.start (this);
		m_TargetingComponent.start (this);
	}
	
	// Update is called once per frame
	public override void update () 
	{  
		//Grab the current target
		setTarget(Target ());

		// If there is no target either the player is dead, gone or something went wront so switch to the idle state
		// OR
		// If its time to leave combat set the current state to idle
		if (getTarget() == null || LeaveCombat(getTarget().transform))
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
			return;
		}

		// If we are in attack range switch to the attack state
		float dist = Vector3.Distance (transform.position, getTarget().transform.position); 
		if (dist <= Constants.MAGE_ATTACK_RANGE)
		{
			m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);
			return;
		}

		// move
		Movement ();

		if (m_ShotTimer <= 0)
		{
			// Manually doing the check if the controller has taken over combat because the base does not have combat included
			if (m_EnemyAI.m_UCombat)
				m_CombatComponent.Combat (getTarget());
			m_ShotTimer = m_TimeBetweenShots;
		}

		m_ShotTimer -= Time.deltaTime;
	}

	public override void ComponentInfo (out string[] names, out BaseComponent[] components)
	{
		names = new string[4];
		components = new BaseComponent[4];
		
		names [0] = "Movement";
		components [0] = m_MovementComponent;
		
		names [1] = "Targeting";
		components [1] = m_TargetingComponent;
		
		names [2] = "Leave Combat";
		components [2] = m_LeavingCombatComponent;

		names [3] = "Combat";
		components [3] = m_CombatComponent;
	}

	public override int numbComponents ()
	{
		return 4;
	}
	
	public override void SetComponents (string[] components)
	{
		m_ComponentsObject = transform.FindChild ("Components").gameObject;
		
		m_MovementComponent = m_ComponentsObject.GetComponent (components [0]) as BaseMovement;
		m_TargetingComponent = m_ComponentsObject.GetComponent (components [1]) as BaseTargeting;
		m_LeavingCombatComponent = m_ComponentsObject.GetComponent (components [2]) as BaseLeavingCombat;
		m_CombatComponent = m_ComponentsObject.GetComponent (components [3]) as BaseCombat;
	}
}
