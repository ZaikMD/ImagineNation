/*
 * Created by Joe Burchill November 14/2014
 * Chase Behaviour for the Spin Top, calls
 * Chase Component
 */

#region ChangeLog
/*
 * Added State changing within the update function and called the component starts - Joe Burchill 2014/11/26
 */
#endregion

using UnityEngine;
using System.Collections;

public class SpinningTopChaseBehaviour : BaseChaseBehaviour 
{
    protected override void start()
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

        if (GetDistanceToTarget() < Constants.SPIN_TOP_ATTACK_RANGE)
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
