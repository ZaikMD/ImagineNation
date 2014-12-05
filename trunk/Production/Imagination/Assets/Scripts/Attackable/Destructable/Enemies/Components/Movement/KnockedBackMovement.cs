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
        if (target != null)
        {
            //Get Current Direction and normalize it
            Vector3 currentDirection = target.transform.position - transform.position;
            currentDirection = currentDirection.normalized;

            //Get our destination position
            m_DestinationPosition = target.transform.forward * -m_KnockbackDistance;

#if DEBUG || UNITY_EDITOR
            Debug.DrawRay(transform.position, m_DestinationPosition, Color.green, 1.0f);
#endif

            //Set Destination to the agent
            m_Agent.SetDestination(m_DestinationPosition);

            return m_DestinationPosition;
        }

        return Vector3.zero;
    }
}
