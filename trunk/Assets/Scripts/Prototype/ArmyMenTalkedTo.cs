using UnityEngine;
using System.Collections;

public class ArmyMenTalkedTo : Subject, Observer
{
	public static ArmyMenTalkedTo Instance{ get; private set; }
	
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

	public void Start()
	{
	GameManager.Instance.addObserver (this);
	}

	public void addAsObsever()
	{

	//	GameManager.Instance.addObserver (this);
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.SpokenToArmyMen)
		{
			sendEvent(ObeserverEvents.Used);
		}
	}
}
