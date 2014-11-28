using UnityEngine;
using System.Collections;
/// <summary>
/// Heavy attack.
/// 
/// created by Zach Dubuc
/// 
/// The Heavy attack for players, does 1 damage
/// </summary>
public class HeavyAttack : BaseAttack
{

	public  HeavyAttack()
	{
		m_Damage = 1.0f;
		m_AttackTimer = 0.8f;
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.5f;
		m_SaveGraceTimer = m_GraceTimer;
	}
	
	public override void createProjectile ()
	{
		GameObject proj = (GameObject)GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), Quaternion.Euler(m_InitialRotation));
		HeavyProjectile projS = proj.GetComponent (typeof(HeavyProjectile)) as HeavyProjectile;
		projS.setDamage (m_Damage);
	}
}
