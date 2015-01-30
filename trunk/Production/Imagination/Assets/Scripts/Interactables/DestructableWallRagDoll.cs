using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableWallRagDoll : MonoBehaviour 
{
	float m_DestroyTime = 3.0f;

	public Material m_mat;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		//float alphaPercentage = m_DestroyTime * 100 / 3.0f;

		//m_mat.color = new Color (m_mat.color.r, m_mat.color.g, m_mat.color.b, alphaPercentage / 100);

		m_DestroyTime -= Time.deltaTime;

		if (m_DestroyTime <= 0)
		{
			Destroy (this.gameObject);
		}				
	}
}
