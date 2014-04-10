using UnityEngine;
using System.Collections;

/// <summary>
/// anything with an update should observe this for noe all it sends if events when the game is paused/ unpaused
/// </summary>
public class GameManager : Subject 
{

	public static GameManager Instance{ get; private set; }

	bool m_IsPaused = false;

	public enum Stage
	{
		StageOne,
		StageTwo,
		StageThree,
		StageFour
	}

	public Stage m_CurrentStage;


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

	public void levelState()
	{
		switch(m_CurrentStage)
		{
			case Stage.StageOne:
			{
				sendEvent(ObeserverEvents.HaveSecondItem);
				break;
			}

			case Stage.StageTwo:
			{
				sendEvent(ObeserverEvents.SpokenToArmyMen);
				sendEvent(ObeserverEvents.HaveSecondItem);
				break;
			}

			case Stage.StageThree:
			{
				sendEvent(ObeserverEvents.HaveFoundPrivateRyan);
				sendEvent(ObeserverEvents.SpokenToArmyMen);
				sendEvent(ObeserverEvents.HaveSecondItem);
				break;
			}

			case Stage.StageFour:
			{
				sendEvent(ObeserverEvents.CanEnterTemple);
				sendEvent(ObeserverEvents.HaveFoundPrivateRyan);
				sendEvent(ObeserverEvents.SpokenToArmyMen);
				sendEvent(ObeserverEvents.HaveSecondItem);
				break;
			}
		}
	}

	public void nextLevelState()
	{
		m_CurrentStage += 1;
		levelState ();
	}



}
