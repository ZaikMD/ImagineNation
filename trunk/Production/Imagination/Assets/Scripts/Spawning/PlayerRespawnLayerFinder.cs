///
/// Player respawn layer finder.
/// Created by: Matthew Whitlaw
///
/// Attach this script to each player so that respawning in co-op is possible.
/// Once a player is dead, DeadPlayerManager will notify this script on the other
/// player to start searching for a respawn layer, once found it will set a bool
/// to true, and the DeadPlayerManager will respawn the player accordingly.
/// 
/// 
/// 

#region Change Log
//Name: Matthew Whitlaw
//Date: 15/10/2014
//Change: Instead of using bit shifting to find the respawn layer
//this script now uses GetMask to check the layer the raycast hit.
#endregion

using UnityEngine;
using System.Collections;

public class PlayerRespawnLayerFinder : MonoBehaviour 
{
	const string RESPAWN_LAYER = "Respawn";
	bool m_SearchForRespawnLayer;
	bool m_RespawnLayerFound;
	float m_MinDistanceFromGroundToRespawn;
	
	void Start () 
	{
		m_SearchForRespawnLayer = true;
		m_RespawnLayerFound = false;
		m_MinDistanceFromGroundToRespawn = 0.1f;
	}
	
	void Update () 
	{

		if(m_SearchForRespawnLayer == true)
		{
			RaycastHit hitInfo;
			bool rayHit; 
			//If the respawn layer is being searched for then raycast downward and return true
			//if the respawn layer is hit.
			//if(Physics.Raycast(transform.position, Vector3.down, m_MinDistanceFromGroundToRespawn, m_RespawnLayer))
			rayHit = Physics.Raycast(transform.position, Vector3.down, out hitInfo, m_MinDistanceFromGroundToRespawn, LayerMask.GetMask(RESPAWN_LAYER));

			if(rayHit)
			{
				m_RespawnLayerFound = true;
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
