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

		//---------------------------------- 
	}


	void Update()
	{
		if(GameObject.FindGameObjectWithTag ("Camera").GetComponentInChildren<CameraController> ().isAbleToSwitch())
		{
			m_AbleToSwitch = true;
		}
	}


	//player ai needs to activly set this
	bool m_AbleToSwitch = true;

	public void switchCharacters()
	{
		if(m_AbleToSwitch)
		{
			m_AbleToSwitch = false;
			sendEvent(ObeserverEvents.CharacterSwitch);
		}
	}

	public void setIfAbleToSwitch(bool isAbleToSwitch)
	{
		m_AbleToSwitch = isAbleToSwitch;
	}

	public bool getIfAbleToSwitch()
	{
		return m_AbleToSwitch;
	}
}
