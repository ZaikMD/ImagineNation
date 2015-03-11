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
		m_ChargingEffectObject = new GameObject[m_ChargingEffectPrefabs.Length];
		m_ChargedEffectObject = new GameObject[m_ChargedEffectPrefabs.Length];
	}
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	protected override void ChargingEffect ()
	{
		for (int i = 0; i < m_ChargingEffectObject.Length; i++)
		{
			m_ChargingEffectObject[i] = (GameObject) Instantiate (m_ChargingEffectPrefabs[i], m_Wand.position, Quaternion.identity);
			m_ChargingEffectObject[i].transform.SetParent (m_Wand);
		}
	}
	
	protected override void ChargedEffect ()
	{
		if (!m_ChargeGlowOn)
		{
			for (int i = 0; i < m_ChargedEffectObject.Length; i++)
			{
				m_ChargedEffectObject[i] = (GameObject) Instantiate (m_ChargedEffectPrefabs[i], m_Wand.position, Quaternion.identity);
				m_ChargedEffectObject[i].transform.SetParent (m_Wand);
			}
			m_ChargeGlowOn = true;
		}
	}
	
	protected override void RemoveChargingEffects ()
	{
		for (int i = 0; i < m_ChargedEffectObject.Length; i++)
		{
			if (m_ChargedEffectObject[i] != null)
				Destroy (m_ChargedEffectObject[i]);
		}
		
		for (int i = 0; i < m_ChargedEffectObject.Length; i++)
		{
			if (m_ChargingEffectObject[i] != null)
				Destroy (m_ChargingEffectObject[i]);
		}
		
		
		
		m_ChargeGlowOn = false;
	}
	
	protected override void AOEEffect ()
	{
		for (int i = 0; i < m_AOEEffects.Length; i++)
		{
			Instantiate (m_AOEEffects[i], m_Wand.position, Quaternion.identity);
		}
		
	}
}