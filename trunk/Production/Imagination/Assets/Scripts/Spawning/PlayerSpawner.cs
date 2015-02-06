/*
*PlayerSpawner
*
*resposible for spawning the players
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour 
{
	public static PlayerSpawner Instance{ get; private set; }
	
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy this
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
	}
	//================================================================================

	const string PLAYER_ONE_SPAWN_POINT = "PlayerOneSpawnPoint";
	const string PLAYER_TWO_SPAWN_POINT = "PlayerTwoSpawnPoint";

	public GameObject ZoeyPrefab;
	public GameObject DerekPrefab;
	public GameObject AlexPrefab;

	// Use this for initialization
	void Start () 
	{
		GameObject currentCheckPoint = null;

		//finds the current check point
		switch(GameData.Instance.CurrentCheckPoint)
		{
		case CheckPoints.CheckPoint_1:
			currentCheckPoint = GameObject.FindGameObjectWithTag(Constants.CHECK_POINT_1_STRING);
			break;
		case CheckPoints.CheckPoint_2:
			currentCheckPoint = GameObject.FindGameObjectWithTag(Constants.CHECK_POINT_2_STRING);
			break;
		case CheckPoints.CheckPoint_3:
			currentCheckPoint = GameObject.FindGameObjectWithTag(Constants.CHECK_POINT_3_STRING);
			break;
		}

		//finds the spawn point
		GameObject spawnPoint = currentCheckPoint.transform.FindChild (PLAYER_ONE_SPAWN_POINT).gameObject;

		//had to give character a value so it wouldnt complain later
		GameObject character = this.gameObject;


		//sets the playerOne character
		switch(GameData.Instance.PlayerOneCharacter)
		{
		case Characters.Zoe:
			character = (GameObject) GameObject.Instantiate (ZoeyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

			character.name = Constants.ZOE_STRING;
			break;
		case Characters.Derek:
			character =  (GameObject) GameObject.Instantiate (DerekPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = Constants.DEREK_STRING;
			break;
		case Characters.Alex:
			character =  (GameObject) GameObject.Instantiate (AlexPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = Constants.ALEX_STRING;
			break;
		}
		//finds the players gameObject using the playerinfo (the player game object is a subObject in the prefab)
		GameObject player = character.GetComponentInChildren (typeof(PlayerInfo)).gameObject;

		//moves the player to the correct position
		player.transform.position = spawnPoint.transform.position;
        player.GetComponent<AcceptInputFrom>().ReadInputFrom = GameData.Instance.m_PlayerOneInput;

		player.GetComponent<PlayerRespawnLayerFinder>().SetSearchForRespawnLayer(true);

		//same stuff but for player two
		spawnPoint = currentCheckPoint.transform.FindChild (PLAYER_TWO_SPAWN_POINT).gameObject;
		switch(GameData.Instance.PlayerTwoCharacter)
		{
		case Characters.Zoe:
			character =  (GameObject) GameObject.Instantiate (ZoeyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = Constants.ZOE_STRING;
			break;
		case Characters.Derek:
			character =  (GameObject) GameObject.Instantiate (DerekPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = Constants.DEREK_STRING;
			break;
		case Characters.Alex:
			character =  (GameObject) GameObject.Instantiate (AlexPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = Constants.ALEX_STRING;
			break;
		}

		player = character.GetComponentInChildren (typeof(PlayerHealth)).gameObject;
		
		player.transform.position = spawnPoint.transform.position;
        player.GetComponent<AcceptInputFrom>().ReadInputFrom = GameData.Instance.m_PlayerTwoInput;

		player.GetComponent<PlayerRespawnLayerFinder>().SetSearchForRespawnLayer(true);
		//this script has done its job and should be deleted now
		Destroy (this.gameObject);
	}
}
