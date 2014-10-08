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


		GameObject spawnPoint = currentCheckPoint.transform.FindChild (PLAYER_ONE_SPAWN_POINT).gameObject;
		GameObject character = this.gameObject;



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
		GameObject player = character.GetComponentInChildren (typeof(PlayerHealth)).gameObject;

		player.transform.position = spawnPoint.transform.position;
        player.GetComponent<AcceptInputFrom>().ReadInputFrom = GameData.Instance.m_PlayerOneInput;

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
		Destroy (this.gameObject);
	}
}
