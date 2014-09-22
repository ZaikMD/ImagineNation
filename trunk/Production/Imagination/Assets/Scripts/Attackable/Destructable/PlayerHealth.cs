using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]

public class PlayerHealth : Destructable 
{
	public Texture[] textures;

	public GUITexture m_GUITexture;

	public float HealthRegenTime = 5.0f;
	float m_HealthRegenTimer;

	public float StopHealthRegenTime = 20.0f;
	float m_StopHealthRegenTimer;

	public float InvulnerabilityTimer = 1.5f;
	float m_InvulnerabilityTimer;

	int m_TotalHealth;

	Characters m_CurrentCharacter;

	bool m_IsDead = false;
/*
	bool IsDead
	{
		public get{return m_IsDead;}
		set{}
	}
*/
	// Use this for initialization
	void Start () 
	{
		m_HealthRegenTimer = HealthRegenTime;
		m_StopHealthRegenTimer = 0.0f;
		m_InvulnerabilityTimer = InvulnerabilityTimer;
		m_TotalHealth = m_Health;

		m_GUITexture.texture = textures [m_Health];

		switch(gameObject.transform.parent.name)
		{
		case "Alex":
			m_CurrentCharacter = Characters.Alex;
			break;
		case "Derek":
			m_CurrentCharacter = Characters.Derek;
			break;
		case "Zoe":
			m_CurrentCharacter = Characters.Zoey;
			break;
		default:
			Debug.LogError("health is set up wrong");
			break;
		}
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		updateHealthBarPosition ();
		if(m_InvulnerabilityTimer > 0)
		{
			m_InvulnerabilityTimer -= Time.deltaTime;
		}

		if (false)//(m_Health <= 0)
		{
			onDeath();
		}
		else
		{
			if(m_StopHealthRegenTimer > 0)
			{
				m_StopHealthRegenTimer -= Time.deltaTime;
			}

			if(m_Health < m_TotalHealth)
			{
				if(m_StopHealthRegenTimer <= 0)
				{
					if(m_HealthRegenTimer <= 0)
					{
						m_Health++;
						m_HealthRegenTimer = HealthRegenTime;
						m_GUITexture.texture = textures[m_Health];
					}
					else
					{
						m_HealthRegenTimer-= Time.deltaTime;
					}
				}
			}
		}
	}

	public override void onHit(PlayerProjectile proj)
	{        
		return;
	}
	
	public override void onHit(EnemyProjectile proj)
	{
		if(m_InvulnerabilityTimer <= 0)
		{
			m_Health -= 1;  
			m_StopHealthRegenTimer = StopHealthRegenTime;
			m_InvulnerabilityTimer = InvulnerabilityTimer;
			m_GUITexture.texture = textures[m_Health];
		}
	}

	void updateHealthBarPosition()
	{

		if(GameData.Instance.PlayerOneCharacter == m_CurrentCharacter)
		{
			m_GUITexture.pixelInset = new Rect (0.0f, Screen.height - textures [m_Health].height, textures [m_Health].width, textures [m_Health].height);
		}
		else if( GameData.Instance.PlayerTwoCharacter == m_CurrentCharacter)
		{
			m_GUITexture.pixelInset = new Rect (Screen.width - textures [m_Health].width, Screen.height - textures [m_Health].height, textures [m_Health].width, textures [m_Health].height);
		}
		else
		{
			Debug.LogError("This is broken the set character shouldnt be in the scene");
		}
	}

	protected override void onDeath ()
	{
		m_IsDead = true;
		Instantiate(m_Ragdoll, transform.position, transform.rotation);
		//TODO: Hide this object
		//TODO: tell Dead player Manager that this player is dead
	}

	public override void instantKill ()
	{
		base.instantKill ();
		m_GUITexture.texture = textures [m_Health];
		m_StopHealthRegenTimer = float.MaxValue;
	}

	public void resetHealth()
	{
		bool m_IsDead = false;
		m_Health = m_TotalHealth;
		InvulnerabilityTimer = m_InvulnerabilityTimer;
		StopHealthRegenTime = 0.0f;
		m_GUITexture.texture = textures[m_Health];
	}
}
