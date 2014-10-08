using UnityEngine;
using System.Collections;

/*
 * Created by Joe Burchill
 * Date: Sept, 26, 2014
 *  
 * This script handles the functionality of the Furbull,
 * a specific enemy that is very basic, it has a simple
 * headbutt attack with a short delay.
 * 
 */
#region ChangeLog
/* 
 * 
 */
#endregion

public class Furbull : BaseEnemy 
{
    //Enemy Projectile for the Furbull to attack the Player
    public EnemyProjectile EnemyProjectile;
    //Const for Attack Delay, so enemy doesn't constantly hit player
    private const float FURBULL_ATTACK_DELAY = 1.5f;
    
	// Use this for initialization
	void Start () 
    {
		m_Health = 1;

        //Set attack timer from base
        m_AttackTimer = 0.0f;
        //Set Combat Range from base
		m_CombatRange = 1.5f;

        //Call base class Start()
		base.Start ();
	}
	
	// Update is called once per frame
    void Update () 
    {
        //Call base class Update()
        base.Update();
	}

    protected override void Fight()
    {
        //Set inital stopping distance
        m_Agent.stoppingDistance = m_InitialStoppingDistance;

        //Check our target
        if (m_Target != null)
        {
            //If our target isn't null we check our attack timer
            if (m_AttackTimer <= 0.0f)
            {
                //Attack player then reset our timer
                ShootProjectile();
                m_AttackTimer = FURBULL_ATTACK_DELAY;
            }
            else
            {
                //Decrement timer until delay is over
                m_AttackTimer -= Time.deltaTime;
            }
            //Return to default
			m_State = State.Default;
        }
    }

    protected override void Die()
    {
        //Set base boolean to false
        m_IsAlive = false;
    }

    private void ShootProjectile()
    {
        //Instantiate our projectile from enemy position and rotation
        Instantiate(EnemyProjectile.gameObject, transform.position, transform.rotation);
    }
}
