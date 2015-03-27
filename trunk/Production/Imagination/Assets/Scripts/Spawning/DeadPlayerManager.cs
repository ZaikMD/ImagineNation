using UnityEngine;
using System.Collections;
/// <summary>
/// Dead player manager.
/// 
/// Created by Zach Dubuc
/// 
/// Manages the dead player and respawns them when appropriate
/// </summary>
/// 

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented and changed strings to constants- Zach Dubuc
*
*26/11/2014 Edit: Added functionality for lives - Greg
* 
*/
#endregion
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

	//Bools for the players being dead, and for if one or two players are dead
	bool m_OnePlayerDead;
	bool m_TwoPlayersDead;

	bool m_PlayerOneDead;
	bool m_PlayerTwoDead;
	
	//Player GameObjects
	GameObject m_PlayerOne;
	GameObject m_PlayerTwo;

	//PlayerHealth references
	PlayerHealth m_PlayerOneHealth;
	PlayerHealth m_PlayerTwoHealth;

	PlayerHealth m_DeadPlayerHealth;
	PlayerHealth m_AlivePlayerHealth;

	//Respawn Timer
	public float m_RespawnTimer = 3.0f;

	protected float RESPAWN_TIMER;

	//RespawnLocation
	Vector3 m_RespawnLocation;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	//Sounds
	SFXManager m_SFX;


	// Use this for initialization
	void Start () 
	{
		m_SFX = SFXManager.Instance;

		RESPAWN_TIMER = m_RespawnTimer;


		//Getting the players in the scene and their health
		switch(GameData.Instance.PlayerOneCharacter)
		{
		case Characters.Alex:
			m_PlayerOne = GameObject.FindGameObjectWithTag(Constants.ALEX_STRING);
			m_PlayerOneHealth = m_PlayerOne.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;

		case Characters.Derek:
			m_PlayerOne = GameObject.FindGameObjectWithTag(Constants.DEREK_STRING);
			m_PlayerOneHealth = m_PlayerOne.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;

		case Characters.Zoe:
			m_PlayerOne = GameObject.FindGameObjectWithTag(Constants.ZOE_STRING);
			m_PlayerOneHealth = m_PlayerOne.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
		}

		switch(GameData.Instance.PlayerTwoCharacter)
		{
		case Characters.Alex:
			m_PlayerTwo = GameObject.FindGameObjectWithTag(Constants.ALEX_STRING);
			m_PlayerTwoHealth = m_PlayerTwo.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
			
		case Characters.Derek:
			m_PlayerTwo = GameObject.FindGameObjectWithTag(Constants.DEREK_STRING);
			m_PlayerTwoHealth = m_PlayerTwo.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
			
		case Characters.Zoe:
			m_PlayerTwo = GameObject.FindGameObjectWithTag(Constants.ZOE_STRING);
			m_PlayerTwoHealth = m_PlayerTwo.GetComponentInChildren(typeof(PlayerHealth)) as PlayerHealth;

			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		checkPlayersAlive (); //See if players are alive or dead

		respawnPlayer ();//Respawn the players

		if(m_TwoPlayersDead)
		{
			GameData.Instance.DecrementLives();

			//if the team of players still have lives left in their pool, it gets decremented
//			if(GameData.Instance.CurrentLives > 0)
//			{
//				GameData.Instance.DecrementLives();
//			}

//			else
//			{
//				GameData.Instance.ResetLives();
//				GameData.Instance.resetCheckPoint();
//			}

//			Application.LoadLevel(Application.loadedLevelName); //If both players are dead, reset the scene
		}
	}

	public bool areBothPlayersAlive()
	{
        return (m_PlayerOneHealth.IsDead == false) && (m_PlayerTwoHealth.IsDead == false);
	}

	void checkPlayersAlive() //Function to check if the players are alive
	{
		if(m_PlayerOneHealth.IsDead) //If player one is dead
		{
			if(!m_OnePlayerDead) //And one player isn't already dead
			{
				m_PlayerOneDead = true; //PlayerOneDead is set to true
				m_OnePlayerDead = true; //One player dead is set to true
				m_DeadPlayerHealth = m_PlayerOneHealth; //Set the dead player
				m_AlivePlayerHealth = m_PlayerTwoHealth; //Set the Alive Player

				m_DeadPlayerHealth.gameObject.transform.position = gameObject.transform.position; //Put the player in the dead player box
			}
 
			if(m_PlayerTwoDead)
			{
				m_TwoPlayersDead = true; //Otherwise, if one player is already dead, then they are both dead
			}
		}

		
		if(m_PlayerTwoHealth.IsDead) //Do the same thing for the second player
		{
			if(!m_PlayerTwoDead)
			{
				m_PlayerTwoDead = true;
				m_OnePlayerDead = true;
				m_DeadPlayerHealth = m_PlayerTwoHealth;
				m_AlivePlayerHealth = m_PlayerOneHealth;
				m_DeadPlayerHealth.gameObject.transform.position = gameObject.transform.position;


			}
			if(m_PlayerOneDead)
			{
				m_TwoPlayersDead = true;
			}
		}
		
	}

	void respawnPlayer()
	{
		if(m_OnePlayerDead && m_TwoPlayersDead != true)//If one player is dead
		{
			m_RespawnTimer -= Time.deltaTime; //Decrement the Respawn Timer
			
			if(m_RespawnTimer <= 0.0f) //If the respawn timer is less than zero
			{

				PlayerRespawnLayerFinder finder = m_AlivePlayerHealth.GetComponentInChildren(typeof(PlayerRespawnLayerFinder)) as PlayerRespawnLayerFinder; //Get the finder component from the living player
				finder.SetSearchForRespawnLayer(true);
					
				if(finder.GetRespawnLayerFound()) //Check to see if a respawn layer has been found
				{
						
					if(getPlayerRespawnLocation(m_AlivePlayerHealth.gameObject)) //If so, check to see if the player is in a valid location to respawn beside
					{

						if(finder.GetRespawnLayerFound()) //Check again just in case they have moved off a layer
						{
							m_DeadPlayerHealth.resetHealth(); //Reset player health
							m_DeadPlayerHealth.gameObject.transform.position = m_RespawnLocation; //Respawn player
							m_SFX.playSound(this.transform, Sounds.CharacterRespawn);
							finder.SetSearchForRespawnLayer(false); //Set looking for a respawn layer to false

							m_RespawnTimer = RESPAWN_TIMER; //Reset respawn timer
								
							m_OnePlayerDead = false; //Reset bools
						
							if(m_DeadPlayerHealth == m_PlayerOneHealth)
							{
								m_PlayerOneDead = false;							
							}

							else
							{
								m_PlayerTwoDead = false;
							}
						}
					}
				}
			}
		}
	}


	bool getPlayerRespawnLocation(GameObject livingPlayer) //This is to get the respawn position for the dead player
	{
		Vector3 rayDirection = livingPlayer.transform.right; //Check the right side of the player first
		
		Ray ray = new Ray(livingPlayer.transform.position, rayDirection);
		
		RaycastHit rayHit;
		

		Physics.Raycast (ray, out rayHit, 3.0f);


		if(rayHit.transform == null) //If the ray didn't hit anything, set the respawn location to the right side of the player
		{
			m_RespawnLocation = livingPlayer.gameObject.transform.position + livingPlayer.gameObject.transform.right;
			return true;
		}
		//Checking the left side

		rayDirection = -(livingPlayer.transform.right);

		ray = new Ray(livingPlayer.transform.position, rayDirection);


		Physics.Raycast (ray, out rayHit, 3.0f);
	
		if(rayHit.transform == null) //If the ray didn't hit anything, set the respawn location to the left side of the player
		{
			m_RespawnLocation = livingPlayer.gameObject.transform.position - (livingPlayer.gameObject.transform.right);
			return true;
		}

		else //Otherwise return false
		{
			return false;
		}

	}

}
