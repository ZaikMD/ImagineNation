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
* 
* 28/11/2014 Edit: Greg- Added live functionality 
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
    float m_PlayerOneCameraRotationScaleModifyer = 1.0f;

    Vector2 PlayerOneInvert = new Vector2(1.0f, 1.0f);
    Vector2 PlayerTwoInvert = new Vector2(1.0f, 1.0f);

    public float PlayerOneCameraRotationScaleModifyer
	{
        get { return m_PlayerOneCameraRotationScaleModifyer; }
		set 
		{
            m_PlayerOneCameraRotationScaleModifyer = value;
            updatePlayerOneCameraRotationSpeed();
         }
	}

    float m_PlayerTwoCameraRotationScaleModifyer = 1.0f;
    public float PlayerTwoCameraRotationScaleModifyer
    {
		get {return m_PlayerTwoCameraRotationScaleModifyer; }
        set
        {
            m_PlayerTwoCameraRotationScaleModifyer = value;
            updatePlayerTwoCameraRotationSpeed();
        }
    }

    public void invertPlayerOneX()
    {
        PlayerOneInvert.x *= -1.0f;
        updatePlayerOneCameraRotationSpeed();
    }

    public void invertPlayerOneY()
    {
        PlayerOneInvert.y *= -1.0f;
        updatePlayerOneCameraRotationSpeed();
    }

    public void invertPlayerTwoX()
    {
        PlayerTwoInvert.x *= -1.0f;
        updatePlayerTwoCameraRotationSpeed();
    }

    public void invertPlayerTwoY()
    {
        PlayerTwoInvert.y *= -1.0f;
        updatePlayerTwoCameraRotationSpeed();
    }

    public void updateCameraRotationSpeeds()
    {
        updatePlayerOneCameraRotationSpeed();
        updatePlayerTwoCameraRotationSpeed();
    }

    void updatePlayerOneCameraRotationSpeed()
    {
		PlayerInfo test = PlayerInfo.getPlayer (Players.PlayerOne);

        PlayerInfo.getPlayer(Players.PlayerOne).m_PlayerCamera.RotationScale = new Vector2(m_DefaultCameraRotationScale.x * m_PlayerOneCameraRotationScaleModifyer * PlayerOneInvert.x,
                                                                                           m_DefaultCameraRotationScale.y * m_PlayerOneCameraRotationScaleModifyer * PlayerOneInvert.y);
		
    }

    void updatePlayerTwoCameraRotationSpeed()
    {
        PlayerInfo.getPlayer(Players.PlayerTwo).m_PlayerCamera.RotationScale = new Vector2(m_DefaultCameraRotationScale.x * m_PlayerTwoCameraRotationScaleModifyer * PlayerTwoInvert.x,
                                                                                          m_DefaultCameraRotationScale.y * m_PlayerTwoCameraRotationScaleModifyer * PlayerTwoInvert.y);
        
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
            m_LightPegsCollectedSinceLastCheckPoint = 0;
		}
	}

	public Sections CurrentSection
	{
		get { return m_CurrentSection;}
		set
		{
			m_CurrentSection = value;
			m_CurrentCheckPoint = CheckPoints.CheckPoint_1;
            m_LightPegsCollectedSinceLastCheckPoint = 0;
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
                m_LightPegsCollectedSinceLastCheckPoint = 0;
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

    int m_LightPegsCollectedSinceLastCheckPoint = 0;
    static int m_TotalLightPegCount = 0;
    static bool[] m_LightPegsCollectedInLevel;
	//the first element will be level, second section, and the will be the puzzle piece in question. 
	static short[][][] m_PuzzlePieceCollectedInLevel; // puzzle pieces will be treated like a bool.

    public int TotalLightPegs()
    {
        return m_TotalLightPegCount;
    }

    public void incrementTotalLightPegs()
    {
        m_TotalLightPegCount++;
        m_LightPegsCollectedSinceLastCheckPoint++;
    }

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
        //the first array is our level.
		m_PuzzlePieceCollectedInLevel = new short[(int)Levels.Count][][];
		for(int i = 0; i < (int)Levels.Count; i++)
		{
            //our second array is our section
			m_PuzzlePieceCollectedInLevel[i] = new short[(int)Sections.Count][];
			for(int n = 0; n < (int)Sections.Count; n++)
			{
                //our third is which puzzle piece
                //There is only ever two puzzle pieces per level
				m_PuzzlePieceCollectedInLevel[i][n] = new short[2];

                //If we are initializing, this has just started, so it is false.
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

	public int CalcPuzzlePieces()
	{
		int PuzzlePiecesCollected = 0;
	
		for(int i = 0; i < 3; i++)
		{
			for(int n = 0; n < 2; n++)
			{
				PuzzlePiecesCollected += m_PuzzlePieceCollectedInLevel[(int)m_CurrentLevel][i][n];
			}
		}
		return PuzzlePiecesCollected;
	}

	public void IncrementLives()
	{
		if(m_Lives < Constants.LIVES_MAX)
			m_Lives++;

        m_TotalLightPegCount -= Constants.LIGHT_PEGS_NEEDED_TO_GAIN_LIVES;
		//Update hud
	}

	public void DecrementLives()
	{
		m_Lives--;
        m_TotalLightPegCount -= m_LightPegsCollectedSinceLastCheckPoint;
        m_LightPegsCollectedSinceLastCheckPoint = 0;
		if(m_Lives <= 0)
		{
			ResetLives();
			Application.LoadLevel(Constants.GAME_OVER_SCREEN);
			resetCheckPoint();
		}
		else
		{
			Application.LoadLevel(Application.loadedLevelName); //If both players are dead, reset the scene
		}
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
