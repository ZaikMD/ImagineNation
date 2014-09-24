using UnityEngine;
using System.Collections;

public class DeadPlayerManager : MonoBehaviour 
{
	public static DeadPlayerManager Instance{ get; private set; }
	
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy this.. THERE CAN BE ONLY ONE
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(this.gameObject);
	}


	bool m_OnePlayerDead;
	bool m_TwoPlayersDead;
	

	GameObject m_PlayerOne;
	GameObject m_PlayerTwo;

	PlayerHealth m_PlayerOneHealth;
	PlayerHealth m_PlayerTwoHealth;

	public float m_RespawnTimer = 3.0f;

	protected float RESPAWN_TIMER;



	// Use this for initialization
	void Start () 
	{
		RESPAWN_TIMER = m_RespawnTimer;

		switch(GameData.Instance.PlayerOneCharacter)
		{
		case Characters.Alex:
			m_PlayerOne = GameObject.FindGameObjectWithTag("Alex");
			m_PlayerOneHealth = m_PlayerOne.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;

		case Characters.Derek:
			m_PlayerOne = GameObject.FindGameObjectWithTag("Derek");
			m_PlayerOneHealth = m_PlayerOne.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;

		case Characters.Zoey:
			m_PlayerOne = GameObject.FindGameObjectWithTag("Zoe");
			m_PlayerOneHealth = m_PlayerOne.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
		}

		switch(GameData.Instance.PlayerTwoCharacter)
		{
		case Characters.Alex:
			m_PlayerTwo = GameObject.FindGameObjectWithTag("Alex");
			m_PlayerTwoHealth = m_PlayerTwo.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
			
		case Characters.Derek:
			m_PlayerTwo = GameObject.FindGameObjectWithTag("Derek");
			m_PlayerTwoHealth = m_PlayerTwo.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
			
		case Characters.Zoey:
			m_PlayerTwo = GameObject.FindGameObjectWithTag("Zoe");
			m_PlayerTwoHealth = m_PlayerTwo.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkPlayersAlive ();


		if(m_OnePlayerDead)
		{
			m_RespawnTimer -= Time.deltaTime;
			Debug.Log(m_RespawnTimer);

			if(m_RespawnTimer <= 0.0f)
			{
				if(m_PlayerOneHealth.IsDead)
				{
					PlayerRespawnLayerFinder finder = m_PlayerTwo.GetComponentInChildren(typeof(PlayerRespawnLayerFinder)) as PlayerRespawnLayerFinder;
					finder.SetSearchForRespawnLayer(true);

					if(finder.GetRespawnLayerFound())
					{
						m_PlayerOneHealth.resetHealth();
						m_PlayerOne.transform.position = m_PlayerTwo.transform.position;

						finder.SetSearchForRespawnLayer(false);

						m_RespawnTimer = RESPAWN_TIMER;

						m_OnePlayerDead = false;
					}
				}
				/*
				if(m_PlayerOneHealth.IsDead)
				{
					PlayerRespawnLayerFinder finder = m_PlayerOne.GetComponentInChildren(typeof(PlayerRespawnLayerFinder)) as PlayerRespawnLayerFinder;
					finder.SetSearchForRespawnLayer(true);
					
					if(finder.GetRespawnLayerFound())
					{
						m_PlayerTwoHealth.resetHealth();
						m_PlayerTwo.transform.position = m_PlayerOne.transform.position;
						
						finder.SetSearchForRespawnLayer(false);
						m_OnePlayerDead = false;

						m_RespawnTimer = RESPAWN_TIMER;
					}
				}*/
			}
		}


		if(m_TwoPlayersDead)
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}


	void checkPlayersAlive()
	{
		if(m_PlayerOneHealth.IsDead)
		{
			if(!m_OnePlayerDead)
			{
				m_OnePlayerDead = true;
			}

			else
			{
				m_TwoPlayersDead = true;
			}
		}
		
		if(m_PlayerTwoHealth.IsDead)
		{
			if(!m_OnePlayerDead)
			{
				m_OnePlayerDead = true;
			}

			else
			{
				m_TwoPlayersDead = true;
			}
		}
	}

}
