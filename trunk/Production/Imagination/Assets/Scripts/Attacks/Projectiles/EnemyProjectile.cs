using UnityEngine;
using System.Collections;
/// <summary>
/// Enemy projectile.
/// 
/// Inherits from BaseProjectile
/// 
/// Give it the type of EnemyProjectile
/// </summary>
public class EnemyProjectile : BaseProjectile 
{

	void OnTriggerEnter( Collider obj)
	{
		if(obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)
		{
			Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable;
			
			attackable.OnHit(this);
		} 
	}
}
