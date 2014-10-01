using UnityEngine;
using System.Collections;

public class Furbull : BaseEnemy 
{
    public EnemyProjectile EnemyProjectile;
    private const float FURBULL_ATTACK_DELAY = 1.5f;
    
	// Use this for initialization
	void Start () 
    {
        m_AttackTimer = 0.0f;
		m_CombatRange = 1.5f;

		base.Start ();
	}
	
	// Update is called once per frame
    void Update () 
    {
        base.Update();
	}

    protected override void Fight()
    {
        m_Agent.stoppingDistance = m_InitialStoppingDistance;

        if (m_Target != null)
        {
            if (m_AttackTimer <= 0.0f)
            {
                ShootProjectile();
                m_AttackTimer = FURBULL_ATTACK_DELAY;
            }
            else
            {
                m_AttackTimer -= Time.deltaTime;
            }
			m_State = State.Default;
        }
    }

    protected override void Die()
    {
        m_IsAlive = false;
        //Instantiate Ragdoll
        Destroy(this.gameObject);
    }

    private void ShootProjectile()
    {
        Instantiate(EnemyProjectile.gameObject, transform.position, transform.rotation);
    }
}
