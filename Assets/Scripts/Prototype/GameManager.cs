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
				ArmyMenTalkedTo.Instance.addAsObsever();
			}

			
			sender = GameObject.FindGameObjectWithTag ("Brian");
			if(sender!= null)
			{
				sender.GetComponent<NPC> ().addObserver (this);
				m_StageUpdaters.Add (sender);
				BrianTalkedTo.Instance.addAsObsever();
			}
		
		//TODO: add aditional stage updaters

		m_CurrentStage = (Stage)PlayerPrefs.GetInt("CurrentLevelStage");
		//levelState ();

		if (m_CurrentStage == Stage.StageThree)
		{
			Invoke ("spoken", 3);	
		}

		if(m_CurrentStage == Stage.StageFour)
		{
			Invoke("foundBrian", 3);
			Invoke ("spoken", 3);	
		}
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


	public void setUpSenders(int numberOfSetup)
	{	
		GameObject sender;

		switch(numberOfSetup)
		{
			case 1:
			{	
			sender = GameObject.FindGameObjectWithTag ("Brian");
			if(sender!= null)
			{
				sender.GetComponent<NPC> ().addObserver (this);
				m_StageUpdaters.Add (sender);
			}
		
			break;
			}

			case 2:
			{
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
				break;
			}

			default:
			{
				break;
			}
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

	public void spoken()
	{
		sendEvent (ObeserverEvents.SpokenToArmyMen);
	}
	public void foundBrian()
	{
		sendEvent(ObeserverEvents.HaveFoundPrivateRyan);
	}
}
