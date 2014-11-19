using UnityEngine;
using System.Collections;
/// <summary>
/// Heavy projectile.
/// 
/// Inherits from BaseProjectile
/// 
/// Makes it typeof HeavyProjectile
/// </summary>

#region ChangeLog
/*
* Created on 19/11/14
*
* 
*/
#endregion

public class HeavyProjectile : BaseProjectile 
{
	
	float m_Damage;
	
	public void setDamage(float damage)
	{
		m_Damage = damage;
	}
	
	void OnTriggerEnter( Collider obj)
	{
		if (obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
		{
			Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
			
			attackable.onHit(this, m_Damage);
			if(obj.gameObject.tag != Constants.PLAYER_STRING)
			{
				Destroy(this.gameObject);
			}
		} 
	}
}