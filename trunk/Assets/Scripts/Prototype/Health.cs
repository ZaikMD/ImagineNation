using UnityEngine;
using System.Collections;


/*
TO USE

 * Add script to character
 * Set maximum health of the character
 * set color of the health light (default cyan blue)
 * Call the takeDamage ( int damage ) function to deal damage to the character
 * Call the resetHealth ( ) function to reset the players health when he revives

Created by Jason Hein

3/16/2014
    Implemented
*/



public class Health : MonoBehaviour {

	//Health
	public int m_MaxHealth = 10;
	int m_Health;

	//Default Intensity of  health light
	float m_DefaultIntensity;

	//Light object
	GameObject m_Light = null;

	//You can set the colour of the health light
	public Color m_HealthColor = Color.cyan;

	//You can choose if the character regenerates health
	public bool m_CanRegenerate = true;
	float m_RegeneratationTimer = -1.0f;
	const float REGENERATION_DELAY = 10.0f;


	// Use this for initialization
	void Start () {

		//characters health always starts at its max health
		m_Health = m_MaxHealth;

		//create a health light and make it follow inside the character
		m_Light = (GameObject)Instantiate(Resources.Load("HealthLight"), this.transform.position, Quaternion.identity);
		m_Light.transform.parent = this.transform;

		//Allows you to set the color of the light through unity
		m_Light.light.color = m_HealthColor;

		//Light range must account for this characters size
		m_Light.light.range *= this.transform.localScale.x;

		//set the intensity to whatever intensity is set in the prefab
		m_DefaultIntensity = m_Light.light.intensity;

		takeDamage(9);
	}

	//Regenerate the character's health.
	void Update() 
	{
		if (m_CanRegenerate && m_RegeneratationTimer >= 0.0f && m_Health > 0)
		{
			m_RegeneratationTimer += Time.deltaTime;
			if (m_RegeneratationTimer >= 1.0f + REGENERATION_DELAY)
			{
				m_Health++;
				m_RegeneratationTimer = 0.0f + REGENERATION_DELAY;
				m_Light.light.intensity = m_DefaultIntensity * ((float)m_Health / m_MaxHealth);

				if (m_Health > m_MaxHealth)
				{
					m_RegeneratationTimer = -1.0f;
				}
			}
		}
	}
	

	/// <summary>
	/// Subtracts health from the character and updates the health light.
	/// </summary>
	public void takeDamage(int damage)
	{
		//Update health
		m_Health -= damage;

		//Update the intensity of the health light
		m_Light.light.intensity = m_DefaultIntensity * ((float)m_Health / m_MaxHealth);

		//Sets the regenration timer to start
		m_RegeneratationTimer = 0.0f;
	}

	/// <summary>
	/// Gets the amount of health the character has.
	/// </summary>
	/// <returns>The health.</returns>
	public int getHealth()
	{
		return m_Health;
	}

	/// <summary>
	/// Reset the players health and updates their health light
	/// </summary>
	public void resetHealth()
	{
		m_Health = m_MaxHealth;
		m_Light.light.intensity = m_DefaultIntensity;
	}
}
