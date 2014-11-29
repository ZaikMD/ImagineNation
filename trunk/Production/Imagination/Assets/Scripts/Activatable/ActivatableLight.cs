///
/// Activatable light.
/// Created By: Matthew Whitlaw
/// 
/// This class inherits from activatable and simply toggles a light
/// on and off based on a switch or switches becoming active
/// 
/// IMPORTANT: Ensure that the light's original intensity is set to 0 in the editor.
/// 

#region Change Log
/// EDIT: 26/09/14 - Added functionality to toggle Darkness' activity - Matthew Whitlaw
/// EDIT: 28/11/14 - Added public variable to set the new light intensity after activating it - Jason Hein
#endregion

using UnityEngine;
using System.Collections;


public class ActivatableLight : Activatable 
{
	public GameObject m_Darkness;
	public float m_NewIntensity = 1.0f;


	bool m_IsActive;
	Light m_Light;
	float m_OriginalIntensity;

	//Get the necessary light component and record its
	//initial intensity
	void Start () 
	{
		m_IsActive = false;
		m_Light = GetComponent<Light> ();
		m_OriginalIntensity = m_Light.intensity;
	}
	
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}

		//Check to see if the switch is already active or not
		if(!m_IsActive)
		{
			//Call the base checkswitches function
			//and if it returns true turn on the light
			if(CheckSwitches())
			{
				TurnOnLight();
				m_IsActive = true;
				m_Darkness.SetActive(false);
			}
		}
		else
		{
			//If the switch is active then check to see
			//if any of the switches have deactivated if so
			//turn off the light
			for(int i = 0; i < m_Switches.Length; i++)
			{
				if(!m_Switches[i].getActive())
				{
					ResetLight();
					m_IsActive = false;
					m_Darkness.SetActive(true);
				}
			}
		}

	}

	//Simply set the light's intensity to zero
	void TurnOnLight()
	{
		m_Light.intensity = m_NewIntensity;
	}

	//Reset the light's intensity back to it's original
	void ResetLight()
	{
		m_Light.intensity = m_OriginalIntensity;
	}
}
