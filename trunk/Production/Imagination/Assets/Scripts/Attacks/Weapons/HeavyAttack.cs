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

	void Start()
	{
		m_Damage = 1.0f;
		m_Projectile = new HeavyProjectile ();
		m_AttackTimer = 0.8f;
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.8f;
		m_SaveGraceTimer = m_GraceTimer;
	}
	
	public override void createProjectile ()
	{
		HeavyProjectile proj =  (HeavyProjectile)GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), m_InitialRotation);
		
		proj.setDamage (m_Damage);
	}
}
