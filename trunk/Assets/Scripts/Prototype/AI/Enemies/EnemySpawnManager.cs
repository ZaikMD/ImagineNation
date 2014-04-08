using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour 
{
	List <EnemySpawn> m_EnemySpawn = new List<EnemySpawn>();

	void Start()
	{
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");

		for (int i = 0; i < spawners.Length; i++)
		{
			m_EnemySpawn.Add(spawners[i].GetComponent<EnemySpawn>());
		}
	}

	/// <summary>
	/// Loops through all the spawn points and spawns the
	/// enemy type associated with it.
	/// </summary>
	public void respawnAll()
	{
		for(int i = 0; i < m_EnemySpawn.Count; i++)
		{
			m_EnemySpawn[i].spawn();
		}
	}
}
