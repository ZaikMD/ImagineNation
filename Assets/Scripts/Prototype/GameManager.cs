using UnityEngine;
using System.Collections;

/// <summary>
/// anything with an update should observe this for noe all it sends if events when the game is paused/ unpaused
/// </summary>
public class GameManager : Subject 
{

	public static GameManager Instance{ get; private set; }

	bool m_IsPaused = false;


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

	// Update is called once per frame
	void Update () 
	{
		if(PlayerInput.Instance.getPause())
		{
			if(m_IsPaused)
			{
				sendEvent(ObeserverEvents.StartGame);
			}
			else
			{
				sendEvent(ObeserverEvents.PauseGame);
			}
			m_IsPaused = !m_IsPaused;
		}
	}


	/// <summary>
	/// Dont call this unless told to
	/// </summary>
	public void startGame()
	{
		if(m_IsPaused)
		{
			m_IsPaused = false;
		}
		sendEvent(ObeserverEvents.StartGame);
	}
}
