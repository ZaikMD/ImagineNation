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

	//Overriden Movement
    public override Vector3 Movement(GameObject target)
    {
		//Set Normal speed variable to our current agent speed
        m_NormalSpeed = m_Agent.speed;

		//Set agent speed to charge speed, set in the inspector
        m_Agent.speed = ChargeSpeed;

		//Set Distance from target
        m_DistanceFromChargeToPosition = Vector3.Distance(target.transform.position, transform.position);

		//Check Distance, set speed if reached
        if(m_DistanceFromChargeToPosition < 3.0f)
        {
            m_Agent.speed = m_NormalSpeed;
        }

		//Set Destination
        m_Agent.SetDestination(target.transform.position);

        return target.transform.position;
    }
}
