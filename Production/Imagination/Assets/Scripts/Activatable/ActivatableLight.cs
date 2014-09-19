using UnityEngine;
using System.Collections;


public class ActivatableLight : Activatable 
{
	bool m_IsActive;
	Light m_Light;
	float m_OriginalIntensity;

	void Start () 
	{
		m_IsActive = false;
		m_Light = GetComponent<Light> ();
		m_OriginalIntensity = m_Light.intensity;
	}
	
	void Update () 
	{
		if(!m_IsActive)
		{
			if(CheckSwitches())
			{
				TurnOnLight();
				m_IsActive = true;
			}
		}
		else
		{
			for(int i = 0; i < m_Switches.Length; i++)
			{
				if(!m_Switches[i].getActive())
				{
					ResetLight();
					m_IsActive = false;
				}
			}
		}

	}
	
	void TurnOnLight()
	{
		m_Light.intensity = 0.0f;
	}

	void ResetLight()
	{
		m_Light.intensity = m_OriginalIntensity;
	}
}
