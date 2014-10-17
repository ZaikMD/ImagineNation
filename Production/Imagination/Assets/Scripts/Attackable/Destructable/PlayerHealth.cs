﻿/*
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
* 
*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	const float TEXTURE_SCALE = 0.45f;

    //used to stop the script from executing and used so other scripts can tell the player is dead
	bool m_IsDead = false;
    public bool IsDead
	{
		get{return m_IsDead;}
		private set{}
	}

	public TPCamera PlayerCamera;

	// Use this for initialization
	void Start () 
	{
		//this block zeros out the parent game object and the health bar GUITexture so health displays properly
		List<Transform> childrenOfParent = new List<Transform> ();

		//this loop gets all the children of the parent
		for(int i = 0; i < transform.parent.transform.childCount; i++)
		{
			Transform child = gameObject.transform.parent.transform.GetChild(i);

			if(child != null)
			{
				childrenOfParent.Add(child);
			}
		}

		//this stores all of the original positions
		Vector3[] originalPositions = new Vector3[childrenOfParent.Count];

		//storeing all the positions
		for(int i = 0; i < childrenOfParent.Count; i++)
		{
			originalPositions[i] = new Vector3(childrenOfParent[i].position.x, childrenOfParent[i].position.y, childrenOfParent[i].position.z);
		}

		//zero out the parent
		this.gameObject.transform.parent.transform.position = Vector3.zero;
		//xero out the healthbar GUITexture
		m_GUITexture.gameObject.gameObject.transform.position = Vector3.zero;

		//move all the children back if they are not the GUITexture
		for(int i = 0; i < childrenOfParent.Count; i++)
		{
			if(childrenOfParent[i].gameObject != m_GUITexture.gameObject)
			{
				childrenOfParent[i].position = originalPositions[i];
			}
		}


        //setting initial values for the timers
		m_HealthRegenTimer = HealthRegenTime;
		m_StopHealthRegenTimer = 0.0f;
		m_InvulnerabilityTimer = InvulnerabilityTimer;

        //setting the total health
		m_TotalHealth = m_Health;

        //setting the current texture
		m_GUITexture.texture = textures [m_Health];

		m_GUITexture.pixelInset = new Rect (0.0f, Screen.height - textures [m_Health].height * TEXTURE_SCALE, textures [m_Health].width * TEXTURE_SCALE, textures [m_Health].height * TEXTURE_SCALE);
	}
	
	// Update is called once per frame
	protected void Update () 
	{	
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
			if(m_Health > 0)
			{
				m_Health -= 1;  
			}
			m_StopHealthRegenTimer = StopHealthRegenTime;
			m_InvulnerabilityTimer = InvulnerabilityTimer;
            //update health bar
			m_GUITexture.texture = textures[m_Health];
		}
	}

	protected override void onDeath ()
	{
        if (!m_IsDead)
        {
            //not dead already so die
            m_IsDead = true;
            GameObject ragdoll = (GameObject) Instantiate(m_Ragdoll, transform.position, transform.rotation);
            
			ragdoll.GetComponent<PlayerRagDoll>().m_PlayerCamera = PlayerCamera;
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
		m_InvulnerabilityTimer = InvulnerabilityTimer;
		StopHealthRegenTime = 0.0f;
		m_GUITexture.texture = textures[m_Health];

		PlayerCamera.Player = this.gameObject.transform.FindChild("\"Centre Point\"").gameObject;
	}
}