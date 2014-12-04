/*
 * Created by Joe Burchill December 2/2014
 * Movement created for an enemy that gets knocked back
 * and is sent in the opposite direction of its target
 */

#region ChangeLog
/*
 */
#endregion

using UnityEngine;
using System.Collections;

public class KnockedBackMovement : BaseMovement 
{
	private Vector3 m_DestinationPosition;
	private float m_KnockbackDistance = 2.0f;

    public override Vector3 Movement(GameObject target)
    {
		Vector3 currentDirection = target.transform.position - transform.position;
		currentDirection = currentDirection.normalized;
		//transform.LookAt (target.transform.position);

		//m_DestinationPosition = currentDirection * m_KnockbackDistance;
		m_DestinationPosition = target.transform.forward * -m_KnockbackDistance;

		Debug.DrawRay (transform.position, m_DestinationPosition, Color.green, 1.0f);

		Debug.Log ("Agent1: " + m_Agent.destination);
		m_Agent.SetDestination (m_DestinationPosition);
		//m_Agent.destination = m_DestinationPosition;

		Debug.Log ("Dest: "+m_DestinationPosition);
		Debug.Log ("Agent2: " + m_Agent.destination);
		Debug.Log ("CurrentPos: "+transform.position);

        return m_DestinationPosition;
    }
}
