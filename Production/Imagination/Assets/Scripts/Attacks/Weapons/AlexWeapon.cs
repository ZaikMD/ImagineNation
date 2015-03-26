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

		m_FadeMaxAmount = 1.0f;
		m_FadeGrowRate = 2.0f / m_MinChargeTime;
	}	
	
	// Update is called once per frame
	void Update () 
	{
		if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }
		update ();
	}
	
	protected override void AOEEffect ()
	{
		for (int i = 0; i < m_AOEEffects.Length; i++)
		{
			if (m_AOEEffects[i] != null)
			{
				Instantiate (m_AOEEffects[i], m_Sword.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Sword.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Sword.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Sword.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_Sword.position, Quaternion.identity);

			}
		}

	}

	protected override void AOESlamEffect ()
	{
		for (int i = 0; i < m_AOESlamEffects.Length; i++)
		{
			if (m_AOESlamEffects != null)
				Instantiate (m_AOESlamEffects[i], m_Sword.position, Quaternion.identity);
		}
	}
}