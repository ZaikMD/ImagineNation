using UnityEngine;
using System.Collections;
/// <summary>
///  A frontal cone attack
/// 
/// Created by Zach Dubuc
/// 
/// ChangeLog:
/// 
/// 1/12/14: Edit: Fully commented - Zach Dubuc
/// </summary>
public class FrontalConeAttack : BaseAttack 
{

	float m_NumberOfProjectiles = 6; //The number of projectiles to spawn
	float m_Angle = 45;  //The angle between the projectiles
	Vector3 m_Rotation; //The rotation for the projectiles

	void Start()
	{
		m_Damage = 1.0f; //Set the damage
		m_AttackTimer = 0.4f; //Set the timer
		m_SaveAttackTimer = m_AttackTimer; //Set the reference for the Attack Timer
		m_GraceTimer = 0.5f; //Set the grace period timer
		m_SaveGraceTimer = m_GraceTimer; //Set the reference for it
		m_AttackMoveSpeed = 0.5f; //Attack movement speed
		m_ForceInput = true;
	}
	
	public override void createProjectile ()
	{
		//offet this to be at one end of the cone
		m_Rotation = m_InitialRotation; //Set the rotation
		m_Rotation.y += m_Angle; //Add the angle to the rotations y

		for(int i = 0; i < m_NumberOfProjectiles; i++)
		{
			//Create projectiles 
			GameObject proj =  (GameObject)GameObject.Instantiate (m_Projectile,
			                        							   new Vector3(m_InitialPosition.x, 
														           m_InitialPosition.y + m_FirePointYOffSet, 
														           m_InitialPosition.z),
			                        							   Quaternion.Euler(m_Rotation));
			//Reference for the projectile
			HeavyProjectile projS = proj.GetComponent (typeof(HeavyProjectile)) as HeavyProjectile;
			projS.setDamage (m_Damage); //Set the projectiles damage
			projS.setCharacter (m_Character);//Set the character that attacked

			m_Rotation.y -= (m_Angle); //Change the angle
		}		
	}

	public virtual float getAttackMoveSpeed()
	{
		float speed = m_AttackMoveSpeed * Mathf.Pow(m_SaveAttackTimer / m_AttackTimer, 3.0f);
		return speed;
	}
}
