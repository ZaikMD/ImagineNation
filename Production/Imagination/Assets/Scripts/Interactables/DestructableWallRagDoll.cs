using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableWallRagDoll : MonoBehaviour 
{
	float m_DestroyTime = 3.0f;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}


				m_DestroyTime -= Time.deltaTime;

				if (m_DestroyTime <= 0)
				{
					Destroy (this.gameObject);
				}

				
	}
}
