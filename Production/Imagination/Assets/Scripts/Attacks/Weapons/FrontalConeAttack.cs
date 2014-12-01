using UnityEngine;
using System.Collections;

public class FrontalConeAttack : BaseAttack 
{

	float m_NumberOfProjectiles = 6;
	float m_Angle = 15;  
	
	
	Vector3 m_Rotation;
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
		//offet this to be at one end of the cone
		m_Rotation = m_InitialRotation;
		m_Rotation.y += 45;

		for(int i = 0; i < m_NumberOfProjectiles; i++)
		{
			LightProjectile proj = (LightProjectile)GameObject.Instantiate (m_Projectile,
			                        new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z),
			                        Quaternion.Euler(m_Rotation));
			m_Rotation.y -= (m_Angle);
			proj.setDamage (m_Damage);
			proj.setCharacter (m_Character);
		}		
	}
}
