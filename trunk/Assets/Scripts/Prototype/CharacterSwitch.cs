using UnityEngine;
using System.Collections;

/// <summary>
/// sends a switch event when the active player state machine says to switch
/// </summary>

public class CharacterSwitch : Subject 
{
	public static CharacterSwitch Instance{ get; private set; }
	// Use this for initialization

	//player ai needs to activly set this
	bool m_AbleToSwitch = true;

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
			if(!RespawnManager.Instance.getIsAnyPlayerDead())
			{
				m_AbleToSwitch = true;
			}

		}

		if(RespawnManager.Instance.getIsAnyPlayerDead())
		{
			m_AbleToSwitch = false;
		}

	}




	public void switchCharacters()
	{
		if(m_AbleToSwitch)
		{
			m_AbleToSwitch = false;
			SoundManager.Instance.playSound(Sounds.CharacterSwitch, this.transform.position);
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
