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
	public GameObject GhostPrefab;
	GameObject m_Ghost;

	float m_Timer = 0.0f;

	public TPCamera m_PlayerCamera;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Use this for initialization
	void Start () 
	{
		//create the ghost
		m_Ghost = (GameObject)GameObject.Instantiate (GhostPrefab, transform.position, transform.rotation);

		//set the timer
		m_Timer = DeadPlayerManager.Instance.m_RespawnTimer;

		//tell the camera to look at the ghost instead of the player
		m_PlayerCamera.Player = m_Ghost;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		if(m_Ghost != null)
		{
			//update the ghost
			m_Ghost.transform.Rotate (Vector3.up);
			m_Ghost.transform.position = m_Ghost.transform.position + (Vector3.up * Time.deltaTime);
		}

		m_Timer -= Time.deltaTime;
		if(m_Timer < 0.0f && DeadPlayerManager.Instance.areBothPlayersAlive())
		{
			//time to despawn the ghost and get rid of the game object
			Destroy(m_Ghost);
			Destroy(this.gameObject);
		}
	}
}
