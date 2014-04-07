using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour 
{
	public EnemySpawn[] m_EnemySpawn;

	/// <summary>
	/// Loops through all the spawn points and spawns the
	/// enemy type associated with it.
	/// </summary>
	public void respawnAll()
	{
		for(int i = 0; i < m_EnemySpawn.Length; i++)
		{
			m_EnemySpawn[i].spawn();
		}
	}
}
