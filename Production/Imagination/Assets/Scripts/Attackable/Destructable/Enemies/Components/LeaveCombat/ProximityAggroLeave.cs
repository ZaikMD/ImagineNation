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
