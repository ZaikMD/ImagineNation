using UnityEngine;
using System.Collections;

public abstract class BaseCombat : BaseComponent
{
	protected GameObject m_ProjectilePrefab;
    public override void start(BaseBehaviour baseBehaviour)
    {
		m_ProjectilePrefab = baseBehaviour.getProjectilePrefab ();
    }

	public abstract void Combat();
}
