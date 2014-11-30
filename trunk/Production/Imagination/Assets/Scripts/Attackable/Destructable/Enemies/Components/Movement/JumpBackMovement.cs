/*
 * Created by Mathieu Elias November 24/2014
 * 
 * //This movement component finds a location the enemy can jump back to then moves him
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class JumpBackMovement : BaseMovement 
{
	// Distance to jump back
	public float m_JumpBackDistance = 5.0f;
	// Target to jump back from
	Transform m_Target;

	Vector3 m_Destination;

	// The override of the abstract movement function
	public override Vector3 Movement (GameObject target)
	{
		m_Target = target.transform;

		transform.LookAt (m_Target.position);		
		m_Destination = FindJumpBackPoint (0.0f);

		if (m_Destination == Vector3.zero)
			return Vector3.zero;

		m_Agent.SetDestination (m_Destination);
		return m_Destination;
	}

	// Uses Raycasting and recursion to find a location that we can jump back to and returns it
	private Vector3 FindJumpBackPoint(float angle)
	{
		// If we have done a 360 so far there is no where to jump back to
		// so return zero
		if (angle == 360.0f)
			return Vector3.zero;

		// Grab a point around the target we want to jump away from
		Vector3 point = RotateAboutOrigin (transform.position, m_Target.position, angle);

		// Add our jump back distance to the point
		point += (transform.position - m_Target.position).normalized * m_JumpBackDistance;

		// direction of the point
		Vector3 dir = point - transform.position;
		RaycastHit hitInfo;
		
		//If we collided with something we need to choose a new point
		if (Physics.Raycast (transform.position, dir, out hitInfo, m_JumpBackDistance))		
			FindJumpBackPoint(angle + 20);		

		// if we didnt then we return that point
		return point;
	}
}
