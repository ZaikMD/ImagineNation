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
		//DontDestroyOnLoad(this.gameObject);
	}


	bool m_OnePlayerDead;
	bool m_TwoPlayersDead;

	bool m_PlayerOneDead;
	bool m_PlayerTwoDead;
	

	GameObject m_PlayerOne;
	GameObject m_PlayerTwo;

	PlayerHealth m_PlayerOneHealth;
	PlayerHealth m_PlayerTwoHealth;

	public float m_RespawnTimer = 3.0f;

	protected float RESPAWN_TIMER;

	Vector3 m_RespawnLocation;



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


		if(m_OnePlayerDead && m_TwoPlayersDead != true)
		{
			m_RespawnTimer -= Time.deltaTime;
			//Debug.Log(m_RespawnTimer);

			if(m_RespawnTimer <= 0.0f)
			{
				if(m_PlayerOneHealth.IsDead)
				{
					PlayerRespawnLayerFinder finder = m_PlayerTwo.GetComponentInChildren(typeof(PlayerRespawnLayerFinder)) as PlayerRespawnLayerFinder;
					finder.SetSearchForRespawnLayer(true);

					if(finder.GetRespawnLayerFound())
					{

						if(getPlayerRespawnLocation(m_PlayerTwoHealth.gameObject))
						{
							m_PlayerOneHealth.resetHealth();
							m_PlayerOneHealth.gameObject.transform.position = m_RespawnLocation;
							finder.SetSearchForRespawnLayer(false);

							m_RespawnTimer = RESPAWN_TIMER;

							m_OnePlayerDead = false;
							m_PlayerOneDead = false;
						}
					}
				}

				if(m_PlayerTwoHealth.IsDead)
				{
					PlayerRespawnLayerFinder finder = m_PlayerOne.GetComponentInChildren(typeof(PlayerRespawnLayerFinder)) as PlayerRespawnLayerFinder;
					finder.SetSearchForRespawnLayer(true);
					
					if(finder.GetRespawnLayerFound())
					{

						if(getPlayerRespawnLocation(m_PlayerOneHealth.gameObject))
						{
							m_PlayerTwoHealth.resetHealth();
							m_PlayerTwoHealth.gameObject.transform.position = m_RespawnLocation;
							
							finder.SetSearchForRespawnLayer(false);
							m_OnePlayerDead = false;

							m_RespawnTimer = RESPAWN_TIMER;
							m_PlayerTwoDead = false;
						}
					}
				}
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
				m_PlayerOneDead = true;
				if(!m_OnePlayerDead)
				{
					m_OnePlayerDead = true;

					m_PlayerOneHealth.gameObject.transform.position = gameObject.transform.position;
				}
 
				else
				{
					m_TwoPlayersDead = true;
				}
			}
		}
		
		if(m_PlayerTwoHealth.IsDead)
		{
			if(!m_PlayerTwoDead)
			{
				m_PlayerTwoDead = true;
				if(!m_OnePlayerDead)
				{
					m_OnePlayerDead = true;
					m_PlayerTwoHealth.gameObject.transform.position = gameObject.transform.position;

				}

				else
				{
					m_TwoPlayersDead = true;
				}
			}
		}
	}


	bool getPlayerRespawnLocation(GameObject livingPlayer)
	{
		Vector3 rayDirection = livingPlayer.transform.position - livingPlayer.transform.right;
		
		Ray ray = new Ray(livingPlayer.transform.position, rayDirection);
		
		RaycastHit rayHit;
		
		
		Physics.Raycast (ray, out rayHit, 3.0f);

		if(rayHit.transform == null)
		{
			m_RespawnLocation = livingPlayer.gameObject.transform.right + livingPlayer.gameObject.transform.position;
			return true;
		}

		rayDirection = livingPlayer.transform.position -  -livingPlayer.transform.right;
		ray = new Ray (livingPlayer.transform.position, rayDirection);
		Physics.Raycast (ray, out rayHit, 3.0f);
	
		if(rayHit.transform == null)
		{
			m_RespawnLocation = -livingPlayer.gameObject.transform.right + livingPlayer.gameObject.transform.position;
			return true;
		}

		else
		{
			return false;
		}

	}

}
