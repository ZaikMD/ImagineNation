/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script takes care of the shield asset on the gnome mage
 * 
 * 
 */
#region ChangeLog
/*
 * 
 * Added Const for Max Health - Joe Burchill Dec.4/2014
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class GnomeShield : MonoBehaviour 
{
	public GameObject m_ShieldAsset;
	private EnemyAI m_EnemyAI;

	float m_DeactiveTimer = 0.0f;
	bool m_ShieldActive;

	private const float MAX_HEALTH = 3.0f;

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
				m_EnemyAI.SetHealth(MAX_HEALTH);
			}

			m_DeactiveTimer -= Time.deltaTime;
		}
	}

	// deactivate the shield
	public void DeactivateShield(float time)
	{
		m_DeactiveTimer = time;
		m_ShieldAsset.SetActive (false);
		m_ShieldActive = false;
	}
}
