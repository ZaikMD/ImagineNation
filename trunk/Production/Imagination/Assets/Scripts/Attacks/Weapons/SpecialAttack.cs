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
#endregion
public class SpecialAttack : BaseAttack
{

	float m_NumberOfProjectiles = 8;
	float m_Angle = 45;  //360/number of projectiles


	Quaternion m_Rotation;
	void Start()
	{
		m_Damage = 0.5f;
		m_AttackTimer = 0.4f;
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.5f;
		m_SaveGraceTimer = m_GraceTimer;
	}

	public override void createProjectile ()
	{

		for(int i = 0; i < m_NumberOfProjectiles; i++)
		{
			m_Rotation = Quaternion.Euler(m_InitialRotation.x, m_InitialRotation.y + (i * m_Angle), m_InitialRotation.z);
			GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), m_Rotation);
		}


	}
}
