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
	public LightAttack()
	{
		m_Damage = 0.5f;
		m_AttackTimer = 1.0f;
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.8f;
		m_SaveGraceTimer = m_GraceTimer;
	}

	public override void createProjectile ()
	{
		GameObject proj =  (GameObject)GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), Quaternion.Euler(m_InitialRotation));

		LightProjectile projS = proj.GetComponent (typeof(LightProjectile)) as LightProjectile;
	
		projS.setDamage (m_Damage);
	}
}
