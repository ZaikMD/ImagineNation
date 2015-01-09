﻿/*
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

	float m_DeactiveTimer = 0.0f;
	bool m_ShieldActive;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		if (!m_ShieldActive)
		{
			if (m_DeactiveTimer <= 0)
			{
				m_ShieldAsset.SetActive(true);
				m_ShieldActive = true;
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
