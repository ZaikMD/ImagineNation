using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	public BaseEnemy m_Enemy;
	//TODO: public enemy type variable

	/// <summary>
	/// Spawns the enemy based off the enemy type
	/// </summary>
	public void spawn()
	{
		if(m_Enemy != null)
		{
			m_Enemy.reset();
		}
		else
		{
			//TODO: instantiate the enemy type specified
			//Instantiate (m_Enemy, this.transform.position, this.transform.rotation);
		}
	}
}
