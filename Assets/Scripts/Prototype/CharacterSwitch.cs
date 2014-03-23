using UnityEngine;
using System.Collections;

public class CharacterSwitch : Subject 
{
	public static CharacterSwitch Instance{ get; private set; }
	// Use this for initialization
	
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy it... there can be only one
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

	//player ai needs to activly set this
	public bool AbleToSwitch { public get; public set; }

	public void switchCharacters()
	{
		if(AbleToSwitch)
		{
			AbleToSwitch = false;
			sendEvent(ObeserverEvents.CharacterSwitch);
		}
	}
}
