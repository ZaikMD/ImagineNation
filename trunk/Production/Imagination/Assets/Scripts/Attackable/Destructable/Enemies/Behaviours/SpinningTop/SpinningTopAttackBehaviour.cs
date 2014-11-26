/*
 * Created by Joe Burchill November 14/2014
 * Attack Behaviour for the Spin Top, calls
 * Attack Component
 */

#region ChangeLog
/*
 * Added Update functionality to change state - Joe Burchill 2014/11/26
 */
#endregion

using UnityEngine;
using System.Collections;

public class SpinningTopAttackBehaviour : BaseAttackBehaviour 
{
    protected override void start()
    {
        m_CombatComponent.start(this);
        m_TargetingComponent.start(this);
        m_MovementComponent.start(this);
    }

	public override void update()
	{
        m_Target = Target();

        if (m_Target == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        if (GetDistanceToTarget() >= Constants.SPIN_TOP_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
        }

        Movement();

        Combat();
    }

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, m_Target.transform.position);
    }
}
