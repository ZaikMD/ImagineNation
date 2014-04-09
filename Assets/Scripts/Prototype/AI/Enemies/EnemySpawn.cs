using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	enum EnemyTypes
	{
		Furbull = 0,
		Crochuck,
		Count
	}

	public BaseEnemy m_Enemy;
	EnemyTypes m_EnemyType;

	/// <summary>
	/// Spawns the enemy based off the enemy type
	/// </summary>
	public void spawn()
	{
		if(m_Enemy != null)
		{
			m_Enemy.reset();
		}
//		else
//		{
//			switch(m_EnemyType)
//			{
//				case(Furbull)
//				{
//
//				}
//			}
//		}
	}
}
