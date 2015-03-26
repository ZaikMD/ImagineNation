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

		m_ChargingEffectMat = m_ChargingEffectObject.renderer.material;

		m_ObjectMaxSize = 0.25f;
		m_ObjectGrowRate = m_ObjectMaxSize / m_MinChargeTime;

		m_FadeMaxAmount = 1.0f;
		m_FadeGrowRate = 2.9f / m_MinChargeTime;

		m_ChargingObjectStartScale = m_ChargingEffectObject.transform.localScale;
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
			Vector3 scale = m_ChargingEffectObject.transform.localScale;
			scale.x += m_ObjectGrowRate * Time.deltaTime;
			scale.y += m_ObjectGrowRate * Time.deltaTime;
			scale.z += m_ObjectGrowRate * Time.deltaTime;

			if (scale.x > m_ObjectMaxSize)
				scale.x = m_ObjectMaxSize;

			if (scale.y > m_ObjectMaxSize)
				scale.y = m_ObjectMaxSize;

			if (scale.z > m_ObjectMaxSize)
				scale.z = m_ObjectMaxSize;

			m_ChargingEffectObject.transform.localScale = scale;
		}

		if (m_ChargingEffectMat != null)
		{
			m_ChargingEffectMat.SetFloat("_FadeAmount", m_ChargingEffectMat.GetFloat("_FadeAmount") - m_FadeGrowRate * Time.deltaTime);

			if (m_ChargingEffectMat.GetFloat("_FadeAmount") < m_FadeMaxAmount)
				m_ChargingEffectMat.SetFloat("_FadeAmount", m_FadeMaxAmount);
		}

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
