using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public enum MenuState
    { 
        MainMenu,
        SelectPlayerOne,
        SelectPlayerTwo,
        LoadGame,
        Options,
        ExitGame,
        Credit,  
    }

    public MenuState m_MenuState;
    public bool m_MainMenu = true;
    public Player m_PlayerOne;
    public Player m_PlayerTwo;
    string m_PlayerOneSelected = null;
    string m_PlayerTwoSelected = null;
    
	bool firstTimePlayerTwoSelect;


    void OnGUI()
    {
        if (m_MainMenu)
        {

            GUI.skin.label.fontSize = 64;

            string buttonText = "NewGame";

            switch (m_MenuState)
            {

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
                        string playerselect = "PlayerOne:\n";
                        switch (m_PlayerOne)
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
                        if (firstTimePlayerTwoSelect)
                        {
                            if (m_PlayerOne == Player.Alex)
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
                            if (m_PlayerOne != Player.Derek)
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

                        //Play
                        Rect PlayButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 3), Screen.width / 2, Screen.height / 5);
                        buttonText = "Play";
                        if (GUI.Button(PlayButtonPosition, buttonText))
                        {
                            firstTimePlayerTwoSelect = true;
                            setPlayer();
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
                            m_MenuState = MenuState.MainMenu;
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
                            m_MenuState = MenuState.MainMenu;
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
                        }

                        //save slot two
                        Rect SaveButtonTwoPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 4);
                        buttonText = "Slot Two";
                        if (GUI.Button(SaveButtonTwoPosition, buttonText))
                        {
                            //load game to Slot Two
                            loadGame(2);

                        }
                        Rect SaveButtonThreePosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 2), Screen.width / 2, Screen.height / 4);
                        buttonText = "Slot Three";
                        if (GUI.Button(SaveButtonThreePosition, buttonText))
                        {
                            //load game to Slot Three
                            loadGame(3);
                        }

                        //back to previous menu
                        Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 4 * 3), Screen.width / 2, Screen.height / 4);
                        buttonText = "Back";
                        if (GUI.Button(BackButtonPosition, buttonText))
                        {
                            m_MenuState = MenuState.MainMenu;
                        }
                        break;
                    }
                case MenuState.ExitGame:
                    {
                        //Exit the Game
                        Application.Quit();
                        break;
                    }



            }
        }
    }
        void loadGame(int slot)
    {
        m_PlayerOne = (Player)PlayerPrefs.GetInt("PlayerOne" + slot);
      
        m_PlayerTwo = (Player)PlayerPrefs.GetInt("PlayerTwo" + slot);

        m_PlayerOneSelected = PlayerPrefs.GetString("PlayerNameOne" + slot);

        m_PlayerTwoSelected = PlayerPrefs.GetString("PlayerNameTwo" + slot);

        if (m_PlayerOneSelected != null)
        {
            setPlayer();
        }
     }


        void setPlayer()
    {
        Setting.Instance.m_PlayerOneSelected = m_PlayerOne;
        Setting.Instance.m_PlayerTwoSelected = m_PlayerTwo;
        Application.LoadLevel("Kole -TestScene 2");
    }


}
