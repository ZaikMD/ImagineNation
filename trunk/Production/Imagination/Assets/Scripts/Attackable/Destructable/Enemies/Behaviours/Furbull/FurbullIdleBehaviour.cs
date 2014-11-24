/*
 * Created by Joe Burchill November 14/2014
 * Idle Behaviour for the Furbull, calls
 * Idle Components
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class FurbullIdleBehaviour : BaseIdleBehaviour 
{

    void Start()
    {
        m_MovementComponent.start(this);
        m_EnterCombatComponent.start(this);
    }

    public override void update()
	{
        if (m_MovementComponent != null)
        {
            m_MovementComponent.Movement();
        }

        if (m_EnterCombatComponent.EnterCombat() == true)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
        }
	}
}
