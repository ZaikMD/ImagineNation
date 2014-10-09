using UnityEngine;
using System.Collections;
/// <summary>
/// Player projectile.
/// 
/// Inherits from BaseProjectile
/// 
/// Makes it typeof PlayerProjectile
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented- Zach Dubuc
*
* 
*/
public class PlayerProjectile : BaseProjectile 
{
	void OnTriggerEnter( Collider obj)
	{
        if (obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
		{
            Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
			
			attackable.onHit(this);
		} 
	}
}
