﻿using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// 
/// Created by Zach Dubuc
/// 
/// This class will inherit from Attackable and will be responsible for toggling a specified lever 
/// and determining whether or not it will on a timer. The lever will switch on and off based on the OnHit
 /// function from Attackable and the timer will be set in the unity editor if one is needed.
/// </summary>
/// 
/// 19/09/14 Matthew Whitlaw EDIT: Added a getActive function
/// 22/09/14 Zach Dubuc EDIT: Added a material when the switch is active/inactive
/// 
public class Switch : SwitchBaseClass, Attackable
{

    public bool m_OnTimer;
    public float m_Timer;

    bool m_Activated;

	protected float m_SaveTimer;

	public Material m_ActiveMaterial;
	public Material m_InactiveMaterial;

	public GameObject m_LeverChange;

	// Use this for initialization
	void Start () 
    {
		m_SaveTimer = m_Timer;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(m_Activated)
		{
			if(m_OnTimer)
			{
				if(m_Timer <= 0)
				{
					resetLever();
				}

				else
				{
					m_Timer -= Time.deltaTime;
				}
			}
			m_LeverChange.renderer.material = m_ActiveMaterial;
		}

		else
		{
			m_LeverChange.renderer.material = m_InactiveMaterial;
		}
	}

	void resetLever()
	{
        m_Activated = false;
		m_Timer = m_SaveTimer;
	}

    public void onHit(PlayerProjectile proj)
    {
        m_Activated = true;
    }

    public void onHit(EnemyProjectile proj)
    {
		return;
    }

	protected virtual void onUse()
	{

	}

}
