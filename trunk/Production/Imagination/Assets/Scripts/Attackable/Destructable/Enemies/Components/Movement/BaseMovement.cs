/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * The base class for all movement components
 * The movement components decide how to move the enemy and then move it
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseMovement : BaseComponent
{
	protected NavMeshAgent m_Agent;

    public override void start(BaseBehaviour baseBehaviour)
    {
		m_Agent = baseBehaviour.GetAgent ();
    }

	// Every Movement component must use this movement function to decide how to move the enemy and then move it
	public abstract Vector3 Movement (GameObject target);

	// An override of the movement function incase you just want to 
	// move to a specific location
	public void Movement(Vector3 moveLocation)
	{
		m_Agent.SetDestination (moveLocation);
	}

	// Set the navAgent
	public void SetAgent(NavMeshAgent navAgent)
	{
		m_Agent = navAgent;
	}

	// Returns a location along the rotation of an object
	protected Vector3 RotateAboutOrigin(Vector3 point, Vector3 origin, float angle)
	{
		// Convert the angle to radians
		angle = angle * (Mathf.PI / 180);
		
		// Find out the new x and z locations 
		float rotatedX = Mathf.Cos (angle) * (point.x - origin.x) - Mathf.Sin (angle) * (point.z - origin.z) + origin.x;
		float rotatedZ = Mathf.Sin (angle) * (point.x - origin.x) - Mathf.Cos (angle) * (point.z - origin.z) + origin.z;
		
		return new Vector3(rotatedX, point.y, rotatedZ);
	} 
}
