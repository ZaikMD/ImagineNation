using UnityEngine;
using System.Collections;
/// <summary>
/// Base attack.
/// Created by Zach Dubuc
/// </summary>
public class BaseAttack : MonoBehaviour 
{
	protected GameObject m_Projectile;

	float m_AttackTimer = 2.0f; //The animation Timer (Hopefully)
	float m_GraceTimer = 1.0f; //How long the player will have to combo the attack, starts .5 seconds before m_AttackTime ends

	bool m_AnimationDone; //Bool for if the animation is done or not
	bool m_Attacking;
	bool m_GraceCountdown;


	protected float m_SaveAttackTimer = 2.0f;
	protected float m_SaveGraceTimer = 1.0f;

	protected Vector3 m_InitialPosition;
	protected Quaternion m_InitialRotation;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	public void Update ()
	{
		if(m_Attacking)
		{
			m_AttackTimer -= Time.deltaTime;
		}

		if(m_AttackTimer <= 0.5f)
		{
			m_GraceCountdown = true;
		}
		if(m_AttackTimer < 0.0f)
		{
			m_Attacking = false;
		}

		if(m_GraceCountdown)
		{
			m_GraceTimer -= Time.deltaTime;
		}

		if(m_GraceTimer <= 0.0f)
		{
			m_GraceCountdown = false;
		}
	}

	public void loadPrefab( GameObject prefab)
	{
		m_Projectile = prefab;
	}

	public void startAttack(Vector3 pos, Quaternion rotation)
	{
		//Reset Timers
		m_AttackTimer = m_SaveAttackTimer;
		m_GraceTimer = m_SaveGraceTimer;


		m_InitialPosition = pos;
		m_InitialRotation = rotation;
		//Start Animation
		//TODO Animation

		createProjectile ();
		m_Attacking = true;
	}

	public float getAttackTimer()
	{
		return m_AttackTimer;
	}

	public float getGraceTimer()
	{
		return m_GraceTimer;
	}

	public bool getAttacking()
	{
		return m_Attacking;
	}

	public virtual void createProjectile()
	{

		Instantiate (m_Projectile,m_InitialPosition, m_InitialRotation);
	}
}
