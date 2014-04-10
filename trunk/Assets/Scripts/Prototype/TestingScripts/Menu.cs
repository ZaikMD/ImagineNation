using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public enum MenuState
    { 
        MainMenu,
        SelectPlayerOne,
        SelectPlayerTwo,
        LoadGame,
		SelectLevel,
		SelectCheckPoint,
		SelectLevelSelectPlayerOne,
		SelectLevelSelectPlayerTwo,
        Options,
        ExitGame,
        Credit,  
    }

	public enum Level
	{
		LevelOnePartOne,
		LevelOnePartTwo,
		LevelOnePartThree,
		LevelOnePartFour,
		LevelOnePartFive,
		LevelOnePartSix,
		LevelOnePartSeven
	}

	public Level m_CurrentLevel;
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
						Screen.showCursor = true;

                        //New Game Button
                        Rect NewGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 5);
                        buttonText = "New Game";
                        if (GUI.Button(NewGamebuttonPosition, buttonText))
                        {
                            m_MenuState = MenuState.SelectPlayerOne;
                        }
                        //Load Game Button
                        buttonText = "Load Game";
                        Rect LoadButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5, Screen.width / 2, Screen.height / 5);
                        if (GUI.Button(LoadButtonPosition, buttonText))
                        {
                            m_MenuState = MenuState.LoadGame;
							
                        }
						
						buttonText = "Free Play";
						Rect SelectButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5 * 2, Screen.width / 2, Screen.height / 5);
						if (GUI.Button(SelectButtonPosition, buttonText))
						{
							m_MenuState = MenuState.SelectLevel;
						}

                        //Options button
                        Rect OptionButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 5*3, Screen.width / 2, Screen.height / 5);
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

				case MenuState.SelectLevel:
					{
						//New Game Button
						Rect NewGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 6);
						buttonText = "Level One";
						if (GUI.Button(NewGamebuttonPosition, buttonText))
						{
							m_CurrentLevel = Level.LevelOnePartOne;
						//	m_MenuState = MenuState.SelectLevelSelectPlayerOne;
							m_MenuState = MenuState.SelectCheckPoint;
						}
	/*					//Load Game Button
						buttonText = "Level Two";
						Rect LoadButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 6, Screen.width / 2, Screen.height / 6);
						if (GUI.Button(LoadButtonPosition, buttonText))
						{
							m_CurrentLevel = Level.LevelOnePartTwo;
					m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						}
						
						buttonText = "Level Three";
						Rect SelectButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height /6* 2, Screen.width / 2, Screen.height / 6);
						if (GUI.Button(SelectButtonPosition, buttonText))
						{
							m_CurrentLevel = Level.LevelOnePartThree;
					m_MenuState = MenuState.SelectLevelSelectPlayerOne;
							
						}
						
						//Options button
						Rect OptionButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 6*3, Screen.width / 2, Screen.height / 6);
						buttonText = "Level Four";
						if (GUI.Button(OptionButtonPosition, buttonText))
						{
							m_CurrentLevel = Level.LevelOnePartFour;
							m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						}
						
						//Exit Game Button
						Rect FiveButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 6 * 4), Screen.width / 2, Screen.height / 6);
						buttonText = "Level Five";
						if (GUI.Button(FiveButtonPosition, buttonText))
						{
							m_CurrentLevel = Level.LevelOnePartFive;
							m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						}
								
	*/				
						
						//Exit Game Button
						Rect ExitButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 6 * 5), Screen.width / 2, Screen.height / 6);
						buttonText = "Back";
						if (GUI.Button(ExitButtonPosition, buttonText))
						{
							m_MenuState = MenuState.MainMenu;
						}

						break;



					}

			case MenuState.SelectCheckPoint:
			{


					//New Game Button
					Rect NewGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 6);
					buttonText = "Start Point";
					if (GUI.Button(NewGamebuttonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartOne;
						//	m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					//Load Game Button
					buttonText = "CheckPoint One";

					Rect CheckAButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 6, Screen.width / 2, Screen.height / 6);
					if (GUI.Button(CheckAButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartTwo;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					
					buttonText = "CheckPoint Two";
					Rect CheckBButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height /6* 2, Screen.width / 2, Screen.height / 6);
					if (GUI.Button(CheckBButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartThree;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						
					}
					
					//Options button
					Rect CheckCButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 6*3, Screen.width / 2, Screen.height / 6);
					buttonText = "CheckPoint Three";
					if (GUI.Button(CheckCButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartFour;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					
					//Exit Game Button
					Rect CheckDButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 6 * 4), Screen.width / 2, Screen.height / 6);
					buttonText = "CheckPoint Four";
					if (GUI.Button(CheckDButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartFive;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					

					buttonText = "CheckPoint Five";
					Rect CheckEButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height /6* 2, Screen.width / 2, Screen.height / 6);
				if (GUI.Button(CheckEButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartSix;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						
					}
					
					//Options button
					Rect CheckFButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 6*3, Screen.width / 2, Screen.height / 6);
					buttonText = "CheckPoint Six";
					if (GUI.Button(CheckFButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartSeven;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					
					//Exit Game Button
					Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 6 * 4), Screen.width / 2, Screen.height / 6);
					buttonText = "Back";
					if (GUI.Button(BackButtonPosition, buttonText))
					{
						m_MenuState = MenuState.SelectLevel;
					}

				break;

			}

			case MenuState.SelectLevelSelectPlayerOne:
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
					m_MenuState = MenuState.SelectLevelSelectPlayerTwo;
				}
				
				//Back
				Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
				buttonText = "Back";
				if (GUI.Button(BackButtonPosition, buttonText))
				{
					m_MenuState = MenuState.SelectLevel;
				}
				
				break;
				
			}



			case MenuState.SelectLevelSelectPlayerTwo:
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
					//firstTimePlayerTwoSelect = true;
					Screen.showCursor = false;
					setPlayer();
					
				}
				
				//Back
				Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 5 * 4), Screen.width / 2, Screen.height / 5);
				buttonText = "Back";
				if (GUI.Button(BackButtonPosition, buttonText))
				{
					m_MenuState = MenuState.SelectLevelSelectPlayerTwo;
					firstTimePlayerTwoSelect = true;
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
                    		Screen.showCursor = false;
							m_CurrentLevel = Level.LevelOnePartOne;
							PlayerPrefs.SetString("CurrentCheckpoint", "StartPoint");
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

		PlayerPrefs.SetString("CurrentCheckpoint", PlayerPrefs.GetString("Checkpoint" + slot));
		PlayerPrefs.SetString ("CurrentLevelStage", PlayerPrefs.GetString ("LevelStage" + slot));


        if (m_PlayerOneSelected != null)
        {
            setPlayer();
        }
     }








        void setPlayer()
    {
		PlayerPrefs.SetInt("CurrentPlayerOne", (int)m_PlayerOne);
		PlayerPrefs.SetInt("CurrentPlayerTwo", (int)m_PlayerTwo);
		PlayerPrefs.SetInt("CurrentLevel", (int)m_CurrentLevel);
		Application.LoadLevel("Level1-" + ((int)m_CurrentLevel + 1));

	}


}
