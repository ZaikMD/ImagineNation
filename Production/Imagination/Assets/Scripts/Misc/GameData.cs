﻿using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{

	public static GameData Instance{ get; private set; }

	static int m_InstanceCounter = 0;
    int m_ID = int.MaxValue;
    public int ID
    {
        get { return m_ID; }
        private set { m_ID = value; }
    }

	void Awake()
	{
        ID = m_InstanceCounter++;

		//if theres another instance (there shouldnt be) destroy this
		if(Instance != null && Instance != this)
		{
            if (ID > Instance.ID)
            {
                //destroy all other instances
                Destroy(gameObject);
            }
            else
            {
                Destroy(Instance.gameObject);
            }
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(this.gameObject);
	}
	//================================================================================ 

    public PlayerInput m_PlayerOneInput;
    public PlayerInput m_PlayerTwoInput;

 	Characters m_PlayerOneCharacter = Characters.Zoe;
	public Characters PlayerOneCharacter
	{
		get{ return m_PlayerOneCharacter; }
		set
		{ 
			switch(value)
			{
			case Characters.Alex:
				if (m_PlayerTwoCharacter == value)
				{
					m_PlayerTwoCharacter = Characters.Derek;
				}
				break;
			case Characters.Derek:
				if (m_PlayerTwoCharacter == value)
				{
					m_PlayerTwoCharacter = Characters.Zoe;
				}
				break;
			case Characters.Zoe:
				if (m_PlayerTwoCharacter == value)
				{
					m_PlayerTwoCharacter = Characters.Alex;
				}
				break;
			}
			m_PlayerOneCharacter = value;
		}
	}

	Characters m_PlayerTwoCharacter = Characters.Alex;
	public Characters PlayerTwoCharacter
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

	Levels m_CurrentLevel = Levels.Level_1;
	Sections m_CurrentSection = Sections.Sections_1;
	CheckPoints m_CurrentCheckPoint = CheckPoints.CheckPoint_1;

	public Levels CurrentLevel
	{
		get {return m_CurrentLevel;}
		set 
		{
			m_CurrentLevel = value;
			m_CurrentSection = Sections.Sections_1;
			m_CurrentCheckPoint = CheckPoints.CheckPoint_1;
		}
	}

	public Sections CurrentSection
	{
		get { return m_CurrentSection;}
		set
		{
			m_CurrentSection = value;
			m_CurrentCheckPoint = CheckPoints.CheckPoint_1;
		}
	}

	public CheckPoints CurrentCheckPoint
	{
		get { return m_CurrentCheckPoint; }
		set 
		{
			if(value > m_CurrentCheckPoint)
			{
				m_CurrentCheckPoint = value;

			}
		}
	}

	void OnLevelWasLoaded(int level)
	{
		if(string.Compare(Application.loadedLevelName, "Game") == 0)
		{
			m_GameIsRunnging = true;
		}
	}
}
