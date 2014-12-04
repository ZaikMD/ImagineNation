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
	//Public aggro range, customizable in the inspector
    public float AggroRange = 20.0f;

	//Override Enter Combat
    public override bool EnterCombat(Transform target)
    {
		//Check distance between target and position
		float dist = Vector3.Distance (transform.position, target.position);

		//Return true if distance is less than aggro range
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
