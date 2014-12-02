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
