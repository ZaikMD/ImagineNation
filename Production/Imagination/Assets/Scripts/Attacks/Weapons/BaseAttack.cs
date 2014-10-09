using UnityEngine;
using System.Collections;
/// <summary>
/// Base attack.
/// Created by Zach Dubuc
/// </summary>
/// 

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented and changed strings to constants- Zach Dubuc
*
* 
*/
#endregion
public class BaseAttack : MonoBehaviour 
{
	protected GameObject m_Projectile;

	float m_AttackTimer = 0.5f; //The animation Timer (Hopefully)
	float m_GraceTimer = 0.8f; //How long the player will have to combo the attack, starts .2 seconds before m_AttackTimer ends

	bool m_AnimationDone; //Bool for if the animation is done or not
	bool m_Attacking = false;
	bool m_GraceCountdown = false;


	protected float m_SaveAttackTimer = 0.5f;
	protected float m_SaveGraceTimer = 0.8f;

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
			m_AttackTimer -= Time.deltaTime; //If the player is attacking, decrement the attack timer
		}

		if(m_AttackTimer <= 0.2f)
		{
			m_GraceCountdown = true; // IF the attack timer is less than 0.2, the grace timer can start decrementing
		}
		if(m_AttackTimer <= 0.0f)
		{
			m_Attacking = false; // If the attack timer is less that zero, the player is done attacking
			//m_AttackTimer = m_SaveAttackTimer;
		}

		if(m_GraceCountdown)
		{
			m_GraceTimer -= Time.deltaTime; //Decrement the grace timer if m_GraceCountdown is true
		}

		if(m_GraceTimer <= 0.0f)
		{
			m_GraceCountdown = false; //If the grace timer is less than zero, then it is no longer decrementing, and it resets
			//m_GraceTimer = m_SaveGraceTimer;

		}

		//Debug.Log (m_GraceTimer);

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

    public void resetGraceTimer()
    {
        m_GraceTimer = m_SaveGraceTimer;
    }

	public bool getAttacking()
	{
		return m_Attacking;
	}

    public bool getGraceCountdown()
    {
        return m_GraceCountdown;
    }

	public virtual void createProjectile()
	{

		Instantiate (m_Projectile,m_InitialPosition, m_InitialRotation);
	}
}
