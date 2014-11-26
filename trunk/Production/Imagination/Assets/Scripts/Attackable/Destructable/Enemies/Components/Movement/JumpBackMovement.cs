using UnityEngine;
using System.Collections;

public class JumpBackMovement : BaseMovement 
{
	public float m_JumpBackDistance = 5.0f;
	Transform m_Target;
	Vector3 m_Destination;

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

	private Vector3 FindJumpBackPoint(float angle)
	{
		if (angle == 360.0f)
			return Vector3.zero;
		
		Vector3 point = RotateAboutOrigin (transform.position, m_Target.position, angle);
		
		point += (transform.position - m_Target.position).normalized * m_JumpBackDistance;
		
		Vector3 dir = point - transform.position;
		RaycastHit hitInfo;
		
		//If we collided with something we need to choose a new point
		if (Physics.Raycast (transform.position, dir, out hitInfo, m_JumpBackDistance))		
			FindJumpBackPoint(angle + 20);		
		
		return point;
	}
}
