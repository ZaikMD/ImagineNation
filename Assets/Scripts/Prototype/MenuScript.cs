using UnityEngine;
using System.Collections;

public enum MenuState
{ 
    MainMenu,
    SelectPlayerOne,
    SelectPlayerTwo,
    LoadGame,
    Options,
    ExitGame,
    PauseMenu,
    SaveGame,
    Credit,
    PlayingGame
}

public enum Player
{ 
    Alex,
    Derek,
    Zoey
}


public class MenuScript : MonoBehaviour , Observer
{

    public static MenuScript Instance { get; private set; }

  //  public Rigidbody playerOneInstance;
  //  public Rigidbody playerTwoInstance;

    GameObject player1;
    GameObject player2;

    public Camera m_MenuCamera;
    public Camera m_GameCamera;
    public MenuState m_MenuState;
    public bool m_MainMenu = true;
    public Player m_PlayerOne;
    string m_PlayerOneSelected;
    string m_PlayerTwoSelected;
    public Player m_PlayerTwo;
	bool firstTimePlayerTwoSelect;

    void Awake()
    {
        //if theres another instance (there shouldnt be) destroy it... there can be only one
        if (Instance != null && Instance != this)
        {
            //destroy all other instances
            Destroy(gameObject);
        }

        //set the instance
        Instance = this;

        //prevents this object being destroyed between scene loads
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        m_MenuState = MenuState.MainMenu;
		firstTimePlayerTwoSelect = true;
		GameManager.Instance.addObserver (this);
        m_MenuCamera.enabled = true;
        m_GameCamera.enabled = false;

        player1 = GameObject.Find("PlayerOnePrefab");
        player2 = GameObject.Find("PlayerTwoPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_PlayerOneSelected);
        Debug.Log(m_PlayerTwoSelected);

    }
     

    public void PauseMenu()
    {
        m_MenuState = MenuState.PauseMenu; 
   
    }

	public void ResumeGame()
	{
		m_MenuState = MenuState.PlayingGame;
       
	}

    void OnGUI()
    {

        GUI.skin.label.fontSize = 64;

        string buttonText = "NewGame";

        switch (m_MenuState)
        {
            case MenuState.PlayingGame:
                {
                    //TODO: set gamemanager to play game
                    GameManager.Instance.startGame();
                    m_MainMenu = false;
                    break;
                }

            case MenuState.MainMenu:
                {   
                    //New Game Button
                    Rect NewGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 4);
                    buttonText = "New Game";
                    if (GUI.Button(NewGamebuttonPosition, buttonText))
                    {
                        m_MenuState = MenuState.SelectPlayerOne;
                    }
                    //Load Game Button
                    buttonText = "Load Game";
                    Rect LoadButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 4);
                    if (GUI.Button(LoadButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.LoadGame;
                    }
                   

                    //Options button
                    Rect OptionButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 2, Screen.width / 2, Screen.height / 4);
                    buttonText = "Options";
                    if (GUI.Button(OptionButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.Options;
                    }

                    //Exit Game Button
                    Rect ExitButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 3), Screen.width / 2, Screen.height / 4);
                    buttonText = "Exit Game";
                    if (GUI.Button(ExitButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.ExitGame;
                    }

                    break;
                }
            case MenuState.SelectPlayerOne:
                {   
                    
                    Rect PlayerSelection = new Rect(0, 0, Screen.width / 4, Screen.height);
                    string playerselect = "PlayerOne:\n" ;
                    switch(m_PlayerOne)
                    {
                        case Player.Alex:
                            {
                                m_PlayerOneSelected = "Alex";
                                break;
                            }
                        case Player.Derek:
                            {
                                m_PlayerOneSelected = "Derek";
                                break;
                            }
                        case Player.Zoey:
                            {
                                m_PlayerOneSelected = "Zoey";
                                break;
                            }

                    }
                    
                    GUI.TextArea(PlayerSelection, playerselect + m_PlayerOneSelected);

                    //in here we select the characters for the level ahead.

                    //Alex
                    Rect AlexButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 5);
                    buttonText = "Alex";
                    if (GUI.Button(AlexButtonPosition, buttonText))
                    {
                        
                        m_PlayerOne = Player.Alex;
                    }

                    //Derek
                    Rect DerekButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5, Screen.width / 2, Screen.height / 5);
                    buttonText = "Derek";
                    if (GUI.Button(DerekButtonPosition, buttonText))
                    {
                        m_PlayerOne = Player.Derek;
                    }

                    //Zoey
                    Rect ZoeyButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 2), Screen.width / 2, Screen.height / 5);
                    buttonText = "Zoey";
                    if (GUI.Button(ZoeyButtonPosition, buttonText))
                    {
                        m_PlayerOne = Player.Zoey;
                    }

                    //Continue
                    Rect PlayButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 3), Screen.width / 2, Screen.height / 5);
                    buttonText = "Continue";
                    if (GUI.Button(PlayButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.SelectPlayerTwo;
                    }

                    //Back
                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.MainMenu;
                    }

                    break;
                    
                }

            case MenuState.SelectPlayerTwo:
                {
					if(firstTimePlayerTwoSelect)
					{
						if(m_PlayerOne == Player.Alex)
						{
							m_PlayerTwo = Player.Derek;
						}
						else
						{
							m_PlayerTwo = Player.Alex;
						}
					firstTimePlayerTwoSelect = false;
					}


                    Rect PlayerSelection = new Rect(0, 0, Screen.width / 4, Screen.height);
                    string playerSelectOne = "Player One:\n";
                    string PlayerSelectTwo = "\n Player Two: \n";
                    switch (m_PlayerTwo)
                    {
                        case Player.Alex:
                            {
                                m_PlayerTwoSelected = "Alex";
                                break;
                            }
                        case Player.Derek:
                            {
                                m_PlayerTwoSelected = "Derek";
                                break;
                            }
                        case Player.Zoey:
                            {
                                m_PlayerTwoSelected = "Zoey";
                                break;
                            }

                    }

                    GUI.TextArea(PlayerSelection, playerSelectOne + m_PlayerOneSelected + PlayerSelectTwo + m_PlayerTwoSelected);



                    //Alex
                    Rect AlexButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 5);
                    buttonText = "Alex";
                    if (GUI.Button(AlexButtonPosition, buttonText))
                    {
                        if (m_PlayerOne != Player.Alex)
                        {
                            m_PlayerTwo = Player.Alex;
                        }
                    }

                    //Derek
                    Rect DerekButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5, Screen.width / 2, Screen.height / 5);
                    buttonText = "Derek";
                    if (GUI.Button(DerekButtonPosition, buttonText))
                    {
                        if(m_PlayerOne != Player.Derek)
                        m_PlayerTwo = Player.Derek;
                    }

                    //Zoey
                    Rect ZoeyButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 2), Screen.width / 2, Screen.height / 5);
                    buttonText = "Zoey";
                    if (GUI.Button(ZoeyButtonPosition, buttonText))
                    {
                        if (m_PlayerOne != Player.Zoey)
                        m_PlayerTwo = Player.Zoey;
                    }

                    //Continue
                    Rect PlayButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 3), Screen.width / 2, Screen.height / 5);
                    buttonText = "Play";
                    if (GUI.Button(PlayButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.PlayingGame;
						firstTimePlayerTwoSelect = true;
						setPlayer();
                        m_MenuCamera.enabled = false;
                        m_GameCamera.enabled = true;
                    }

                    //Back
                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.SelectPlayerOne;
						firstTimePlayerTwoSelect = true;
                    }

                    break;
                }
           case MenuState.Options:
                {
                    //TODO: Add Options

                    Rect CreditsButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 3), Screen.width / 2, Screen.height / 5);
                    buttonText = "Credits";
                    if (GUI.Button(CreditsButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.Credit;                    
                    }

                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        if (m_MainMenu)
                        {
                            m_MenuState = MenuState.MainMenu;
                        }
                        else
                        {
                            m_MenuState = MenuState.PauseMenu;
                        }
                    }


                    break;
                }


            case MenuState.Credit:
                {

                    GUI.skin.label.fontSize = 124;

                    Rect CreditsButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5), Screen.width / 2, Screen.height - (Screen.height / 5 * 2));
                    buttonText = "Project Manager: Sean Donnely \n\nLead Designer: Adam Holloway \n\nLead Artist: Justin Lamoureux \nArtist: Luc Pitre \n\nLead Programmer: Kristoffer 'Kris' Matis \nProgrammer: Matthew WhitLaw \nProgrammer: Joe Burchill \nProgrammer: Zach Dubuc \nProgrammer: Kole Tackney \nProgrammer: Matt Elias \nProgrammer: Greg Fortier \nProgrammer: Jason Hein \n\nWith special thanks to Nick McNielly";
                    GUI.TextArea(CreditsButtonPosition, buttonText);
                   

                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        if (m_MainMenu)
                        {
                            m_MenuState = MenuState.MainMenu;
                        }
                        else
                        {
                            m_MenuState = MenuState.PauseMenu;
                        }
                    }
                    break;
                }
            case MenuState.LoadGame:
                {
                    //TODO: call load game

                    //save slot one
                    Rect SaveButtonOnePosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot One";
                    if (GUI.Button(SaveButtonOnePosition, buttonText))
                    {
                        //load game to Slot one
                        loadGame(1);
                        m_MenuState = MenuState.PlayingGame;
                        ResumeGame();
                    }

                    //save slot two
                    Rect SaveButtonTwoPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Two";
                    if (GUI.Button(SaveButtonTwoPosition, buttonText))
                    {
                        //load game to Slot Two
                        loadGame(2);
                        m_MenuState = MenuState.PlayingGame;
                    }
                    Rect SaveButtonThreePosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 2), Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Three";
                    if (GUI.Button(SaveButtonThreePosition, buttonText))
                    {
                        //load game to Slot Three
                        loadGame(3);
                        m_MenuState = MenuState.PlayingGame;
                    }

                    
                    //back to previous menu
                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 3), Screen.width / 2, Screen.height / 4);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        if (m_MainMenu)
                        {
                            m_MenuState = MenuState.MainMenu;
                        }
                        else
                        {
                            m_MenuState = MenuState.PauseMenu;
                        }
                    }
                    break;
                }
            case MenuState.ExitGame:
                {
                    //Exit the game
                    if (m_MainMenu)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        m_MenuState = MenuState.MainMenu;
                        m_MainMenu = true;
                    }
                    break;
                }
            case MenuState.PauseMenu:
                {
                    //TODO: tell gamemager to pause game


                    //Resume Game Button
                    Rect ResumeGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 5);
                    buttonText = "Resume Game";
                    if (GUI.Button(ResumeGamebuttonPosition, buttonText))
                    {
                        m_MenuState = MenuState.PlayingGame;
						//TODO set observer to play game;
						GameManager.Instance.startGame();
                    }

                    //Save Game Button
                    Rect NewGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4,  Screen.height / 5, Screen.width / 2, Screen.height / 5);
                    buttonText = "Save Game";
                    if (GUI.Button(NewGamebuttonPosition, buttonText))
                    {
                        m_MenuState = MenuState.SaveGame;
                    }
                    //Load Game Button
                    buttonText = "Load Game";
                    Rect LoadButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5 * 2, Screen.width / 2, Screen.height / 5);
                    if (GUI.Button(LoadButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.LoadGame;
                    }


                    //Options button
                    Rect OptionButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5 * 3, Screen.width / 2, Screen.height / 5);
                    buttonText = "Options";
                    if (GUI.Button(OptionButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.Options;
                    }

                    //Exit Game Button
                    Rect ExitButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
                    buttonText = "Exit Game";
                    if (GUI.Button(ExitButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.ExitGame;
                    }
                    break;
                }
            case MenuState.SaveGame:
                {
                    //TODO: call save game

                    //add save slots
                    Rect SaveButtonOnePosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot One";
                    if (GUI.Button(SaveButtonOnePosition, buttonText))
                    {
                       //save game to Slot one
                        saveGame(1);
                    }
                    Rect SaveButtonTwoPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4 , Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Two";
                    if (GUI.Button(SaveButtonTwoPosition, buttonText))
                    {
                        //save game to Slot Two
                        saveGame(2);
                    }
                    Rect SaveButtonThreePosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 2), Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Three";
                    if (GUI.Button(SaveButtonThreePosition, buttonText))
                    {
                        //save game to Slot Three
                        saveGame(3);
                    }

                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 3), Screen.width / 2, Screen.height / 4);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.PauseMenu;
                    }
                    break;
                }
           
        }


    }

	void setPlayer()
	{
		
        player1.name = m_PlayerOneSelected;
        player2.name = m_PlayerTwoSelected;

        
		switch(m_PlayerOne)
		{
			case Player.Alex:
				{
					player1.AddComponent<AlexPlayerState>();
					break;
				}
			case Player.Derek:
				{
					player1.AddComponent<DerekPlayerState>();
					break;
				}
			case Player.Zoey:
				{
					player1.AddComponent<ZoeyPlayerState>();
					break;
				}
		}

        player1.GetComponent<PlayerState>().m_CurrentPartner = player2;

		switch(m_PlayerTwo)
		{
			case Player.Alex:
				{

					player2.AddComponent<AlexPlayerState>();
					break;
				}
			case Player.Derek:
				{
					player2.AddComponent<DerekPlayerState>();
					break;
				}
			case Player.Zoey:
				{
					player2.AddComponent<ZoeyPlayerState>();
					break;
				}

		}
        player2.GetComponent<PlayerState>().m_CurrentPartner = player1;
	}
    	
    void saveGame(int slot)
    {
        PlayerPrefs.SetInt("PlayerOne" + slot, (int)m_PlayerOne);
        PlayerPrefs.SetInt("PlayerTwo" + slot, (int)m_PlayerTwo);
        PlayerPrefs.SetString("PlayerNameOne" + slot, m_PlayerOneSelected);
        PlayerPrefs.SetString("PlayerNameTwo" + slot, m_PlayerTwoSelected); 
    }

    void loadGame(int slot)
    {
     
        m_PlayerOne = (Player)PlayerPrefs.GetInt("PlayerOne" + slot);
      
        m_PlayerTwo = (Player)PlayerPrefs.GetInt("PlayerTwo" + slot);

        m_PlayerOneSelected = PlayerPrefs.GetString("PlayerNameOne" + slot);

        m_PlayerTwoSelected = PlayerPrefs.GetString("PlayerNameTwo" + slot);

        setPlayer();

     }


	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(!m_MainMenu)
		{
			if(recievedEvent == ObeserverEvents.PauseGame)
			{
				PauseMenu();
                m_MenuCamera.enabled = true;
                m_GameCamera.enabled = false;
			}
			else
			{
				ResumeGame();
                m_MenuCamera.enabled = false;
                m_GameCamera.enabled = true;
			}
		}
	}

 }
