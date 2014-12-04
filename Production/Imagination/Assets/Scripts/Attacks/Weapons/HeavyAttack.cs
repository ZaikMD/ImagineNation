using UnityEngine;
using System.Collections;
/// <summary>
/// Heavy attack.
/// 
/// created by Zach Dubuc
/// 
/// The Heavy attack for players, does 1 damage
/// 
/// CHANGELOG:
/// 
/// 1/12/14 Edit: Fully Commented - Zach Dubuc
/// </summary>
public class HeavyAttack : BaseAttack
{

	public  HeavyAttack()
	{
		m_Damage = 1.0f; // Damage
		m_AttackTimer = 0.5f; //Time it takes to attack
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.5f; //Time players have to attack again
		m_SaveGraceTimer = m_GraceTimer;
		m_AttackMoveSpeed = 0.2f; //Attack movement speed
		m_ForceInput = true;
	}
	
	public override void createProjectile ()
	{
		GameObject proj = (GameObject)GameObject.Instantiate (m_Projectile,
		                                                      new Vector3(m_InitialPosition.x,
												            	m_InitialPosition.y + m_FirePointYOffSet,
												            	m_InitialPosition.z),
		                                                      	Quaternion.Euler(m_InitialRotation));
		//Get a reference of the projectile and set the damage and character that's attacking
		HeavyProjectile projS = proj.GetComponent (typeof(HeavyProjectile)) as HeavyProjectile;
		projS.setDamage (m_Damage);
		projS.setCharacter (m_Character);
	}

	public virtual float getAttackMoveSpeed()
	{
		//Speed is cubed using Power function
		float speed = m_AttackMoveSpeed * Mathf.Pow(m_SaveAttackTimer / m_AttackTimer, 3.0f);
		return speed;
	}
}
