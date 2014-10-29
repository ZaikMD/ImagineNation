/*
*GameData
*
*resposible for holding game settings and player data IE input type
*
*decides whether or not to update a check point when a check point requests it
*
*chenges player ones character and sets player twos so they dont match
*
*decides whether or not to change player two character based off player ones character
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 17/10/2014 Edit: Zach Dubuc - Added in a reset checkpoint Function
*/
#endregion

using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{

	public static GameData Instance{ get; private set; }
    
    //ID is used to ensure that the older GamaData is kept in the case of duplicates
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
	//the set ensures that playerTwo is not set to the same value as player one
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
	//only changes the setting if its not what player two is set to
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

	//eventually to be used for pausing
	public bool m_GameIsRunnging = false;

	//current level, section, and checkpoint
	Levels m_CurrentLevel = Levels.Level_1;
	Sections m_CurrentSection = Sections.Sections_1;
	CheckPoints m_CurrentCheckPoint = CheckPoints.CheckPoint_1;
    bool m_FirstTimePlayingLevel = false; 

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

	public void resetCheckPoint()
	{
		m_CurrentCheckPoint = CheckPoints.CheckPoint_1;
	}

    //Collectable info
    public bool FirstTimePlayingLevel
    {
        get { return m_FirstTimePlayingLevel; }
        set
        {
            m_FirstTimePlayingLevel = value;
        }
    }
    
    bool[] m_LightPegsCollectedInLevel;

    public bool[] CollectedLightPegs()
    {
        return m_LightPegsCollectedInLevel;
    }

    public void SetCollectedPegs(int length)
    {
        m_LightPegsCollectedInLevel = new bool[length];

        for (int i = 0; i < length; i++)
        {
            m_LightPegsCollectedInLevel[i] = false;   
        }
    }

    public void LightPegCollected(int ID)
    {
        m_LightPegsCollectedInLevel[ID] = true;
    }

}
