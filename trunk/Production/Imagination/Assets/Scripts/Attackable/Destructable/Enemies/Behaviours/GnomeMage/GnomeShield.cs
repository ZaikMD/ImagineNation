/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script takes care of the shield asset on the gnome mage
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class GnomeShield : MonoBehaviour 
{
	public GameObject m_ShieldAsset;
	public GameObject m_ShieldInvincibleAsset;

	float m_DeactiveTimer = 0.0f;
	bool m_ShieldActive;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

		// Use this for initialization
	void Start () 
	{
		m_ShieldInvincibleAsset.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		if (!m_ShieldActive)
		{
			if (m_DeactiveTimer <= 0)
			{
				ReActivateShield();
			}

			m_DeactiveTimer -= Time.deltaTime;
		}
	}

	void ReActivateShield()
	{
		m_ShieldAsset.SetActive(true);
		m_ShieldActive = true;
		m_ShieldInvincibleAsset.SetActive (false);
	}

	// deactivate the shield
	public void DeactivateShield(float time)
	{
		m_DeactiveTimer = time;
		m_ShieldAsset.SetActive (false);
		m_ShieldInvincibleAsset.SetActive (false);
		m_ShieldActive = false;
	}

	public void SwitchToRed()
	{
		m_ShieldInvincibleAsset.SetActive (true);
		m_ShieldAsset.SetActive (false);
	}
}
