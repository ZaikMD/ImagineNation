/* Created by Jason Hein on March 3rd, 2015
 * 
 * TO USE:
 * 
 * 1. Create an empty gameobject (or prefab), and add a trigger collider.
 * 2. Attach this script.
 * 3. Add the SceneLights script to lights affected by this script.
 * 4. Add those lights to the SceneLights array for this object.
 */ 


using UnityEngine;
using System.Collections;

public class AreaTriggerForLights : MonoBehaviour
{
	//Whether to activate or deactivate when you enter or exit
	public bool m_OnEnterState = true;
	public bool m_OnExitState = true;

	//Lights to affect
	public SceneLights[] m_TriggeredLights;

	//When this trigger is entered
	void OnTriggerEnter (Collider other)
	{
		//Only activate for players
		if (other.tag == Constants.PLAYER_STRING)
		{
			//Activate all the lights
			for (int i = 0; i < m_TriggeredLights.Length; i++)
			{
				m_TriggeredLights[i].SetLightActive(m_OnEnterState);
			}
		}
	}

	//When this trigger is exited
	void OnTriggerExit (Collider other)
	{
		//Only deactivate for players
		if (m_OnEnterState != m_OnExitState && other.tag == Constants.PLAYER_STRING)
		{
			//Deactivate all the lights
			for (int i = 0; i < m_TriggeredLights.Length; i++)
			{
				m_TriggeredLights[i].SetLightActive(m_OnExitState);
			}
		}
	}
}
