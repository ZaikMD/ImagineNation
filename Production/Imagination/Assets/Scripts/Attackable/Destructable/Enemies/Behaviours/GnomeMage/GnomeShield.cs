using UnityEngine;
using System.Collections;

public class GnomeShield : MonoBehaviour 
{
	public GameObject m_ShieldAsset;
	private EnemyAI m_EnemyAI;

	float m_DeactiveTimer = 0.0f;
	bool m_ShieldActive;

	// Use this for initialization
	void Start () 
	{
		m_EnemyAI = GetComponentInParent<EnemyAI> ();
	}
	
	// Update is called once per frame
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}

		if (!m_ShieldActive)
		{
			if (m_DeactiveTimer <= 0)
			{
				m_ShieldAsset.SetActive(true);
				m_ShieldActive = true;
				//TODO: Change to valid health number
				m_EnemyAI.SetHealth(3);
			}

			m_DeactiveTimer -= Time.deltaTime;
		}
	}

	public void DeactivateShield(float time)
	{
		m_DeactiveTimer = time;
		m_ShieldAsset.SetActive (false);
		m_ShieldActive = false;
	}
}
