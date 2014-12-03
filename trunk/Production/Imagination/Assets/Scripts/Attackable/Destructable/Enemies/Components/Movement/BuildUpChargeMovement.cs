/*
 * Created by Joe Burchill December 2/2014
 * Movement created for an enemy that builds up its charge
 * chooses target to travel towards
 */

#region ChangeLog
/*
 * 
 */
#endregion

using UnityEngine;
using System.Collections;

public class BuildUpChargeMovement : BaseMovement 
{
    private Vector3 m_ChargeDirection;
    private Vector3 m_ChargeToPosition;
    private float m_ChargeDistance;
    private float m_DistanceToTarget;
    private const float PERCENT_TO_CHARGE_PAST_PLAYER = 1.5f;

    public override Vector3 Movement(GameObject target)
    {
        Vector3 currentPosition = transform.position;
        Vector3 destinationPosition = target.transform.position;

        //Get the direction vector between then and zero out the y axis
        m_ChargeDirection = destinationPosition - currentPosition;
        m_ChargeDirection.y = 0.0f;

        //Get the distance between the two
        m_DistanceToTarget = m_ChargeDirection.magnitude;

        //Get a distance just passed the distance to the player
        m_ChargeDistance = m_DistanceToTarget * PERCENT_TO_CHARGE_PAST_PLAYER;

        //Determine a specific position just passed the player
        m_ChargeToPosition = currentPosition + m_ChargeDirection.normalized * m_ChargeDistance;

        //Set new destination
        m_Agent.SetDestination(m_ChargeToPosition);

        return target.transform.position;
    }
}
