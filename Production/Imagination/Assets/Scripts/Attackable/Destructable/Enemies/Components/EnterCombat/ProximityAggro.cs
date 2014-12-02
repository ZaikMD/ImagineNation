/*
 * Created by Joe Burchill November 17/2014
 * 
 * This EnterCombat component uses an aggro range to decide if it is time to enter combat
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class ProximityAggro : BaseEnterCombat 
{
    public float AggroRange = 20.0f;

    public override bool EnterCombat(Transform target)
    {
		float dist = Vector3.Distance (transform.position, target.position);

		if (dist < AggroRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
