using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/// <summary>
/// the stage enum is used to keep track things like if a door is open or not,
/// or if we have second item.
/// </summary>
	public enum Stage
	{
        StartStage,
		StageOne,
		StageTwo,
		StageThree,
		StageFour
	}


/// <summary>
/// anything with an update should observe this for noe all it sends if events when the game is paused/ unpaused
/// </summary>
public class GameManager : Subject , Observer
{

	public static GameManager Instance{ get; private set; }

	bool m_IsPaused = false;


	public Stage m_CurrentStage = Stage.StageOne;

	List <GameObject> m_StageUpdaters = new List<GameObject>();

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

	void Start()
	{
		GameObject sender;
		sender = GameObject.FindGameObjectWithTag ("quiggs");
		if(sender!= null)
		{
			sender.GetComponent<NPC> ().addObserver (this);
			m_StageUpdaters.Add (sender);
		}

		sender = GameObject.FindGameObjectWithTag ("Brian");
		if(sender!= null)
		{
			sender.GetComponent<NPC> ().addObserver (this);
			m_StageUpdaters.Add (sender);
		}

		//TODO: add aditional stage updaters

		m_CurrentStage = Stage.StageTwo;
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

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		for(int i = 0; i < m_StageUpdaters.Count; i++)
		{
			if(sender.gameObject == m_StageUpdaters[i])
			{
				nextLevelState();
				m_StageUpdaters.RemoveAt(i);
				break;
			}
		}
	}
}
