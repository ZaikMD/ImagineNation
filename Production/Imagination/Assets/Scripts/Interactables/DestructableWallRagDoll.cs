using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableWallRagDoll : MonoBehaviour 
{
	float m_DestroyTime = 3.0f;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		m_DestroyTime -= Time.deltaTime;

		if (m_DestroyTime <= 0)
		{
			Destroy (this.gameObject);
		}				
	}
}
