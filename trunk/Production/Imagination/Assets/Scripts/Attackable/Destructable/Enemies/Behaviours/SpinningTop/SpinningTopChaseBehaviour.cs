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
    new public AnimatorSpinTops EnemyAnimator
    {
        get
        {
            return base.EnemyAnimator as AnimatorSpinTops;
        }
    }

    protected override void start()
    {
		//Call components start functions
        m_LeavingCombatComponent.start(this);
        m_MovementComponent.start(this);
        m_TargetingComponent.start(this);
    }

	public override void update()
	{
		//Set Target
        setTarget(Target());

		//If Target is null then enter idle state
		if (getTarget() == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

		//Set state to idle if we need to leave combat
		if (LeaveCombat(getTarget().transform))
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

		//Check attack range, set state accordingly
        if (GetDistanceToTarget() < Constants.SPIN_TOP_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Attack);
        }

        EnemyAnimator.playAnimation(AnimatorSpinTops.Animations.Idle);

		//Call movement
        Movement();
    }

	//Return distance between target and the enemy
    private float GetDistanceToTarget()
    {
		return Vector3.Distance(transform.position, getTarget().transform.position);
    }
}
