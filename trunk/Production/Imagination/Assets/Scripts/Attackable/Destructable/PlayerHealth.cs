using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]

public class PlayerHealth : Destructable 
{
	public Texture[] textures;

	GUITexture m_GUITexture;

	public float HealthRegenTime = 5.0f;
	float m_HealthRegenTimer;

	public float StopHealthRegenTime = 20.0f;
	float m_StopHealthRegenTimer;

	public float InvulnerabilityTimer = 1.5f;
	float m_InvulnerabilityTimer;

	int m_TotalHealth;

	// Use this for initialization
	void Start () 
	{
		m_HealthRegenTimer = HealthRegenTime;
		m_StopHealthRegenTimer = StopHealthRegenTime;
		m_InvulnerabilityTimer = InvulnerabilityTimer;
		m_TotalHealth = m_Health;

		m_GUITexture = gameObject.GetComponent<GUITexture> ();
		m_GUITexture.texture = textures [m_Health - 1];
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		if(m_InvulnerabilityTimer > 0)
		{
			m_InvulnerabilityTimer -= Time.deltaTime;
		}

		if (m_Health <= 0)
		{
			Instantiate(m_Ragdoll, transform.position, transform.rotation);
			//TODO: Hide this object
			//TODO: tell Dead player Manager that this player is dead
		}
		else
		{
			if(m_StopHealthRegenTimer > 0)
			{
				m_StopHealthRegenTimer -= Time.deltaTime;
			}

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
		}
	}

	public virtual void onHit(PlayerProjectile proj)
	{        
		return;
	}
	
	public virtual void onHit(EnemyProjectile proj)
	{
		if(m_InvulnerabilityTimer < 0)
		{
			m_Health -= 1;  
			m_StopHealthRegenTimer = StopHealthRegenTime;
			m_InvulnerabilityTimer = InvulnerabilityTimer;
		}
	}
}
