﻿/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script handles the Idle functionality of the Gnome Mage enemy
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class GnomeIdle : BaseIdleBehaviour 
{
	new public AnimatorGnomeMage EnemyAnimator
	{
		get
		{
			return base.EnemyAnimator as AnimatorGnomeMage;
		}
	}
	// Use this for initialization
	protected override void start ()
	{
		base.update ();
		//initialise all components
		m_MovementComponent.start (this);
		m_EnterCombatComponent.start (this);
		m_TargetingComponent.start (this);
	}
	
	// Update is called once per frame 
	public override void update()
	{
		EnemyAnimator.playAnimation (AnimatorGnomeMage.Animations.Hover);
		//Grab current target
		setTarget(Target ());

		//move
		Movement ();

		// If we have a target and its time to enter combat switch the state to chase
		if (getTarget() != null)
		{
			if (EnterCombat(getTarget().transform))
				m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
		}
	}
}
