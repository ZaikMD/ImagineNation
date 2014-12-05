/*
 * Created by Joe Burchill November 14/2014
 * Attack Behaviour for the Furbull, calls
 * Attack Component
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class FurbullAttackBehaviour : BaseAttackBehaviour 
{
	const float MIN_CHARGE_UP_TIME = 0.2f;
	const float MAX_CHARGE_UP_TIME = 0.6f;
	float m_ChargeUpTimer;

	protected override void start ()
    {
		//Setting Random Charge up timer
		m_ChargeUpTimer = Random.Range (MIN_CHARGE_UP_TIME, MAX_CHARGE_UP_TIME);

        //Call start functionality for each component
        m_CombatComponent.start(this);
        m_TargetingComponent.start(this);
        m_MovementComponent.start(this);
    }

	public override void update()
	{
        //Set the target
        m_Target = Target();

        //if we dont have a target switch to idle
        if (m_Target == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        //if we are not in attack range chase the target
        if (GetDistanceToTarget() >= Constants.FURBULL_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
        }

        //look at the target
		transform.LookAt (m_Target.transform.position);

        //Call the movement
        Movement();

        //if our charge up timer is less than or equal to 0, call Combat
		if (m_ChargeUpTimer <= 0)
		{
       	    Combat();
			m_ChargeUpTimer = Random.Range (MIN_CHARGE_UP_TIME, MAX_CHARGE_UP_TIME);
		}

        //Decrement timer
		m_ChargeUpTimer -= Time.deltaTime;
	}

    private float GetDistanceToTarget()
    {
        //Return Distance between target and current position
        return Vector3.Distance(transform.position, m_Target.transform.position);
    }
}
