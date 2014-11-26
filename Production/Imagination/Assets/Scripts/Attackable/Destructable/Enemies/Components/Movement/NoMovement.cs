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
