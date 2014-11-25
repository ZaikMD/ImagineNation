using UnityEngine;
using System.Collections;

public abstract class BaseTargeting : BaseComponent
{
	protected Perception m_Perception;

    public override void start(BaseBehaviour baseBehaviour)
    {
		m_Perception = GetComponentInParent<Perception> ();
    }

    public abstract GameObject CurrentTarget();
}
