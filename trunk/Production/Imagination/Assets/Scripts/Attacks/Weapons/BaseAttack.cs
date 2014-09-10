using UnityEngine;
using System.Collections;
/// <summary>
/// Base attack.
/// Created by Zach Dubuc
/// </summary>
public class BaseAttack : MonoBehaviour 
{
	GameObject m_Projectile;

	float m_AttackTimer; //The animation Timer (Hopefully)
	float m_GraceTimer = 1.0f; //How long the player will have to combo the attack, starts .5 seconds before m_AttackTime ends

	bool m_AnimationDone; //Bool for if the animation is done or not


	protected float m_SaveAttackTimer;
	protected float m_SaveGraceTimer;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void startAttack()
	{
		//Reset Timers
		m_AttackTimer = m_SaveAttackTimer;
		m_GraceTimer = m_SaveGraceTimer;

		//Start Animation
		//TODO Animation

		createProjectile ();
	}

	public virtual void createProjectile()
	{
		Instantiate (m_Projectile, this.transform.position, this.transform.rotation);
	}
}
