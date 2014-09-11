using UnityEngine;
using System.Collections;
/// <summary>
/// Base attack.
/// Created by Zach Dubuc
/// </summary>
public class BaseAttack : MonoBehaviour 
{
	protected GameObject m_Projectile;

	float m_AttackTimer; //The animation Timer (Hopefully)
	float m_GraceTimer = 1.0f; //How long the player will have to combo the attack, starts .5 seconds before m_AttackTime ends

	bool m_AnimationDone; //Bool for if the animation is done or not
	bool m_Attacking;


	protected float m_SaveAttackTimer;
	protected float m_SaveGraceTimer;

	protected Vector3 m_InitialPosition;
	protected Quaternion m_initialRotation;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_Attacking)
		{
			m_AttackTimer -= Time.deltaTime;
		}

		if(m_AttackTimer <= 0.5f)
		{
			m_GraceTimer -= Time.deltaTime;
		}
	}

	public void loadPrefab()
	{
		m_Projectile = (GameObject)Resources.Load("Prefab_PlayerProjectile", typeof (GameObject));
	}

	public void startAttack(Vector3 pos, Quaternion rotation)
	{


		//Reset Timers
		m_AttackTimer = m_SaveAttackTimer;
		m_GraceTimer = m_SaveGraceTimer;

		m_InitialPosition = pos;
		m_initialRotation = rotation;
		//Start Animation
		//TODO Animation

		createProjectile ();
	}

	public float getAttackTimer()
	{
		return m_AttackTimer;
	}

	public float getGraceTimer()
	{
		return m_GraceTimer;
	}

	public virtual void createProjectile()
	{
		Instantiate (m_Projectile,m_InitialPosition, transform.rotation);
	}
}
