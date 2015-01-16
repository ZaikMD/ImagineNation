using UnityEngine;
using System.Collections;
/// <summary>
/// Light projectile.
/// 
/// Inherits from BaseProjectile
/// 
/// Makes it typeof LightProjectile
/// </summary>

#region ChangeLog
/*
* Created on 19/11/14
*
* 
*/
#endregion

public class LightProjectile : BaseProjectile 
{
	public const float KNOCKBACK = 0.8f;
	
	float m_Damage;
	
	public void setDamage(float damage)
	{
		m_Damage = damage;
	}

	public void setRange(float range)
	{
		m_Range = range;
	}
	
	void OnTriggerEnter( Collider obj)
	{
		if (obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
		{
			Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject
			
			attackable.onHit(this, m_Damage);
		} 
	}
}