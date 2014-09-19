using UnityEngine;
using System.Collections;

public class Activatable : MonoBehaviour 
{
	public Switch[] m_Switches;
	public bool m_OnlyOneSwitchNeeded = false;
	//public EnemySpawners[] m_Spawners;
	protected bool m_AllSwitchesActive;
	public bool m_ActivatesOnEnemiesDeath = false;
	bool m_AllEnemiesDead;
	
	void Start () 
	{
		m_AllSwitchesActive = false;
		m_AllEnemiesDead = false;
	}
	
	void Update () 
	{

	}
	
	protected bool CheckSwitches() 
	{
		if(!m_ActivatesOnEnemiesDeath)
		{
			if(m_OnlyOneSwitchNeeded)
			{
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
	
	bool CheckSpawners() 
	{
		/*
		for(int i = 0; i < m_Spawners.Length; i++)
		{
			if(m_Spawners[i].GetIsAlive == true)
			{
				return false;
			}
		}*/
		return true;
	}
}
