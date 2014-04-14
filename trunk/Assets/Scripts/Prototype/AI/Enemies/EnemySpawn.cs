using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	public GameObject m_Prefab;

	BaseEnemy m_Enemy = null;
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
			GameObject enemy = (GameObject)Instantiate(m_Prefab, this.transform.position, this.transform.rotation);
		}
	}
}
