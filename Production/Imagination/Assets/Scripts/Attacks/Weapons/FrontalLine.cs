using UnityEngine;
using System.Collections;

public class FrontalLine : BaseAttack 
{
	float m_Range = 10.0f;
	public override void createProjectile ()
	{
		GameObject proj = (GameObject)GameObject.Instantiate (m_Projectile,new Vector3(m_InitialPosition.x, m_InitialPosition.y + m_FirePointYOffSet, m_InitialPosition.z), Quaternion.Euler(m_InitialRotation));
		LightProjectile projS = proj.GetComponent (typeof(LightProjectile)) as LightProjectile;
		projS.setRange (m_Range);
		projS.setDamage (m_Damage);
		projS.setCharacter (m_Character);
	}
}
