/*
 * Created by Joe Burchill & Mathieu Elias November 17/2014
 * 
 * This class is the base class for combat components
 * The combat components decide are used when the enemy attacks the player
 * 
 */

#region ChangeLog
/*
 * Added GameObject as a argument to combat function. Dec 4 - Mathieu Elias
 */
#endregion
using UnityEngine;
using System.Collections;

public abstract class BaseCombat : BaseComponent
{
	//Projectile Prefab to pass through combat
	protected GameObject m_ProjectilePrefab;
    public override void start(BaseBehaviour baseBehaviour)
    {
		//Get our base behaviour projectile
		m_ProjectilePrefab = baseBehaviour.getProjectilePrefab ();
    }

	//Function in order to set the projectile prefab
	public void SetProjectilePrefab(GameObject prefab)
	{
		m_ProjectilePrefab = prefab;
	}

	// All combat components must use this function to attack
	public abstract void Combat(GameObject target);
}
