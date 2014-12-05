/*
 * Created by Joe Burchill November 14/2014
 * Chase Behaviour for the Furbull, calls
 * Chase Component
 */

#region ChangeLog
/*
 * Added the base.update to the begining of the update so the controller can take over. Dec 1 - Mathieu Elias
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class FurbullChaseBehaviour : BaseChaseBehaviour 
{
	protected override void start ()
    {
        //Call all the component start functions
        m_LeavingCombatComponent.start(this);
        m_MovementComponent.start(this);
        m_TargetingComponent.start(this);
    }

	public override void update()
	{
        //Set the target
        m_Target = Target();

        //Set state to idle if the target is null
        if (m_Target == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        //If we leave combat set the state to idle
        if (LeaveCombat(m_Target.transform))
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        //If we are within attack range switch to attack state
        if (GetDistanceToTarget() < Constants.FURBULL_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);
        }

        //Call Movement
        Movement();
		base.update ();
	}

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, m_Target.transform.position);
    }
}
