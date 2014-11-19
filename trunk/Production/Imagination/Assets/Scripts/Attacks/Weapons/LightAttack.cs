using UnityEngine;
using System.Collections;
/// <summary>
/// Light attack.
/// 
/// created by Zach Dubuc
/// 
/// The light attack for players, does 0.5 damage
/// </summary>
public class LightAttack : BaseAttack
{
	void Start()
	{
		m_Damage = 0.5f;
		m_Projectile = new LightProjectile ();
		m_AttackTimer = 0.4f;
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.5f;
		m_SaveGraceTimer = m_GraceTimer;
	}

	public override void createProjectile ()
	{
		LightProjectile proj =  (LightProjectile)GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), m_InitialRotation);

		proj.setDamage (m_Damage);
	}
}
