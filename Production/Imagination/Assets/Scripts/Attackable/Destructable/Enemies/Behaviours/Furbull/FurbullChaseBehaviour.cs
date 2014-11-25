/*
 * Created by Joe Burchill November 14/2014
 * Chase Behaviour for the Furbull, calls
 * Chase Component
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class FurbullChaseBehaviour : BaseChaseBehaviour 
{
	protected override void start ()
    {
        m_LeavingCombatComponent.start(this);
        m_MovementComponent.start(this);
        m_TargetingComponent.start(this);
    }

	public override void update()
	{
        m_Target = Target();

        if (m_Target == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        if (LeaveCombat(m_Target.transform))
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        if (GetDistanceToTarget() < Constants.FURBULL_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);
        }

        Movement();
	}

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, m_Target.transform.position);
    }
}
