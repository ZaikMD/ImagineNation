using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour {

	public GameObject m_AlexDeadPrefab;
	public GameObject m_DerekDeadPrefab;
	public GameObject m_ZoeyDeadPrefab;

	const float RespawnTime = 5;
	const float ResetTime = 5;

	public GameObject PlayerOne;
	public GameObject PlayerTwo;

	float m_Timer;

	bool m_PlayerOneDead;
	bool m_PlayerTwoDead;



	public bool getPlayerOneDead()
	{
		return m_PlayerOneDead;
	}

	public bool getPlayerTwoDead()
	{
		return m_PlayerTwoDead;
	}

	public bool getIsAnyPlayerDead()
	{
		if(getPlayerOneDead() || getPlayerTwoDead())
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static RespawnManager Instance{ get; private set; }
	/// <summary>
	/// Setting the instance.
	/// </summary>
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy this... there can be only one
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}

		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(gameObject);
	}




	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_PlayerOneDead && m_PlayerTwoDead)
		{
			if(m_Timer >= ResetTime)
			{
				m_PlayerOneDead = false;
				m_PlayerTwoDead = false;
				//TODO call respawn function
				return;
			}

			m_Timer += Time.deltaTime;
		}

		if(m_PlayerOneDead || m_PlayerTwoDead)
		{
			if(m_Timer >= RespawnTime)
			{
				respawnPlayer();
				//TODO call respawn function;
				return;
			}
			m_Timer += Time.deltaTime;
		}
	}
	/// <summary>
	/// This function is called when a player dies, 
	/// it handles all timers while player is dead.
	/// </summary>
	/// <param name="player">Player.</param>
	public void playerDied(GameObject player)
	{

		if(player.gameObject.name == PlayerTwo.gameObject.name)
		{
			m_Timer = 0.0f;
			m_PlayerTwoDead = true;
			PlayerTwo.SetActive(false);
			//TODO instantiate a ragdoll.
			if(!m_PlayerOneDead)
			{
		//		PlayerTwo.GetComponent<PlayerState>().m_IsActive = false;
		//		PlayerTwo.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
				//TODO Call Switch character 
				if(player.name == "Zoey")
				{
					Instantiate(m_ZoeyDeadPrefab, player.transform.position, player.transform.rotation);
				}
				else if (player.name == "Derek")
				{
					Instantiate(m_DerekDeadPrefab, player.transform.position, player.transform.rotation);
				}
				else if (player.name == "Alex")
				{
					Instantiate(m_AlexDeadPrefab, player.transform.position, player.transform.rotation);
				}

				CharacterSwitch.Instance.switchCharacters();
				CharacterSwitch.Instance.setIfAbleToSwitch(false);
			}
		}

		else if(player.gameObject.name == PlayerOne.gameObject.name)
		{

			m_Timer = 0.0f;
			m_PlayerOneDead = true;
			PlayerOne.SetActive(false);
			//TODO instantiate a ragdoll.
			if(!m_PlayerOneDead)
			{
			//	PlayerOne.GetComponent<PlayerState>().m_IsActive = false;
			//	PlayerOne.GetComponent<PlayerAIStateMachine>().m_IsActive = true;
				//TODO Call Switch character 
				if(player.name == "Zoey")
				{
					Instantiate(m_ZoeyDeadPrefab, player.transform.position, player.transform.rotation);
				}
				else if (player.name == "Derek")
				{
					Instantiate(m_DerekDeadPrefab, player.transform.position, player.transform.rotation);
				}
				else if (player.name == "Alex")
				{
					Instantiate(m_AlexDeadPrefab, player.transform.position, player.transform.rotation);
				}
				CharacterSwitch.Instance.switchCharacters();
				CharacterSwitch.Instance.setIfAbleToSwitch(false);
			}

		}
	}

	public void respawnPlayer()
	{
		if(m_PlayerOneDead)
		{
			PlayerOne.SetActive(true);
			PlayerOne.GetComponent<PlayerState>().m_PlayerState = PlayerState.PlayerStates.Default;
			PlayerOne.GetComponent<Health>().resetHealth();
			PlayerOne.transform.position = PlayerTwo.transform.position + new Vector3(2, 0, 0);
			PlayerOne.GetComponent<PlayerAIStateMachine>().ClearEnemies();

		}
		else if(m_PlayerTwoDead)
		{
			PlayerTwo.SetActive(true);
			PlayerTwo.GetComponent<PlayerState>().m_PlayerState = PlayerState.PlayerStates.Default;
			PlayerTwo.GetComponent<Health>().resetHealth();
			PlayerTwo.transform.position = PlayerOne.transform.position + new Vector3(2, 0, 0);
			PlayerTwo.GetComponent<PlayerAIStateMachine>().ClearEnemies();
		}
		m_PlayerOneDead = m_PlayerTwoDead = false;
		CharacterSwitch.Instance.setIfAbleToSwitch (true);
		FearScript.Instance.setPlayerIgnore ();
	}

}
