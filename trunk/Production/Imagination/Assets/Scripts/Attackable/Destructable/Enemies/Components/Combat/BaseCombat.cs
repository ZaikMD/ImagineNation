/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * This class is the base class for combat components
 * The combat components decide are used when the enemy attacks the player
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
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

	// All combat components must use this function to attack
	public abstract void Combat();
}
