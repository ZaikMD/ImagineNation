/*
 * Created by Mathieu Elias November 17/2014
 * 
 * This LeaveCombat component uses aggrorange to decide when it is time to leave combat
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class ProximityAggroLeave : BaseLeavingCombat
{
	//Public aggro range, customizable in the inspector
	public float AggroRange = 20.0f;

	//Override Leave Combat
	public override bool LeaveCombat(Transform target)
	{
		//Return true if distance is less than aggro range
		if (Vector3.Distance(transform.position, target.position) > AggroRange)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
