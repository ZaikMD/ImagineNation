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

	public GameObject m_Prefab;

	BaseEnemy m_Enemy;
	EnemyTypes m_EnemyType;

	/// <summary>
	/// Spawns the enemy based off the enemy type
	/// </summary>
	public void spawn()
	{
		if(m_Enemy != null)
		{
			m_Enemy.Reset();
		}
		else
		{
			switch(m_EnemyType)
			{
				case  EnemyTypes.Furbull:
				{
					GameObject enemy = (GameObject)Instantiate(m_Prefab, this.transform.position, this.transform.rotation);
							
					m_Enemy = enemy.GetComponent<BaseEnemy>();
					break;
				}
			}
		}
	}
}
