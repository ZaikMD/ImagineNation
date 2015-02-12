/*
 * Created by Joe Burchill November 27/2014
 * 
 */

#region ChangeLog
/*
 * Added the agent stopping distance. Dec 3. - Mathieu Elias
 * Changed Stopping Distance to a Constant - Joe Burchill Dec. 4/2014
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class ChaseTargetMovement : BaseMovement 
{
	private const float CHASE_STOPPING_DISTANCE = 1.0f;

    public override Vector3 Movement(GameObject target)
    {
		//If we have a target
        if (target != null)
        {
			//Change Agent Stopping Distance
			m_Agent.stoppingDistance = CHASE_STOPPING_DISTANCE;
            m_Agent.SetDestination(target.transform.position);

			return target.transform.position;
        }

		return Vector3.zero;
    }
}
