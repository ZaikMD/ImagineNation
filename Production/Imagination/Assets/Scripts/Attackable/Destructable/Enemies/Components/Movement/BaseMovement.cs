using UnityEngine;
using System.Collections;

public abstract class BaseMovement : BaseComponent
{
	protected NavMeshAgent m_Agent;

    public override void start(BaseBehaviour baseBehaviour)
    {
		m_Agent = baseBehaviour.GetAgent ();
    }

	public abstract Vector3 Movement (GameObject target);


	public virtual void Movement(Vector3 moveLocation)
	{
		m_Agent.SetDestination (moveLocation);
	}

	public void SetAgent(NavMeshAgent navAgent)
	{
		m_Agent = navAgent;
	}

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
