﻿/*
 * Created by Joe Burchill November 14/2014
 * Idle Behaviour for the Spin Top, calls
 * idle component
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class SpinningTopIdleBehaviour : BaseIdleBehaviour 
{
    protected override void start()
    {
        m_MovementComponent.start(this);
        m_EnterCombatComponent.start(this);
        m_TargetingComponent.start(this);
    }

	public override void update()
	{
        m_Target = Target();

        if (m_MovementComponent != null)
        {
            Movement();
        }

        if (m_Target != null)
        {
            if (m_EnterCombatComponent.EnterCombat(m_Target.transform) == true)
            {
                m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
            }
        }
	}
}
