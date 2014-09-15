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

	Enums.Characters m_PlayerOneCharacter = Enums.Characters.Zoey;
	public Enums.Characters PlayerOneCharacter
	{
		get{ return m_PlayerOneCharacter; }
		set
		{ 
			switch(value)
			{
			case Enums.Characters.Alex:
				if (m_PlayerTwoCharacter == value)
				{
					m_PlayerTwoCharacter = Enums.Characters.Derek;
				}
				break;
			case Enums.Characters.Derek:
				if (m_PlayerTwoCharacter == value)
				{
					m_PlayerTwoCharacter = Enums.Characters.Zoey;
				}
				break;
			case Enums.Characters.Zoey:
				if (m_PlayerTwoCharacter == value)
				{
					m_PlayerTwoCharacter = Enums.Characters.Alex;
				}
				break;
			}
			m_PlayerOneCharacter = value;
		}
	}

	Enums.Characters m_PlayerTwoCharacter = Enums.Characters.Alex;
	public Enums.Characters PlayerTwoCharacter
	{
		get{ return m_PlayerTwoCharacter; }
		set
		{ 
			if (m_PlayerOneCharacter != value)
			{
				m_PlayerTwoCharacter = value;
			}
		}
	}

	public bool m_GameIsRunnging = false;

	void OnLevelWasLoaded(int level)
	{
		if(string.Compare(Application.loadedLevelName, "Game") == 0)
		{
			m_GameIsRunnging = true;
		}
	}
}
