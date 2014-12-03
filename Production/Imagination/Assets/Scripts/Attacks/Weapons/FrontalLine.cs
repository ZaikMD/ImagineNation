using UnityEngine;
using System.Collections;
/// <summary>
///  A frontal line attack that extends a ways past the player
/// 
/// Created by Zach Dubuc
/// 
/// ChangeLog:
/// 
/// 1/12/14: Edit: Fully commented - Zach Dubuc
/// </summary>
public class FrontalLine : BaseAttack 
{
	float m_Range = 10.0f;//The range of the projectile

	public FrontalLine()
	{
		m_Damage = 0.5f;//Damage
		m_AttackTimer = 0.8f; //Time it takes to attack
		m_SaveAttackTimer = m_AttackTimer;
		m_GraceTimer = 0.4f; //Time players have to attack again
		m_SaveGraceTimer = m_GraceTimer;
		m_AttackMoveSpeed = 0.0f; //Attack movement speed
		m_ForceInput = true;
	}
	public override void createProjectile ()
	{
		GameObject proj = (GameObject)GameObject.Instantiate (m_Projectile,
		                                                      new Vector3(m_InitialPosition.x,
													          m_InitialPosition.y + m_FirePointYOffSet,
													          m_InitialPosition.z),
		                                                      Quaternion.Euler(m_InitialRotation));
		LightProjectile projS = proj.GetComponent (typeof(LightProjectile)) as LightProjectile;
		projS.setRange (m_Range); //Set the range, damage and the character that attacked
		projS.setDamage (m_Damage);
		projS.setCharacter (m_Character);
	}
}
