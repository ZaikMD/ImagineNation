/*
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
    new public AnimatorSpinTops EnemyAnimator
    {
        get
        {
            return base.EnemyAnimator as AnimatorSpinTops;
        }
    }

    protected override void start()
    {
		//Call the start for each of the Idle Components
        m_MovementComponent.start(this);
        m_EnterCombatComponent.start(this);
        m_TargetingComponent.start(this);
    }

	public override void update()
	{
        EnemyAnimator.playAnimation(AnimatorSpinTops.Animations.Idle);

		//Set the target
        setTarget(Target());

		//Call Movement
        if (m_MovementComponent != null)
        {
            Movement();
        }

		//Set state to chase if the enter combat is called
		if (getTarget() != null)
        {
			if (m_EnterCombatComponent.EnterCombat(getTarget().transform) == true)
            {
                m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
            }
        }
	}
}
