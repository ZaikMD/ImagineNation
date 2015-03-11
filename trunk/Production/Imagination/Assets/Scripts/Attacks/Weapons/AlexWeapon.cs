using UnityEngine;
using System.Collections;

public class AlexWeapon : BaseWeapon 
{
	Transform m_Sword;

	// Use this for initialization
	void Start () 
	{
		start ();
		m_Sword = GameObject.Find ("SwordBlade").transform;
		m_ChargingEffectObject = new GameObject[m_ChargingEffectPrefabs.Length];
		m_ChargedEffectObject = new GameObject[m_ChargedEffectPrefabs.Length];
	}	
	
	// Update is called once per frame
	void Update () 
	{
		if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }
		update ();
	}

	protected override void ChargingEffect ()
	{
		for (int i = 0; i < m_ChargingEffectObject.Length; i++)
		{
			m_ChargingEffectObject[i] = (GameObject) Instantiate (m_ChargingEffectPrefabs[i], m_Sword.position, Quaternion.identity);
			m_ChargingEffectObject[i].transform.SetParent (m_Sword);
		}
	}
	
	protected override void ChargedEffect ()
	{
		if (!m_ChargeGlowOn)
		{
			for (int i = 0; i < m_ChargedEffectObject.Length; i++)
			{
				m_ChargedEffectObject[i] = (GameObject) Instantiate (m_ChargedEffectPrefabs[i], m_Sword.position, Quaternion.identity);
				m_ChargedEffectObject[i].transform.SetParent (m_Sword);
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
			Instantiate (m_AOEEffects[i], m_Sword.position, Quaternion.identity);
		}

	}

	protected override void AOESlamEffect ()
	{
		for (int i = 0; i < m_AOESlamEffects.Length; i++)
		{
			Instantiate (m_AOESlamEffects[i], m_Sword.position, Quaternion.identity);
		}
	}
}