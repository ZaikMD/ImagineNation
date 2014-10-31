using UnityEngine;
using System.Collections;

public static class Bezier 
{
	// Cubic bezier curve
	public static Vector3 GetPoint (Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float time) 
	{
		time = Mathf.Clamp01(time);
		float oneMinusT = 1f - time;
		return (Mathf.Pow (oneMinusT, 3.0f) * point0 + 3.0f *
		        Mathf.Pow (oneMinusT, 2.0f) * time * point1 + 3.0f * 
		        oneMinusT * Mathf.Pow (time, 2) * point2 + Mathf.Pow (time, 3.0f) * 
				point3);
				
				
	}

}
