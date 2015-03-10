﻿using UnityEngine;
using System.Collections;

public class ZoeWeapon : BaseWeapon 
{
	Transform m_Wand;

	GameObject m_ChargingEffectObject;
	GameObject m_ChargedEffectObject;

	bool m_ChargeGlowOn = false;

	// Use this for initialization
	void Start () 
	{
		start ();

		m_Wand = GameObject.Find ("WandStar").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		update ();
	}

	protected override void ChargingEffect ()
	{
		m_ChargingEffectObject = (GameObject) Instantiate (m_ChargingEffect, m_Wand.position, Quaternion.identity);
		m_ChargingEffectObject.transform.SetParent (m_Wand);
	}
	
	protected override void ChargedEffect ()
	{
		if (!m_ChargeGlowOn)
		{
			m_ChargedEffectObject = (GameObject) Instantiate (m_ChargedEffect, m_Wand.position, Quaternion.identity);
			m_ChargedEffectObject.transform.SetParent (m_Wand);
			m_ChargeGlowOn = true;
		}
	}
	
	protected override void RemoveChargingEffects ()
	{
		if (m_ChargingEffectObject != null)
			Destroy (m_ChargingEffectObject);
		
		if (m_ChargedEffectObject != null)
			Destroy (m_ChargedEffectObject);
		
		m_ChargeGlowOn = false;
	}
	
	protected override void AOEEffect ()
	{
		Instantiate (m_AOEEffectOne, m_Wand.position, Quaternion.identity);
		Instantiate (m_AOEEffectTwo, m_Wand.position, Quaternion.identity);
	}
	

}
