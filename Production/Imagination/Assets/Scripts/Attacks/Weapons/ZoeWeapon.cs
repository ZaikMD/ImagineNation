﻿using UnityEngine;
using System.Collections;

public class ZoeWeapon : BaseWeapon 
{
	Transform m_Wand;

	// Use this for initialization
	void Start () 
	{
		start ();

		m_Wand = GameObject.Find ("WandStar").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }
		update ();
	}

	protected override void ChargingEffect ()
	{
		if (m_ChargingEffectObject != null) 
		{
			
			
			
		}
	}
	
	protected override void RemoveChargingEffects ()
	{
		if (m_ChargingEffectObject != null) 
		{
			
			
			
		}
	}
	
	protected override void AOEEffect ()
	{
		for (int i = 0; i < m_AOEEffects.Length; i++)
		{
			if (m_AOEEffects[i] != null)
			{
				Instantiate (m_AOEEffects[i], m_Wand.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Wand.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Wand.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Wand.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Wand.position, Quaternion.identity);
			}
		}
		
	}

	protected override void AOESlamEffect ()
	{
		for (int i = 0; i < m_AOESlamEffects.Length; i++)
		{
			if (m_AOESlamEffects != null)
				Instantiate (m_AOESlamEffects[i], m_Wand.position, Quaternion.identity);
		}
	}
}