using UnityEngine;
using System.Collections;

public class Furbull : BaseEnemy 
{
    private const float FURBULL_ATTACK_DELAY = 2.0f;
    EnemyProjectile m_EnemyProjectile;

	// Use this for initialization
	void Start () 
    {
        m_AttackTimer = FURBULL_ATTACK_DELAY;
		m_CombatRange = 1.5f;

		base.Start ();
	}
	
	// Update is called once per frame
    void Update () 
    {
		UpdateState ();
	}

    protected override void Fight()
    {
        if (m_Target != null)
        {
            if (m_AttackTimer <= 0.0f)
            {
                //Projectile Needed
                //Flag Damage to Player
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
}
