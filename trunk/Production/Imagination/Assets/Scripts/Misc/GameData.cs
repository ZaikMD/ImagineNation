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
* 
* 30/10/2014 Edit: Kole - Added collectable code, so collectables would reset right 
* 
* 26/11/2014 Edit: Kole - Added a varible to save and load for puzzle pieces.
*/
#endregion

using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{

	public static GameData Instance{ get; private set; }
    
    //ID is used to ensure that the older GamaData is kept in the case of duplicates
	static int m_InstanceCounter = 0;
    public int m_ID = int.MaxValue;
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
				return;
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
	short m_NumberOfLevels = Constants.NUMBER_OF_LEVELS;
	Levels m_CurrentLevel = Levels.Level_1;
	Sections m_CurrentSection = Sections.Sections_1;
	CheckPoints m_CurrentCheckPoint = CheckPoints.CheckPoint_1;
    bool m_FirstTimePlayingLevel = true; 

	//========================================
	//settings
	//cant make a vector2 const
	private Vector2 m_DefaultCameraRotationScale = new Vector2 (-2.5f, -1.5f);
	float m_CameraRotationScaleModifyer = 1.0f;
	public float CameraRotationScaleModifyer
	{
		get { return m_CameraRotationScaleModifyer;}
		set 
		{ 
			m_CameraRotationScaleModifyer = value;
			for(int i = 0; i < TPCamera.Cameras.Count; i++)
			{
				TPCamera.Cameras[i].RotationScale = new Vector2(m_DefaultCameraRotationScale.x * m_CameraRotationScaleModifyer,
				                                                m_DefaultCameraRotationScale.y * m_CameraRotationScaleModifyer);
			}
		}
	}

	public Material i_Brightness;
	float m_Brightness = 1.0f;
	public float Brightness
	{
		get{ return m_Brightness;}
		set
		{ 
			m_Brightness = value;
			if(i_Brightness != null)
				i_Brightness.SetFloat(Constants.BRIGHTNESS_PROPERTY_NAME, 1.0f * m_Brightness);
		}
	}

	//========================================

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

	//when saving and loading happens, this needs to be loaded in
	int m_Lives = Constants.LIVE_INITIAL_COUNT;

	public int CurrentLives
	{
		get { return m_Lives; }

		set { m_Lives = value;}
	}

	void Start()
	{
		Brightness = Constants.BRIGHTNESS_DEFAULT;
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
	//the first element will be level, second section, and the will be the puzzle piece in question. 
	short[][][] m_PuzzlePieceCollectedInLevel; // puzzle pieces will be treated like a bool.

    public bool[] CollectedLightPegs()
    {
        return m_LightPegsCollectedInLevel;
    }

	public short[] CollectedPuzzlePiece()
	{
		return m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection];
	}

    public void SetCollectedPegs(int length)
    {
        m_LightPegsCollectedInLevel = new bool[length];

        for (int i = 0; i < length; i++)
        {
            m_LightPegsCollectedInLevel[i] = false;   
        }
    }

	void intianlizePuzzlePieceArray()
	{
		m_PuzzlePieceCollectedInLevel = new short[1][][];
		for(int i = 0; i < (int)Levels.Count; i++)
		{
			m_PuzzlePieceCollectedInLevel[i] = new short[3][];
			for(int n = 0; n < (int)Sections.Count; n++)
			{
				m_PuzzlePieceCollectedInLevel[i][n] = new short[2];

				for(int x = 0; x < 2; x++)
				{
					m_PuzzlePieceCollectedInLevel[i][n][x] = 0;
				}
			}
		}	
	}


	public void SetCollectedPuzzlePieces(bool[] m_PuzzlePeiceCollected)
	{
		//If m_PuzzlePeicesCollectedInLevel is not initialized
		if(m_PuzzlePieceCollectedInLevel == null)
		{
			intianlizePuzzlePieceArray();
		}

		for (int i = 0; i > m_PuzzlePeiceCollected.Length; i++)
		{
			if(m_PuzzlePeiceCollected[i])
			{
				m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][i] = 1;   
			}
			else
			{
				m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][i] = 0;
			}
		}
	}

	public short[] GetCollectedPuzzlePeices()
	{
		if(m_PuzzlePieceCollectedInLevel == null)
		{
			intianlizePuzzlePieceArray();
		}

		if(m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][1] != null)
		{
			short[] temp = new short[2];
			temp[0] = m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][0];
			temp[1] = m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][1];
			return temp;	
		}
		else
		{
			bool[] newBool = new bool[2];
			for(int i = 0; i > newBool.Length; i++)
			{
				newBool[i] = false;
			}

			SetCollectedPuzzlePieces(newBool);
			short[] temp = new short[2];
			temp[0] = m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][0];
			temp[1] = m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][1];
			return temp;	
		}
	}

    public void LightPegCollected(int ID)
    {
		m_LightPegsCollectedInLevel[ID] = true;
    }

	public void ResetCollectedPeg(int id)
	{
		m_LightPegsCollectedInLevel[id] = false;
	}

	public void PuzzlePieceCollected(int ID)
	{
		m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][ID] = 1; 
	}
	
	public void ResetPuzzlePiece(int id)
	{
		m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][(int)m_CurrentSection][ID] = 0;
	}


	public void IncrementLives()
	{
		if(m_Lives < Constants.LIVES_MAX)
			m_Lives++;
		//Update hud
	}

	public void DecrementLives()
	{
		m_Lives--;
		//update hud
	}

	public void ResetLives()
	{
		m_Lives = Constants.LIVE_INITIAL_COUNT;
		//update hud
	}

	public int GetLivesRemaining()
	{
		return m_Lives;
	}
}
