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
	public float AggroRange = 20.0f;
	
	public override bool LeaveCombat(Transform target)
	{
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
