using UnityEngine;
using System.Collections;

public abstract class BaseMovement : BaseComponent
{
	protected NavMeshAgent m_Agent;

    public override void start(BaseBehaviour baseBehaviour)
    {
		m_Agent = baseBehaviour.GetAgent ();
    }

	public abstract void Movement();

	public virtual void Movement(Vector3 targetLocation)
	{
		m_Agent.SetDestination (targetLocation);
	}
}
