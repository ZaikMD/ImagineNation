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
    PlayingGame
}

public enum Player
{ 
    Alex,
    Derek,
    Zoey
}


public class MenuScript : MonoBehaviour
{

    public static MenuScript Instance { get; private set; }


    public MenuState m_MenuState;
    public bool m_MainMenu = true;
    public Player m_PlayerOne;
    string m_PlayerOneSelected;
    string m_PlayerTwoSelected;
    public Player m_PlayerTwo;

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
    }

    // Update is called once per frame
    void Update()
    {



    }
     

    public void PauseMenu()
    {
        m_MenuState = MenuState.PauseMenu;    
    }

    void OnGUI()
    {

        
        string buttonText = "NewGame";

        switch (m_MenuState)
        {
            case MenuState.PlayingGame:
                {
                    //TODO: set gamemanager to play game

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
                    Rect PlayerSelection = new Rect(0, 0, Screen.width / 4, Screen.height);
                    string playerSelectOne = "Player One:\n";
                    string PlayerSelectTwo = "\n Player Two: \n ";
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
                    }

                    //Back
                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
                    buttonText = "Back";
                    if (GUI.Button(BackButtonPosition, buttonText))
                    {
                        m_MenuState = MenuState.SelectPlayerOne;
                    }

                    break;
                }
           case MenuState.Options:
                {
                    //TODO: Add Options

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
                    }

                    //save slot two
                    Rect SaveButtonTwoPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Two";
                    if (GUI.Button(SaveButtonTwoPosition, buttonText))
                    {
                        //load game to Slot Two
                    }
                    Rect SaveButtonThreePosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 2), Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Three";
                    if (GUI.Button(SaveButtonThreePosition, buttonText))
                    {
                        //load game to Slot Three
                    }

                    
                    //back to previous menu
                    Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 4);
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
                    }
                    Rect SaveButtonTwoPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4 , Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Two";
                    if (GUI.Button(SaveButtonTwoPosition, buttonText))
                    {
                        //save game to Slot Two
                    }
                    Rect SaveButtonThreePosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 2), Screen.width / 2, Screen.height / 4);
                    buttonText = "Slot Three";
                    if (GUI.Button(SaveButtonThreePosition, buttonText))
                    {
                        //save game to Slot Three
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


   

}
