using UnityEngine;
using System.Collections;

public class PlayerHealth : Destructable 
{
    //the textures used for the health bar
	public Texture[] textures;

    //the gui texture used to display the textures
	public GUITexture m_GUITexture;

    //how long it takes to regen health and the timer
	public float HealthRegenTime = 5.0f;
	float m_HealthRegenTimer;

    //how long being hit stops your health regen for and its timer
	public float StopHealthRegenTime = 20.0f;
	float m_StopHealthRegenTimer;

    //how long you're invulnerable after being hit
	public float InvulnerabilityTimer = 1.5f;
	float m_InvulnerabilityTimer;

    //used to reset the health
	int m_TotalHealth;

    //the character the script is running on (used to determin which players health is being kept track of)
	Characters m_CurrentCharacter;

    //used to stop the script from executing and used so other scripts can tell the player is dead
	bool m_IsDead = false;
    public bool IsDead
	{
		get{return m_IsDead;}
		private set{}
	}

	// Use this for initialization
	void Start () 
	{
        //setting initial values for the timers
		m_HealthRegenTimer = HealthRegenTime;
		m_StopHealthRegenTimer = 0.0f;
		m_InvulnerabilityTimer = InvulnerabilityTimer;

        //setting the total health
		m_TotalHealth = m_Health;

        //setting the current texture
		m_GUITexture.texture = textures [m_Health];

        //figureing out what player the script is executing on
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
		
        if(!m_IsDead)
		{
			if (m_Health <= 0)
			{
				onDeath();//you died
			}
			else
			{
				if(m_StopHealthRegenTimer > 0)
				{
					m_StopHealthRegenTimer -= Time.deltaTime;
				}

				if(m_Health < m_TotalHealth)
				{
                    //need to regen health
					
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
	}

	public override void onHit(PlayerProjectile proj)
	{        
		return;
	}
	
	public override void onHit(EnemyProjectile proj)
	{
		if(m_InvulnerabilityTimer <= 0)
		{
            //not invulnerable so take damage
			m_Health -= 1;  
			m_StopHealthRegenTimer = StopHealthRegenTime;
			m_InvulnerabilityTimer = InvulnerabilityTimer;
            //update health bar
			m_GUITexture.texture = textures[m_Health];
		}
	}

	void updateHealthBarPosition()
	{
        //place the healthbar in a corner
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
        if (!m_IsDead)
        {
            //not dead already so die
            m_IsDead = true;
            Instantiate(m_Ragdoll, transform.position, transform.rotation);
            //TODO: Hide this object
            //TODO: tell Dead player Manager that this player is dead
        }
	}

	public override void instantKill ()
	{
		base.instantKill ();
        //update the health
		m_GUITexture.texture = textures [m_Health];
	}

	public void resetHealth()
	{
        //reset everything
		m_IsDead = false;
		m_Health = m_TotalHealth;
		InvulnerabilityTimer = m_InvulnerabilityTimer;
		StopHealthRegenTime = 0.0f;
		m_GUITexture.texture = textures[m_Health];
	}
}
