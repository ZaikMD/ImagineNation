/*
 * Created by Joe Burchill November 14/2014
 * Idle Behaviour for the Furbull, calls
 * Idle Components
 */

#region ChangeLog
/*
 * Added the base.update to the begining of the update so the controller can take over. Dec 1 - Mathieu Elias
 */
#endregion

using UnityEngine;
using System.Collections;

public class FurbullIdleBehaviour : BaseIdleBehaviour 
{

	protected override void start ()
    {
        m_MovementComponent.start(this);
        m_EnterCombatComponent.start(this);
		m_TargetingComponent.start (this);
    }

    public override void update()
	{
		base.update ();

		m_Target = Target ();

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
