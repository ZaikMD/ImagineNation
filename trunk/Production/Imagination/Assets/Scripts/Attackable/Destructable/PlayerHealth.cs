/*
*PlayerHealth
*
*resposible for keeping track of the players health and updating the gui
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 24/10/2014 Edit: Took out m_StopRegenHealthTimer and made it so m_HealthRegenTimer resets when the player is hit
* 
* 7/11/2014 Edit: added varible to determine if we are player one or two - Kole 
* 
* 28/11/2014 Edit: Removed anything to do with textures - Greg
* 
* 03/12/2014 Edit: Changed Health to float value, to coincide with damage - Joe Burchill
* 
* 12/1/2014 Edit: Added knockback to the players by getting player movement at start, then calling launch on hit - Jason Hein
* 
*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : Destructable 
{
    //how long it takes to regen health and the timer
	public float HealthRegenTime = 15.0f;
	float m_HealthRegenTimer;

    //how long you're invulnerable after being hit
	public float InvulnerabilityTimer = 1.5f;
	float m_InvulnerabilityTimer;

    //used to reset the health
	float m_TotalHealth;

	//Used to know which player we are
	int m_Player;

	//used to make sound calls
	SFXManager m_SFX;
	Hud m_Hud;

	//Movement for knockback
	BaseMovementAbility m_Movement;
	const float LAUNCH_AMOUNT = 8.0f;
	const float LAUNCH_TIMER = 0.25f;
	const float LAUNCH_UPWARD_DIRECTION = 0.5f;

    //used to stop the script from executing and used so other scripts can tell the player is dead
	bool m_IsDead = false;
    public bool IsDead
	{
		get{return m_IsDead;}
		private set{}
	}

	public TPCamera PlayerCamera;

	//get the players in the scene
	static List<PlayerHealth> m_PlayersList = new List<PlayerHealth>();
	public static List<PlayerHealth> PlayerHealthList
	{
		get { return m_PlayersList; }
	}

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

    SkinnedMeshRenderer m_PlayerRenderer;
    const float MAX_FADE = 0.5f;
    const float FLASH_LENGTH = 0.1875f;


	// Use this for initialization
	void Start () 
	{
		Characters currentCharacter;
		switch (transform.parent.name)
		{
		case Constants.ALEX_STRING:
			currentCharacter = Characters.Alex;
			break;
		case Constants.DEREK_STRING:
			currentCharacter = Characters.Derek;
			break;
		case Constants.ZOE_STRING:
			currentCharacter = Characters.Zoe;
			break;
		default:
#if DEBUG || UNITY_EDITOR
			Debug.LogError("parent is named wrong");
#endif
			currentCharacter = Characters.Zoe;
			break;
		}

		//Check if player one
		if (GameData.Instance.PlayerOneCharacter == currentCharacter)
		{
			m_Player = 1;
		}
		else
		{
			m_Player = 2;
		}

		//gets reference to sound manager
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();

		//Gets reference to hud
		m_Hud = GameObject.FindGameObjectWithTag(Constants.HUD).GetComponent<Hud>();

		//setting initial values for the timers
		m_HealthRegenTimer = HealthRegenTime;
		m_InvulnerabilityTimer = InvulnerabilityTimer;

        //setting the total health
		m_TotalHealth = m_Health;

		//Set Health in hud
		m_Hud.SetHealth (m_TotalHealth, m_Player);

        m_PlayerRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

		//Get the players movement for knockback
		m_Movement = GetComponent<BaseMovementAbility> ();
	}

	void OnDestroy()
	{
		//removes the camera from the static list
		for(int i = 0; i < m_PlayersList.Count; i++)
		{
			if(m_PlayersList[i] == this)
			{
				m_PlayersList.RemoveAt(i);
				break;
			}
		}
	}
	
	void Awake()
	{
		m_PlayersList.Add(this);
	}
	
	// Update is called once per frame
	protected void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

        if (m_InvulnerabilityTimer > 0.0f)
        {
            m_InvulnerabilityTimer -= Time.deltaTime;

           // Debug.Log(Mathf.Abs(Mathf.Cos(m_InvulnerabilityTimer * 5.0f)));
        
            m_PlayerRenderer.materials[0].color = new Color(m_PlayerRenderer.materials[0].color.r,
                                                            m_PlayerRenderer.materials[0].color.g,
                                                            m_PlayerRenderer.materials[0].color.b,
                                                            1.0f - (MAX_FADE * Mathf.Abs(Mathf.Cos((m_InvulnerabilityTimer % (FLASH_LENGTH)) / (FLASH_LENGTH)))));
        }
        else
        {
            m_PlayerRenderer.materials[0].color = new Color(m_PlayerRenderer.materials[0].color.r,
                                                            m_PlayerRenderer.materials[0].color.g,
                                                            m_PlayerRenderer.materials[0].color.b,
                                                            1.0f);
        }

        if(!m_IsDead)
		{
			if (m_Health <= 0.0f)
			{
				onDeath();//you died
			}
			else
			{

				if(m_Health < m_TotalHealth)
				{
                    //need to regen health
					if(m_HealthRegenTimer <= 0.0f)
					{
						m_Health++;
						m_HealthRegenTimer = HealthRegenTime;
                        m_Hud.SetHealth(m_Health, m_Player);
					}
					else
					{
						m_HealthRegenTimer-= Time.deltaTime;
					}
				}
			}
		}
	}

	public override void onHit(LightProjectile proj, float damage)
	{     
		//Knockback
		KnockBackPlayer(proj.gameObject.transform.forward);
	}

    public override void onHit(HeavyProjectile proj, float damage)
    {
		//Knockback
		KnockBackPlayer(proj.gameObject.transform.forward);
    }
	
	public override void onHit(EnemyProjectile proj)
	{
		if(m_InvulnerabilityTimer <= 0.0f)
		{
            //not invulnerable so take damage
			if(m_Health > 0.0f)
			{
				//Take damage
				m_Health -= ENEMY_DAMAGE;

				//Knockback
				KnockBackPlayer(proj.gameObject.transform.forward);

				//play sound
				playSound();
			}

			m_HealthRegenTimer = HealthRegenTime;
			m_InvulnerabilityTimer = InvulnerabilityTimer;
            //update health bar
			m_Hud.SetHealth (m_Health, m_Player);
		}
	}

	protected override void onDeath ()
	{
        if (!m_IsDead)
        {
			//not dead already so die
            m_IsDead = true;
            GameObject ragdoll = (GameObject) Instantiate(m_Ragdoll, transform.position, transform.rotation);
            
			//Give our ragdoll a reference to the camera
			ragdoll.GetComponent<PlayerRagDoll>().m_PlayerCamera = PlayerCamera;
        }
	}

	public override void instantKill ()
	{
		base.instantKill ();
        //update the health
	}

	public void resetHealth()
	{
        //reset everything
		m_IsDead = false;
		m_Health = m_TotalHealth;
		m_InvulnerabilityTimer = InvulnerabilityTimer;
		m_HealthRegenTimer = HealthRegenTime;
		m_Hud.SetHealth (m_Health, m_Player);
		PlayerCamera.Player = this.gameObject.transform.FindChild("\"Centre Point\"").gameObject;
	}

	public void playSound()
	{
		//Check to see which player we are
		switch(this.gameObject.name)
		{
			case Constants.ALEX_WITH_MOVEMENT_STRING:
			//first we check we have any health left, if not, were dead, and should play death sound
			if(m_Health <= 0.0f)
			{
				m_SFX.playSound(transform.position, Sounds.AlexDeath);
			}
			else
			{
				//still have health left so just play hurt sound
				m_SFX.playSound(transform.position, Sounds.AlexHurt);
			}
			break;

			case Constants.DEREK_WITH_MOVEMENT_STRING:
			if(m_Health <= 0.0f)
			{
				m_SFX.playSound(transform.position, Sounds.DerekDeath);
			}
			else
			{
				//still have health left so just play hurt sound
				m_SFX.playSound(transform.position, Sounds.DerekHurt);
			}
			break;

			case Constants.ZOE_WITH_MOVEMENT_STRING:
			if(m_Health <= 0.0f)
			{
				m_SFX.playSound(transform.position, Sounds.ZoeyDeath);
			}
			else
			{
				//still have health left so just play hurt sound
				m_SFX.playSound(transform.position, Sounds.ZoeyHurt);
			}
			break;
		}
	}

	//Causes the player to experience knockback
	void KnockBackPlayer(Vector3 direction)
	{
		Vector2 newDirection = new Vector2 (direction.x, direction.z).normalized;
		m_Movement.Launch(new Vector3(newDirection.x, LAUNCH_UPWARD_DIRECTION, newDirection.y) * LAUNCH_AMOUNT, LAUNCH_TIMER, true);
	}

}
