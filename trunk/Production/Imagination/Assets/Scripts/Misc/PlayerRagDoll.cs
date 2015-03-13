/*
 * this class creates and destroys the ghost and player
 * it also tells the camera to follow the ghost and not the player
 * 
 * created by Kris MAtis 10/10/2014
 * 
 * commented and cleaned 17/1012014
 * 
 * 
 */


using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class PlayerRagDoll : MonoBehaviour 
{
	//the prefab and game object for the ghost
	public GameObject m_RagdollBody;

	float m_Timer = 0.0f;

	public TPCamera m_PlayerCamera;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Use this for initialization
	void Start () 
	{
		if (m_RagdollBody != null)
		{
			m_PlayerCamera.Player = m_RagdollBody;
		}
		else
		{
			m_PlayerCamera.Player = this.gameObject;
		}
		//set the timer
	//	m_Timer = DeadPlayerManager.Instance.m_RespawnTimer;
		m_Timer = 5;

		//tell the camera to look at the ghost instead of the player
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }


		m_Timer -= Time.deltaTime;
		if(m_Timer < 0.0f && DeadPlayerManager.Instance.areBothPlayersAlive())
		{
			//time to despawn the ghost and get rid of the game object
			Destroy(this.gameObject);
		}
	}
}
