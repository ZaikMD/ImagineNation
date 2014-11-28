using UnityEngine;
using System.Collections;

public abstract class BaseCombat : BaseComponent
{
	protected GameObject m_ProjectilePrefab;
    public override void start(BaseBehaviour baseBehaviour)
    {
		m_ProjectilePrefab = baseBehaviour.getProjectilePrefab ();
    }

	public void SetProjectilePrefab(GameObject prefab)
	{
		m_ProjectilePrefab = prefab;
	}
	public abstract void Combat();
}
