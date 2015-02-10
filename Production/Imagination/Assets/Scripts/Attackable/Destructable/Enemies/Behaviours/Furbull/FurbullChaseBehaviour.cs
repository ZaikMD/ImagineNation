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

    new public AnimatorFurbull EnemyAnimator
    {
        get
        {
            return base.EnemyAnimator as AnimatorFurbull;
        }
    }

	protected override void start ()
    {
        //Call all the component start functions
        m_LeavingCombatComponent.start(this);
        m_MovementComponent.start(this);
        m_TargetingComponent.start(this);
    }

	public override void update()
	{
        EnemyAnimator.setFloat("Speed", 0.0f);
        //Set the target
		setTarget(Target());

        //Set state to idle if the target is null
		if (getTarget() == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        //If we leave combat set the state to idle
		if (LeaveCombat(getTarget().transform))
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        //If we are within attack range switch to attack state
        if (GetDistanceToTarget() < Constants.FURBULL_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);
        }

        //EnemyAnimator.playAnimation(AnimatorFurbull.Animations.Run);

        //Call Movement
        Movement();
		base.update ();
	}

    private float GetDistanceToTarget()
    {
		return Vector3.Distance(transform.position, getTarget().transform.position);
    }
}
