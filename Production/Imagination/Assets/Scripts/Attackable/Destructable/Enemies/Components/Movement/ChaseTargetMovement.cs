/*
 * Created by Joe Burchill November 27/2014
 * 
 * 
 */

#region ChangeLog
/*
 * 
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
            m_Agent.SetDestination(target.transform.position);

			return target.transform.position;
        }

		return Vector3.zero;
    }
}
