using UnityEngine;
using System.Collections;
/// <summary>
/// Activatable colliders. Script will destroy colliders
/// Created by Zach Dubuc  10/29/14
/// </summary>
public class ActivatableColliders : Activatable 
{
	//The array of colliders so there can be more than one
	public GameObject[] m_Colliders;

	// Update is called once per frame
	void Update () 
	{
		if(CheckSwitches()) //If checkSwitches() returns true, destroy all colliders
		{
			for(int i = 0; i < m_Colliders.Length; i++)
			{
				Destroy(m_Colliders[i]);
			}
		}
	}
}
