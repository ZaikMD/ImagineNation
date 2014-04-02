using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour 
{
	public EnemySpawn[] m_EnemySpawn;

	public void respawnAll()
	{
		for(int i = 0; i < m_EnemySpawn.Length; i++)
		{
			m_EnemySpawn[i].spawn();
		}
	}
}
