using UnityEngine;
using System.Collections;

public class BrianTalkedTo : Subject, Observer 
{

	public static BrianTalkedTo Instance{ get; private set; }
	
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
	
	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(recievedEvent == ObeserverEvents.HaveFoundPrivateRyan)
		{
			sendEvent(ObeserverEvents.Used);
		}
	}
}
