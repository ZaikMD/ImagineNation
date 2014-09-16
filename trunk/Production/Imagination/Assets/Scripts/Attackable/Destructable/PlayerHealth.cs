using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]

public class PlayerHealth : Destructable 
{
	public Texture[] textures;

	public float HealthRegenTime = 5.0f;
	float m_HealthRegenTimer;

	public float StopHealthRegenTime = 20.0f;
	float m_StopHealthRegenTimer;

	int m_TotalHealth;

	// Use this for initialization
	void Start () 
	{
		m_HealthRegenTimer = HealthRegenTime;
		m_StopHealthRegenTimer = StopHealthRegenTime;
		m_TotalHealth = m_Health;
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		if (m_Health <= 0)
		{
			Instantiate(m_Ragdoll, transform.position, transform.rotation);
			//TODO: Hide this object
			//TODO: tell Dead player Manager that this player is dead
		}
		else
		{
			if(m_Health < m_TotalHealth)
			{
				if(m_StopHealthRegenTimer < 0)
				{
					if(m_HealthRegenTimer < 0)
					{
						m_Health++;
						m_HealthRegenTimer = HealthRegenTime;
					}
					else
					{
						m_HealthRegenTimer-= Time.deltaTime;
					}
				}
			}

			if(m_StopHealthRegenTimer > 0)
			{
				m_StopHealthRegenTimer -= Time.deltaTime;
			}
		}
	}

	public virtual void onHit(PlayerProjectile proj)
	{        
		return;
	}
	
	public virtual void onHit(EnemyProjectile proj)
	{
		m_Health -= 1;  
		m_StopHealthRegenTimer = StopHealthRegenTime;
	}
}
