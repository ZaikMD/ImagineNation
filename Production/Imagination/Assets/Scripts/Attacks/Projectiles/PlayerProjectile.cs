using UnityEngine;
using System.Collections;
/// <summary>
/// Player projectile.
/// 
/// Inherits from BaseProjectile
/// 
/// Makes it typeof PlayerProjectile
/// </summary>
public class PlayerProjectile : BaseProjectile 
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
