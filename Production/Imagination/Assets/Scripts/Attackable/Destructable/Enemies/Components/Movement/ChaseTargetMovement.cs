/*
 * Created by Joe Burchill November 27/2014
 * 
 * 
 */

#region ChangeLog
/*
 * // Added the agent stopping distance. Dec 3. - Mathieu Elias
 */
#endregion
using UnityEngine;
using System.Collections;

public class ChaseTargetMovement : BaseMovement 
{
    public override Vector3 Movement(GameObject target)
    {
        if (target != null)
        {
			m_Agent.stoppingDistance = 2.0f;
            m_Agent.SetDestination(target.transform.position);

			return target.transform.position;
        }

		return Vector3.zero;
    }
}
