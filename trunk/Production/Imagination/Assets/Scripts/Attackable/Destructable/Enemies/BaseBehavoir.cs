/*
 * Created by Jason Hein October 31/2014
 * Designed to managed all the ai components of this enemy behavoir
 */ 
#region ChangeLog
/* 
 * 
 */
#endregion


using UnityEngine;
using System.Collections.Generic;

public class BaseBehavoir : MonoBehaviour
{
	//A list of components to execute that is chosen by designers for each enemy
	public List<AIControlComponent> m_Components = new List<AIControlComponen>();

	//What states are overrided by the GroupController
	List<bool> m_IgnoreComponent = new List<bool> ();


	//Update the behavoirs logic
	public void Update ()
	{
		//For every component of this behavoir
		for (int i = 0; i < m_Components.Count; i++)
		{
			//If we are not to ignore it
			if (!m_IgnoreComponent[i])
			{
				//Do that comnponents logic
				m_Components[i].Update();
			}
		}
	}

	//Set a component to ignore
	public void SetIgnoreComponent (int index)
	{
		m_IgnoreComponent [index] = true;
	}

	//Set a component to ignore
	public void SetUnIgnoreComponent (int index)
	{
		m_IgnoreComponent [index] = false;
	}
}
