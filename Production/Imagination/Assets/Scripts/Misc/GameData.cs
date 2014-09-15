using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{

	public static GameData Instance{ get; private set; }
	
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

	public bool m_GameIsRunnging = false;

	void OnLevelWasLoaded(int level)
	{
		if(string.Compare(Application.loadedLevelName, "Game") == 0)
		{
			m_GameIsRunnging = true;
		}
	}
}
