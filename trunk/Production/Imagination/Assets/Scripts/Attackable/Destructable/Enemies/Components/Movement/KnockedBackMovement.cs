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
    private Vector3 m_Direction;

    public override Vector3 Movement(GameObject target)
    {
        Vector3 currentPosition = transform.position;
        Vector3 destinationPosition = target.transform.position;

        //Get the direction vector between then and zero out the y axis
        m_Direction = destinationPosition - currentPosition;
        m_Direction.y = 0.0f;

        transform.position -= m_Direction.normalized * Time.deltaTime * Constants.KNOCKBACK_MULTIPLIER;

        return transform.position;
    }
}
