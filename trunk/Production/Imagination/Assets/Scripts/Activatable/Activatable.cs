/// 
/// Activatable.
/// Created By: Matthew Whitlaw
/// 
/// This is a base class for all activatable objects, it will act as a built-in switch manager
/// for inheriting classes. Designers can set whether the switch is activated on enemy deaths
/// or on actual switches, and whether or not only one switch is needed.
/// 
/// 
/// 

#region Change Log
#endregion

using UnityEngine;
using System.Collections;

public class Activatable : MonoBehaviour 
{
    public SwitchBaseClass[] m_Switches;
	public bool m_OnlyOneSwitchNeeded = false;
	public EnemyAI[] m_Enemies;
	protected bool m_AllSwitchesActive;
	public bool m_ActivatesOnEnemiesDeath = false;
	bool m_AllEnemiesDead;
	
	void Start () 
	{
		m_AllSwitchesActive = false;
		m_AllEnemiesDead = false;
	}

	//This is the main function that inheriting classes will call.
	//If this function returns true then those classes can use
	//their activatable functionality.
	protected bool CheckSwitches() 
	{
		//Is this object activated on enemy deaths
		if(!m_ActivatesOnEnemiesDeath)
		{
			//Is only one switch required
			if(m_OnlyOneSwitchNeeded)
			{
				//Check to see if atleast one switch is true
				for(int i = 0; i < m_Switches.Length; i++)
				{
					if(m_Switches[i].getActive() == true)
					{
						return true;
					}
				}
			}
			else
			{
				//Check to see if ALL switches are active
				for(int i = 0; i < m_Switches.Length; i++)
				{
					if(m_Switches[i].getActive() != true)
					{
						return false;
					}
				}
				return true;
			}
		}
		else
		{
			//Check the enemy spawners
			if(CheckSpawners())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	//An additional function that will check enemy spawners
	//if the activatable is triggered by enemy deaths
	bool CheckSpawners() 
	{

		for(int i = 0; i < m_Enemies.Length; i++)
		{
			if(m_Enemies[i] != null)
			{
				return false;
			}
		}
		return true;

	}

}
