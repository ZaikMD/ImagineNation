///
/// Player respawn layer finder.
/// Created by: Matthew Whitlaw
///
/// Attach this script to each player so that respawning in co-op is possible.
/// Once a player is dead, DeadPlayerManager will notify this script on the other
/// player to start searching for a respawn layer, once found it will set a bool
/// to true, and the DeadPlayerManager will respawn the player accordingly.
/// 
/// IMPORTANT: In order for this script to function as intended the desired respawn
/// location must be set to layer "Respawn" and that layer must be the 8th layer in the
/// list of layers. If it is not, then change the variable or move it to 8th in the list.

using UnityEngine;
using System.Collections;

public class PlayerRespawnLayerFinder : MonoBehaviour 
{
	bool m_SearchForRespawnLayer;
	int m_RespawnLayer;
	bool m_RespawnLayerFound;
	float m_MinDistanceFromGroundToRespawn;
	
	void Start () 
	{
		m_SearchForRespawnLayer = true;
		m_RespawnLayer = 1 << 8;
		m_RespawnLayerFound = false;
		m_MinDistanceFromGroundToRespawn = 0.1f;
	}
	
	void Update () 
	{

		if(m_SearchForRespawnLayer == true)
		{
			//If the respawn layer is being searched for then raycast downward and return true
			//if the respawn layer is hit.
			if(Physics.Raycast(transform.position, Vector3.down, m_MinDistanceFromGroundToRespawn, m_RespawnLayer))
			{
				m_RespawnLayerFound = true;
				//Debug.Log("Respawn!"); //For testing purposes, uncomment if needed.
			}
		}
		else
		{
			//If the respawn layer isn't being searched for then
			//the respawn layer shouldn't be found.
			m_RespawnLayerFound = false;
		}
	}

	//DeadPlayerManager will set the search for respawn layer variable
	public void SetSearchForRespawnLayer(bool searchForRespawn)
	{
		m_SearchForRespawnLayer = searchForRespawn;
	}

	//DeadPlayerManager will be able to get whether the respawnLayerFound is true or not.
	public bool GetRespawnLayerFound()
	{
		return m_RespawnLayerFound;
	}
}
