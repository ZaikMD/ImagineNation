using UnityEngine;
using System.Collections;

public class SpecialAttack : BaseAttack
{

	float m_NumberOfProjectiles = 8;
	float m_Angle = 45;  //360/number of projectiles

	Quaternion m_Rotation;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public override void createProjectile ()
	{

		for(int i = 0; i < m_NumberOfProjectiles; i++)
		{
			m_Rotation = Quaternion.Euler(m_InitialRotation.x, m_InitialRotation.y + (i * m_Angle), m_InitialRotation.z);
			Instantiate (m_Projectile, m_InitialPosition, m_Rotation);
		}


	}
}
