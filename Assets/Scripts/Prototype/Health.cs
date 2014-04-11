/*
TO USE

Add script to character

Set maximum health of the character

set color of the health light (default cyan blue)

Call the takeDamage ( int damage ) function to deal damage to the character

Call the resetHealth ( ) function to reset the players health when he revives

Created by Jason "The casual"  Hein


3/16/2014
    Implemented

4/11/2014
Changed by Zach Dubuc to use three cylinders instead of light
*/


using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour {

	//Health
	public int m_MaxHealth = 3;
	public int m_Health;

	//Default Intensity of  health light
	float m_DefaultIntensity;

	//GlowStick object
	public GameObject[] m_Cylinders;

	Vector3[] m_CylinderPositions = new Vector3[3];
	Vector3[] m_CylinderResets = new Vector3[3];

	GameObject[] m_CylinderPositionReset = new GameObject[3];

	//You can choose if the character regenerates health
	public bool m_CanRegenerate = true;
	float m_RegeneratationTimer = -1.0f;
	const float REGENERATION_DELAY = 10.0f;



	// Use this for initialization
	void Start () 
	{

		//characters health always starts at its max health
		m_Health = m_MaxHealth;

		for(int i = 0; i < m_CylinderPositionReset.Length; i ++)
		{
			m_CylinderPositionReset[i] = new GameObject();
			m_CylinderPositionReset[i].transform.parent = this.gameObject.transform;
		}

		//create a health cylinders and make it follow  the character
		for(int i = 0; i < m_Cylinders.Length; i++)
		{
			m_Cylinders[i].SetActive (true);
			m_Cylinders[i].transform.parent = this.gameObject.transform;
			m_CylinderPositions[i] = m_Cylinders[i].transform.position;
			m_CylinderPositionReset[i].transform.position = m_Cylinders[i].transform.position;
		}

		for(int i = 0; i < m_CylinderPositions.Length; i ++)
		{
			m_CylinderResets[i] = new Vector3(gameObject.transform.position.x - m_CylinderPositions[i].x, 
			                                  gameObject.transform.position.y - m_CylinderPositions[i].y,
			                                  gameObject.transform.position.z - m_CylinderPositions[i].z);

		}



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

				if (m_Health > m_MaxHealth)
				{
					m_RegeneratationTimer = -1.0f;
				}
			}
		} 

		for(int i = 0; i < m_Cylinders.Length; i++)
		{
			if(i < m_Health)
			{
				m_Cylinders[i].SetActive(true);  

				m_Cylinders[i].transform.position = m_CylinderPositionReset[i].transform.position;

				Rigidbody rigid = m_Cylinders[i].GetComponent<Rigidbody>();
				if( rigid != null)
				{
					Destroy(rigid);
				}
			}
			else
			{
				if( m_Cylinders[i].GetComponent<Rigidbody>() == null)
				{
					Rigidbody rigid = m_Cylinders[i].AddComponent<Rigidbody>();
					rigid.useGravity = true;
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
		//m_Light.light.intensity = m_DefaultIntensity * ((float)m_Health / m_MaxHealth);

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
		//m_Light.light.intensity = m_DefaultIntensity;
	}
}
