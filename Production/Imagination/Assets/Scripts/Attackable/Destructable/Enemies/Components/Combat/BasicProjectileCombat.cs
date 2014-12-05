/*
 * Created by Mathieu Elias November 17/2014
 * 
 * This Combat component just launches a projectile from the enemies position in the direction he is facing
 * 
 */

#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class BasicProjectileCombat : BaseCombat 
{
	public override void Combat (GameObject target)
	{
		//If our prefab is not null then instantiate it at the enemy's position and rotation.
		if (m_ProjectilePrefab != null)
			Instantiate ((Object)m_ProjectilePrefab, transform.position, transform.rotation);
	}
}
