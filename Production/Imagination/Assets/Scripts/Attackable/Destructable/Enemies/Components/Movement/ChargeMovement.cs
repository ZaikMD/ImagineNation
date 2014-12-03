/*
 * Created by Joe Burchill December 2/2014
 * Movement created for an enemy that charges at the player
 * set speed at which it charges
 */

#region ChangeLog
/*
 */
#endregion

using UnityEngine;
using System.Collections;

public class ChargeMovement : BaseMovement 
{
    public float ChargeSpeed;
    private float m_NormalSpeed;
    private float m_DistanceFromChargeToPosition;

    public override Vector3 Movement(GameObject target)
    {
        m_NormalSpeed = m_Agent.speed;

        m_Agent.speed = ChargeSpeed;

        m_DistanceFromChargeToPosition = Vector3.Distance(target.transform.position, transform.position);

        if(m_DistanceFromChargeToPosition < 3.0f)
        {
            m_Agent.speed = m_NormalSpeed;
        }

        m_Agent.SetDestination(target.transform.position);

        return target.transform.position;
    }
}
