using UnityEngine;
using System.Collections;

public static class Bezier 
{
	// Cubic bezier curve
	public static Vector3 GetPoint (Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t) 
	{
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return (Mathf.Pow (oneMinusT, 3.0f) * point0 + 3.0f *
						Mathf.Pow (oneMinusT, 2.0f) * t * point1 + 3.0f * 
						oneMinusT * Mathf.Pow (t, 2) * point2 + Mathf.Pow (t, 3.0f) * 
						point3);
				
				
	}

}
