using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{

	public BaseEnemy m_Enemy;
	public CheckPoint m_MyCheckpoint;


	// Use this for initialization
	void Start () 
	{
		
	}

	void OnLevelWasLoaded(int level)
	{
		if (GameData.Instance.CurrentCheckPoint <= m_MyCheckpoint.m_Value)
		{
			m_Enemy.Reset();

		}

		else 
		{
			m_Enemy.SetIsActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public bool GetIsAlive()
	{
		return m_Enemy.GetIsAlive ();
	}

}
