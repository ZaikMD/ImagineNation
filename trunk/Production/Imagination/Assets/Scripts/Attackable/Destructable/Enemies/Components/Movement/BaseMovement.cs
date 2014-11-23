using UnityEngine;
using System.Collections;

public abstract class BaseMovement : BaseComponent
{
	protected NavMeshAgent m_Agent;

    public override void start(BaseBehaviour baseBehaviour)
    {
		m_Agent = baseBehaviour.GetAgent ();
    }

	public virtual void Movement()
	{

	}

	public virtual void Movement(Transform target)
	{

	}

	public virtual void Movement(Vector3 moveLocation)
	{
		m_Agent.SetDestination (moveLocation);
	}
}
