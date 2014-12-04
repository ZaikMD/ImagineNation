using UnityEngine;
using System.Collections;
/// <summary>
/// Light attack.
/// 
/// created by Zach Dubuc
/// 
/// The light attack for players, does 0.5 damage
/// 
/// ChangeLog:
/// 
/// 1/12/14: Edit- Fully commented- Zach Dubuc
/// </summary>
public class LightAttack : BaseAttack
{
	public LightAttack()
	{
		m_Damage = 0.5f;//Damage
		m_AttackTimer = 0.2f; //Time it takes to attack
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.4f; //Time players have to attack again
		m_SaveGraceTimer = m_GraceTimer;
		m_AttackMoveSpeed = 0.2f; //Attack movement speed
		m_ForceInput = true;
	}

	public override void createProjectile ()
	{
		GameObject proj =  (GameObject)GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), Quaternion.Euler(m_InitialRotation));

		//Get a reference for the projectile and set the damage and which character attacked
		LightProjectile projS = proj.GetComponent (typeof(LightProjectile)) as LightProjectile;
	
		projS.setDamage (m_Damage);
		projS.setCharacter (m_Character);
	}

	public virtual float getAttackMoveSpeed()
	{
		//Speed is cubed using Power function
		float speed = m_AttackMoveSpeed * Mathf.Pow(m_AttackTimer / m_SaveAttackTimer, 3.0f);
		return speed;
	}
}
