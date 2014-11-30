/*
 * Created by Mathieu Elias November 17/2014
 * 
 * This Movement component does nothing however it is needed for consistency. 
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class NoMovement : BaseMovement 
{
	public override Vector3 Movement (GameObject target)
	{
		//Do Nothing
		return Vector3.zero;
	}
}
