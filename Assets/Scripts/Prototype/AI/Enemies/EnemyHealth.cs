using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{

	//Quick script nearly identical to player health
	//Simply a convenience to not require a visual display for health
	//Due to time constraints no health base class exists

	public int m_MaxHealth = 3;
	public int m_Health;

	//You can choose if the character regenerates health
	public bool m_CanRegenerate = true;
	float m_RegeneratationTimer = -1.0f;
	const float REGENERATION_DELAY = 10.0f;

	// Use this for initialization
	void Start () 
	{
		m_Health = m_MaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_CanRegenerate && m_RegeneratationTimer >= 0.0f && m_Health > 0)
		{
			m_RegeneratationTimer += Time.deltaTime;
			if (m_RegeneratationTimer >= 10.0f + REGENERATION_DELAY)
			{
				m_Health++;
				m_RegeneratationTimer = 0.0f + REGENERATION_DELAY;
				
				if (m_Health >= m_MaxHealth)
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
	}
}
