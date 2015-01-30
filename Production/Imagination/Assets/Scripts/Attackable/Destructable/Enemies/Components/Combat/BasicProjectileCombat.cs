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
		{
			GameObject projectile = (GameObject) Instantiate ((Object)m_ProjectilePrefab, transform.position, transform.rotation);

			Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
			Vector3 myPos = new Vector3(transform.position.x, 0, transform.position.z);

			projectile.transform.forward = targetPos - myPos;
		}
	}
}
