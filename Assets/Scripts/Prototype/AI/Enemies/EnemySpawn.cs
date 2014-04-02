using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	public BaseEnemy m_Enemy;
	//TODO: public enemy type variable

	public void spawn()
	{
		if(m_Enemy != null)
		{
			//TODO: m_Enemy.reset();
		}
		else
		{
			//TODO: instantiate the enemy type specifed
			//Instantiate (m_Enemy, this.transform.position, this.transform.rotation);
		}
	}
}
