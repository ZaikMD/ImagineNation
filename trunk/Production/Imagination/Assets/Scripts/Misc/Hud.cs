using UnityEngine;
using System.Collections;

/* Created by: kole
 * 
 * this class is will display all the hud elements to the player
 * when they are needed to be shown, such as how many collectables
 * has the player found and player health
 * 
 */


public class Hud : MonoBehaviour {

	//bool's to determine to show things
    bool m_ShowCheckPoint;
    bool m_ShowHiddenHud;
	bool m_ShowLightPegs;
	bool m_ShowLifes;
	bool m_ShowPuzzlePieces;

   	//Time for showing and hiding hud
    float m_HudDisplayLength;
    float m_HudDisplayTimer;
	float m_LightPegTimer;
	float m_PuzzlePieceTimer;
	float m_LifeTimer;
    float m_CheckPointDisplayTimer;

    //Varibles to display
    int LightPegCollected;
    int PuzzlePiecesCollected;
	float PlayerOneHealth;
	float PlayerTwoHealth;
	int NumberOfLives;

	//Images for our hud
    public Texture m_LightPegHudImage;
    public Texture[] m_PuzzlePieceHudImages;
    public Texture m_LifeCounterImage;
    public Texture m_CheckpointImage;
	public Texture m_LeftHealthBoarder;
	public Texture m_RightHealthBoarder;
	public Texture[] m_DerekHealthImages;
	public Texture[] m_AlexHealthImages;
	public Texture[] m_ZoeyHealthImages;


	Texture[] m_PlayerOneHealthImages;
	Texture[] m_PlayerTwoHealthImages;

    //Font for numbers
    public Font m_NumberFont;


    //Use this for initialization
	void Start ()
    {
        //Setting all varibles to desired starting stat
        m_HudDisplayLength = Constants.HUD_ON_SCREEN_TIME;
        m_HudDisplayTimer = 0;
        m_CheckPointDisplayTimer = 0;
		m_LightPegTimer = 0;
		m_PuzzlePieceTimer = 0;
		m_LifeTimer = 0;
        m_ShowCheckPoint = false;
        m_ShowHiddenHud = false;
		m_ShowLightPegs = false;
		m_ShowLifes = false;
		m_ShowPuzzlePieces = false;


		switch(GameData.Instance.PlayerOneCharacter)
		{
			case Characters.Alex:
			m_PlayerOneHealthImages = m_AlexHealthImages;
			break; 

			case Characters.Derek:
			m_PlayerOneHealthImages = m_DerekHealthImages;
			break;

			case Characters.Zoe:
			m_PlayerOneHealthImages = m_ZoeyHealthImages;
			break;
		}

		switch(GameData.Instance.PlayerTwoCharacter)
		{
			case Characters.Alex:
			m_PlayerTwoHealthImages = m_AlexHealthImages;
			break;

			case Characters.Derek:
			m_PlayerTwoHealthImages = m_DerekHealthImages;
			break;

			case Characters.Zoe:
			m_PlayerTwoHealthImages = m_ZoeyHealthImages;
			break;
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (PauseScreen.IsGamePaused) { return; }

		if(InputManager.getShowHud())
		{
			ShowHiddenHud();
		}

        if (m_HudDisplayTimer > 0)
        {
            m_HudDisplayTimer -= Time.deltaTime;
        }
        else
        {
            m_HudDisplayTimer = 0;
            m_ShowHiddenHud = false;
        }

        if (m_CheckPointDisplayTimer > 0)
        {
            m_CheckPointDisplayTimer -= Time.deltaTime;
        }
        else
        {
            m_CheckPointDisplayTimer = 0;
            m_ShowCheckPoint = false;
        }

		if (m_LightPegTimer > 0)
		{
			m_LightPegTimer -= Time.deltaTime;
		}
		else
		{
			m_LightPegTimer = 0;
			m_ShowLightPegs = false;
		}

		if (m_PuzzlePieceTimer > 0)
		{
			m_PuzzlePieceTimer -= Time.deltaTime;
		}
		else
		{
			m_PuzzlePieceTimer = 0;
			m_ShowPuzzlePieces = false;
		}

		if (m_LifeTimer > 0)
		{
			m_LifeTimer -= Time.deltaTime;
		}
		else
		{
			m_LifeTimer = 0;
			m_ShowLifes = false;
		}


	}

	public void SetHealth(float Health, int Player)
	{
		if(Player == 1)
		{
			PlayerOneHealth = Health;
		}
		else
		{
			PlayerTwoHealth = Health;
		}

	}

    public void ShowCheckpoint()
    {
        m_ShowCheckPoint = true;
        m_CheckPointDisplayTimer = m_HudDisplayLength;
    }

	public void ShowLightPegs()
	{
		m_ShowLightPegs = true;
		m_LightPegTimer = m_HudDisplayLength;
	}

	public void ShowPuzzlePieces()
	{
		m_ShowPuzzlePieces = true;
		m_PuzzlePieceTimer = m_HudDisplayLength;

		PuzzlePiecesCollected = GameData.Instance.CalcPuzzlePieces();
	}

	public void ShowLifes()
	{
		m_ShowLifes = true;
		m_LifeTimer = m_HudDisplayLength;		
	}

	public void GetNumberOfLifes()
	{
		NumberOfLives = GameData.Instance.GetLivesRemaining();
	}

	public void ShowHiddenHud()
    {
		m_ShowLightPegs = false;
		m_ShowLifes = false;
		m_ShowPuzzlePieces = false;
        m_ShowHiddenHud = true;
        m_HudDisplayTimer = m_HudDisplayLength;
    }

    public void UpdateLightPegs(int NumberOfLightPegs)
    {
        LightPegCollected = NumberOfLightPegs;
		ShowLightPegs();
    }

    //All our graphics have to be done in on gui
    void OnGUI()
    {
		if (PauseScreen.IsGamePaused) { return; }


		float SizeOfHudElements = Screen.width / 10;
		Rect PositionRect = new Rect(0, 0, SizeOfHudElements, SizeOfHudElements);
		//Need to fix the custom fonts.
		//GUI.skin.font = m_NumberFont;
		//Hidden hud elements such as collectables and Lives remaining
        if(m_ShowHiddenHud)
        {
			GetNumberOfLifes();

			//this will scale our font to the approximite size
			GUI.skin.label.fontSize = Screen.width/12;

            //Light Pegs
         	GUI.DrawTexture(PositionRect, m_LightPegHudImage);

            PositionRect.Set(PositionRect.width, 0, SizeOfHudElements, SizeOfHudElements);
            if (LightPegCollected < 10)
            {
                GUI.Label(PositionRect, "0" + LightPegCollected.ToString());
            }
            else
            {
                GUI.Label(PositionRect, LightPegCollected.ToString());
            }

// no Puzzle piece texture yet
			//PuzzlePieces
			PositionRect.Set( Screen.width /2 - SizeOfHudElements, 0, SizeOfHudElements * 2, SizeOfHudElements);
			GUI.DrawTexture(PositionRect, m_PuzzlePieceHudImages[PuzzlePiecesCollected]);

			//Life Counter image
			PositionRect.Set(Screen.width - SizeOfHudElements, 0, SizeOfHudElements, SizeOfHudElements);
			GUI.DrawTexture(PositionRect, m_LifeCounterImage);

			//Number repersenting lives remaining;
			PositionRect.Set(Screen.width - SizeOfHudElements * 2, 0, SizeOfHudElements, SizeOfHudElements);
			if(NumberOfLives < 10)
			{
				//No Lifes implemented yet
				GUI.Label(PositionRect, "0" + NumberOfLives.ToString());
			}
			else
			{
				//No Lives imlemented yet
				GUI.Label(PositionRect, NumberOfLives.ToString());
			}
        }

		//Light Pegs by themselves
		if(m_ShowLightPegs)
		{
			GUI.skin.label.fontSize = Screen.width/12;

			PositionRect.Set(0, 0, SizeOfHudElements, SizeOfHudElements);
			GUI.DrawTexture(PositionRect, m_LightPegHudImage);

			PositionRect.Set(PositionRect.width, 0, SizeOfHudElements, SizeOfHudElements);
			if (LightPegCollected < 10)
			{
				GUI.Label(PositionRect, "0" + LightPegCollected.ToString());
			}
			else
			{
				GUI.Label(PositionRect, LightPegCollected.ToString());
			}
		}

		if(m_ShowLifes)
		{
			GetNumberOfLifes();

			//Life Counter image
			PositionRect.Set(Screen.width - SizeOfHudElements, 0, SizeOfHudElements, SizeOfHudElements);
			GUI.DrawTexture(PositionRect, m_LifeCounterImage);
			
			//Number repersenting lives remaining;
			PositionRect.Set(Screen.width - SizeOfHudElements * 2, 0, SizeOfHudElements, SizeOfHudElements);
			if(NumberOfLives < 10)
			{
				//No Lifes implemented yet
				GUI.Label(PositionRect, "0" + NumberOfLives.ToString());
			}
			else
			{
				//No Lives imlemented yet
				GUI.Label(PositionRect, NumberOfLives.ToString());
			}
		}

		if(m_ShowPuzzlePieces)
		{
			// no Puzzle piece texture yet
			//PuzzlePieces
			PositionRect.Set( Screen.width /2 - SizeOfHudElements, 0, SizeOfHudElements * 2, SizeOfHudElements);
			GUI.DrawTexture(PositionRect, m_PuzzlePieceHudImages[PuzzlePiecesCollected]);
		}

		//CheckPoints
		if(m_ShowCheckPoint)
		{
			PositionRect.Set (Screen.width/4, Screen.height/4, Screen.width/ 2 , Screen.height / 8);
			GUI.DrawTexture(PositionRect, m_CheckpointImage);
		}

		//Health
		PositionRect.Set (0, Screen.height - SizeOfHudElements, SizeOfHudElements, SizeOfHudElements);

		GUI.DrawTexture(PositionRect, m_PlayerOneHealthImages[(int)PlayerOneHealth]);
		GUI.DrawTexture (PositionRect, m_LeftHealthBoarder);

		PositionRect.Set (Screen.width - SizeOfHudElements, Screen.height - SizeOfHudElements, SizeOfHudElements, SizeOfHudElements);

		GUI.DrawTexture(PositionRect, m_PlayerTwoHealthImages[(int)PlayerTwoHealth]);
		GUI.DrawTexture (PositionRect, m_RightHealthBoarder);
    }
}
