using UnityEngine;
using System.Collections;
/// <summary>
/// Special attack.
/// 
/// created by Zach Dubuc
/// 
/// The special attack for players, shoots in 360 radius
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented- Zach Dubuc
*
* 
*/
public class SpecialAttack : BaseAttack
{

	float m_NumberOfProjectiles = 8;
	float m_Angle = 45;  //360/number of projectiles

	Quaternion m_Rotation;


	public override void createProjectile ()
	{

		for(int i = 0; i < m_NumberOfProjectiles; i++)
		{
			m_Rotation = Quaternion.Euler(m_InitialRotation.x, m_InitialRotation.y + (i * m_Angle), m_InitialRotation.z);
			Instantiate (m_Projectile, m_InitialPosition, m_Rotation);
		}


	}
}
