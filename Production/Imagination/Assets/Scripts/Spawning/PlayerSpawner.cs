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
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(this.gameObject);
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
		switch(GameData.Instance.PlayerOneCharacter)
		{
		case Characters.Zoey:
			GameObject.Instantiate (ZoeyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			break;
		case Characters.Derek:
			GameObject.Instantiate (DerekPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			break;
		case Characters.Alex:
			GameObject.Instantiate (AlexPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			break;
		}

		spawnPoint = currentCheckPoint.transform.FindChild ("PlayerTwoSpawnPoint").gameObject;
		switch(GameData.Instance.PlayerTwoCharacter)
		{
		case Characters.Zoey:
			GameObject.Instantiate (ZoeyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			break;
		case Characters.Derek:
			GameObject.Instantiate (DerekPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			break;
		case Characters.Alex:
			GameObject.Instantiate (AlexPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			break;
		}

		Destroy (this.gameObject);
	}
}
