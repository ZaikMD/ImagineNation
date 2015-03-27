using UnityEngine;
using System.Collections;

public class DerekWeapon : BaseWeapon 
{
	Transform m_LeftGlove;
	Transform m_RightGlove;

	// Use this for initialization
	void Start () 
	{
		start ();
		m_LeftGlove = GameObject.Find ("LeftGloveEffect").transform;
		m_RightGlove = GameObject.Find ("RightGloveEffect").transform;

		m_FadeMaxAmount = 1.0f;
		m_FadeGrowRate = 2.9f / m_MinChargeTime;
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
				Instantiate (m_AOEEffects[i], m_RightGlove.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_RightGlove.position, Quaternion.identity);
				Instantiate (m_AOEEffects[i], m_RightGlove.position, Quaternion.identity);
			}
		}

		m_SFX.playSound(this.transform, Sounds.AirSmashAttack);

	}

	protected override void AOESlamEffect ()
	{
		for (int i = 0; i < m_AOESlamEffects.Length; i++)
		{
			if (m_AOESlamEffects != null)
				Instantiate (m_AOESlamEffects[i], m_RightGlove.position, Quaternion.identity);
		}
	}
}
