using UnityEngine;
using System.Collections;

public static class Helpers 
{
	//a lerp that wont over shoot the target
    public static Vector3 lerpVector3(Vector3 from, Vector3 to, float amount)
	{
        Vector3 direction = to - from;

        direction = Vector3.ClampMagnitude(direction * amount, (to - from).magnitude);

        return from + direction;
    }
}
