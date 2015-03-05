using UnityEngine;
using System.Collections;

public class PlayerLightController : MonoBehaviour
{
	//The light to affect
	public Light PlayerLight;

	//Ranes
	float m_OriginalRange = 10.0f;
	const float MAX_RANGE_ADDED = 2.0f;
	const float LIGHTPEG_RANGE_ADDED = 0.0f;
	const float RANGE_CHANGE_SPEED = 1.5f;

	//Intesities
	float m_OriginalIntensity = 2.0f;
	const float MAX_INTENSITY_ADDED = 1.25f;
	const float LIGHTPEG_INTENSITY_ADDED = 1.25f;
	const float INTENSITY_CHANGE_SPEED = 1.0f;

	//Get the player lights original intesnity
	void Start ()
	{
		m_OriginalRange = PlayerLight.range;
		m_OriginalIntensity = PlayerLight.intensity;
	}

	void Update ()
	{
		if (PlayerLight.range > m_OriginalRange)
		{
			PlayerLight.range -= Time.deltaTime * RANGE_CHANGE_SPEED;
		}
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
		PlayerLight.range += LIGHTPEG_RANGE_ADDED;
		if (PlayerLight.range - m_OriginalRange > MAX_RANGE_ADDED)
		{
			PlayerLight.range = m_OriginalRange + MAX_RANGE_ADDED;
		}

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
		PlayerLight.range = m_OriginalRange;
		PlayerLight.intensity = m_OriginalIntensity;
	}
}
