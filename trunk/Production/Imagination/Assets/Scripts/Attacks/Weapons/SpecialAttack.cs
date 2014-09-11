using UnityEngine;
using System.Collections;

public class SpecialAttack : BaseAttack
{

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
		Instantiate (m_Projectile, m_InitialPosition, transform.rotation);
	}
}
