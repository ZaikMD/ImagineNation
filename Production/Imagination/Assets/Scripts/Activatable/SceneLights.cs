/* Created by Jason Hein on March 3rd, 2015
 * 
 * START WITH THE AreaTriggerForLightsScript TO USE instructions.
 * Then...
 * 
 * 1. Set m_StartActive to on if you want this light to start turned on, and false to start turned off
 * 2. MinimumIntensity is the intensity of the light in the turned off state.
 * 3. You can choose to this still produce a minimum amount of light.
 * 4. ChangeSpeed effects how fast this light gains and loses light.
 * 5. You can switch around to gain intensity when deactivating, and get dimmer when activated by making ChangeSpeed negative.
 * 6. ActivationDelay can be used to delay this light before it starts gaining light, so you can stagger the lights turning on and off
 */ 

#region changelog
#endregion

using UnityEngine;
using System.Collections;

public class SceneLights : MonoBehaviour
{
	//If this light should start turned on or off
	public bool ActivateOnStartUp = true;

	//Speeds to change this lights intensity
	public float ChangeSpeed = 3.0f;
	float m_IntensityChange = 0.0f;

	//Maximum and minimum intensity
	public float ActiveIntensity = 2.0f;
	public float DeactiveIntensity = 0.0f;

	//Delay before activating or deactivating
	public float ActivationDelay = 0.0f;
	float m_ActivateTimer = -1.0f;


	//Initialization
	void Start ()
	{
		//Check if this lights starts deactivated
		if (ActivateOnStartUp)
		{
			light.enabled = true;

			//Set this lights intensity to the minimum
			light.intensity = DeactiveIntensity;
		}
		else
		{
			//Turn off unecessary lights
			if (DeactiveIntensity <= 0.0f)
			{
				//Turn the light off
				light.enabled = false;
			}

			//Set this lights intensity to the minimum
			light.intensity = DeactiveIntensity;
		}
	}
	
	//Update this light
	void Update ()
	{
		//Timer for activating/deactivating the lights
		if (m_ActivateTimer > -1.0f)
		{
			//Timer for activating/deactivating the lights
			if (m_ActivateTimer > 0.0f)
			{
				m_ActivateTimer -= Time.deltaTime;
			}
			//If the timer has completed, start  getting brighter or dimmer
			else
			{
				EnableActivation ();
			}
		}
		//If the intensity of this light should change
		else if (m_IntensityChange != 0.0f &&
		         (m_IntensityChange > 0.0f && !HandleMaximumIllumination ()) ||
		         (m_IntensityChange < 0.0f && !HandleMinumumIllumination ()))
		{
			//Change this lights intensity
			light.intensity += m_IntensityChange * Time.deltaTime;
		}
	}

	//Start this light activating
	public void SetLightActive (bool activeState)
	{
		//Activate
		if (activeState)
		{
			m_IntensityChange = ChangeSpeed;
		}
		//Deactivate
		else
		{
			m_IntensityChange = -ChangeSpeed;
		}

		//If their is a delay turn on the timer
		if (ActivationDelay > 0.0)
		{
			m_ActivateTimer = ActivationDelay;
		}
		//Otherwise just activate the turning on or off
		else
		{
			EnableActivation ();
		}
	}

	//Add to light intensity
	public void AddToLightIntensity (float intensity)
	{
		//Add to the light if it is turned on
		ActiveIntensity += intensity;

		//The light should now start getting brighter
		SetLightActive (true);
	}

	//Enable activation
	void EnableActivation ()
	{
		//Reset the timer
		m_ActivateTimer = -1.0f;
		
		//If the light is getting brighter
		if (m_IntensityChange > 0.0f)
		{
			//Turn the light on
			light.enabled = true;
		}
	}

	//Check if this light is too bright
	bool HandleMaximumIllumination ()
	{
		//If this light is beyond the maximum intensity
		if (light.intensity >= ActiveIntensity)
		{
			//Cap light intensity
			light.intensity = ActiveIntensity;
			m_IntensityChange = 0.0f;

			//We reached our maximum
			return true;
		}

		//Keep adding intensity
		return false;
	}

	//Check if this light is too dim
	bool HandleMinumumIllumination ()
	{
		//If this light
		if (light.intensity <= DeactiveIntensity)
		{
			//Cap light intensity
			light.intensity = DeactiveIntensity;
			m_IntensityChange = 0.0f;

			//Turn off unecessary lights
			if (light.intensity <= 0.0f)
			{
				//Turn the light off
				light.enabled = false;
			}

			//We reached our minimum
			return true;
		}

		//Keep reducing intensity
		return false;
	}
}
