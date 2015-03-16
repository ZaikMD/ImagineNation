using UnityEngine;
using System.Collections;

public class PlayerLightController : MonoBehaviour
{
	//The light to affect
	public Light PlayerLight;

	//Intesities
	float m_OriginalIntensity = 2.0f;
	const float MAX_INTENSITY_ADDED = 1.5f;
	const float LIGHTPEG_INTENSITY_ADDED = 1.5f;
	const float INTENSITY_CHANGE_SPEED = 1.75f;

	//Get the player lights original intesnity
	void Start ()
	{
		m_OriginalIntensity = PlayerLight.intensity;
	}

	//Fade light
	void Update ()
	{
		if (PlayerLight.intensity > m_OriginalIntensity)
		{
			PlayerLight.intensity -= Time.deltaTime * INTENSITY_CHANGE_SPEED;
		}
	}

	/// <summary>
	/// Adds to the player lights intesnity.
	/// </summary>
	public void AddToIntesnity ()
	{
		PlayerLight.intensity += LIGHTPEG_INTENSITY_ADDED;
		if (PlayerLight.intensity - m_OriginalIntensity > MAX_INTENSITY_ADDED)
		{
			PlayerLight.intensity = m_OriginalIntensity + MAX_INTENSITY_ADDED;
		}
	}

	/// <summary>
	///Return to original intesnity
	/// </summary>
	public void RemoveAddedIntesnity ()
	{
		PlayerLight.intensity = m_OriginalIntensity;
	}
}
