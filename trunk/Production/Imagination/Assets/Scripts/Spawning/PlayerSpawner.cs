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
			currentCheckPoint = GameObject.FindGameObjectWithTag("CheckPoint_1");
			break;
		case CheckPoints.CheckPoint_2:
			currentCheckPoint = GameObject.FindGameObjectWithTag("CheckPoint_2");
			break;
		case CheckPoints.CheckPoint_3:
			currentCheckPoint = GameObject.FindGameObjectWithTag("CheckPoint_3");
			break;
		}


		GameObject spawnPoint = currentCheckPoint.transform.FindChild ("PlayerOneSpawnPoint").gameObject;
		GameObject character = this.gameObject;



		switch(GameData.Instance.PlayerOneCharacter)
		{
		case Characters.Zoey:
			character = (GameObject) GameObject.Instantiate (ZoeyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

			character.name = "Zoe";
			break;
		case Characters.Derek:
			character =  (GameObject) GameObject.Instantiate (DerekPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = "Derek";
			break;
		case Characters.Alex:
			character =  (GameObject) GameObject.Instantiate (AlexPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = "Alex";
			break;
		}
		GameObject player = character.GetComponentInChildren (typeof(PlayerHealth)).gameObject;

		player.transform.position = spawnPoint.transform.position;
        player.GetComponent<AcceptInputFrom>().ReadInputFrom = GameData.Instance.m_PlayerOneInput;

		spawnPoint = currentCheckPoint.transform.FindChild ("PlayerTwoSpawnPoint").gameObject;
		switch(GameData.Instance.PlayerTwoCharacter)
		{
		case Characters.Zoey:
			character =  (GameObject) GameObject.Instantiate (ZoeyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = "Zoe";
			break;
		case Characters.Derek:
			character =  (GameObject) GameObject.Instantiate (DerekPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = "Derek";
			break;
		case Characters.Alex:
			character =  (GameObject) GameObject.Instantiate (AlexPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			character.name = "Alex";
			break;
		}

		player = character.GetComponentInChildren (typeof(PlayerHealth)).gameObject;
		
		player.transform.position = spawnPoint.transform.position;
        player.GetComponent<AcceptInputFrom>().ReadInputFrom = GameData.Instance.m_PlayerTwoInput;
		Destroy (this.gameObject);
	}
}
