﻿using UnityEngine;
using System.Collections;



public enum Level
	{
		LevelOneStart,
		LevelOnePartTwo,
		LevelOnePartThree,
		LevelOnePartFour,
		LevelOnePartFive,
		LevelOnePartSix,
		LevelOnePartSeven
	}

public class Menu : MonoBehaviour {
/*
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
        Credit  
    }
*/
    //setting variables
    Resolution m_Resolution = Resolution.Res720p;
    Quality m_QualityText = Quality.High;
    QualityLevel m_Quality = QualityLevel.Good;
    Refresh m_Refresh = Refresh.Refresh60;
    bool m_FullScreen = true;
    float m_Volume;
    int m_WidthRes = Screen.width;
    int m_HeightRes = Screen.height;
    int m_RefreshRate = 60;





	public Level m_CurrentLevel;
    public MenuState m_MenuState;
    public bool m_MainMenu = true;
    public Player m_PlayerOne;
    public Player m_PlayerTwo;
    string m_PlayerOneSelected = null;
    string m_PlayerTwoSelected = null;
    Stage m_StartingStage;

	bool firstTimePlayerTwoSelect;

    /// <summary>
    /// menu is mainly done in on gui as all the buttons and text require they be inside the function.
    /// helper functions are at the bottom.
    /// </summary>
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
						//	m_CurrentLevel = Level.LevelOneStart;
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
//TODO add stage change in menu.
			case MenuState.SelectCheckPoint:
			{


					//New Game Button
					Rect NewGamebuttonPosition = new Rect(Screen.width / 2 - Screen.width / 4, 0, Screen.width / 2, Screen.height / 8);
					buttonText = "Start Point";
					if (GUI.Button(NewGamebuttonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOneStart;
						//	m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					//Load Game Button
					buttonText = "CheckPoint One";

					Rect CheckAButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 8, Screen.width / 2, Screen.height / 8);
					if (GUI.Button(CheckAButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartTwo;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					
					buttonText = "CheckPoint Two";
					Rect CheckBButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height /8* 2, Screen.width / 2, Screen.height / 8);
					if (GUI.Button(CheckBButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartThree;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						
					}
					
					//Options button
                    Rect CheckCButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 8 * 3, Screen.width / 2, Screen.height / 8);
					buttonText = "CheckPoint Three";
					if (GUI.Button(CheckCButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartFour;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					
					//Exit Game Button
					Rect CheckDButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 8 * 4), Screen.width / 2, Screen.height / 8);
					buttonText = "CheckPoint Four";
					if (GUI.Button(CheckDButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartFive;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					

					buttonText = "CheckPoint Five";
					Rect CheckEButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 8 * 5, Screen.width / 2, Screen.height / 8);
				if (GUI.Button(CheckEButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartSix;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
						
					}
					
					//Options button
					Rect CheckFButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 8 * 6, Screen.width / 2, Screen.height / 8);
					buttonText = "CheckPoint Six";
					if (GUI.Button(CheckFButtonPosition, buttonText))
					{
						m_CurrentLevel = Level.LevelOnePartSeven;
						m_MenuState = MenuState.SelectLevelSelectPlayerOne;
					}
					
					//Exit Game Button
					Rect BackButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 8 * 7), Screen.width / 2, Screen.height / 8);
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
					m_MenuState = MenuState.SelectCheckPoint;
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
							m_CurrentLevel = Level.LevelOneStart;
                            m_StartingStage = Stage.StartStage;
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
                        Rect VolumeSliderPosition = new Rect(Screen.width / 3, Screen.height / 8, Screen.width / 3, Screen.height / 6);
                        m_Volume = GUI.HorizontalSlider(VolumeSliderPosition, m_Volume, 0, 100);

                        Rect VolumePosition = new Rect(Screen.width / 3, 0, Screen.width / 3, Screen.height / 6);
                        buttonText = "Volume";
                        GUI.TextArea(VolumePosition, buttonText);

                        Rect WindowButtonPosition = new Rect(0, 0, Screen.width / 3, Screen.height / 6);
                        buttonText = "Window: " + m_FullScreen;
                        if (GUI.Button(WindowButtonPosition, buttonText))
                        {
                            m_FullScreen = !m_FullScreen;
                        }

                        Rect ControlButtonPosition = new Rect(Screen.width / 3 * 2, 0, Screen.width / 3, Screen.height / 6);
                        buttonText = "Controls";
                        if (GUI.Button(ControlButtonPosition, buttonText))
                        {
                            m_MenuState = MenuState.Controls;
                        }


                        //TODO: Add Options
                        Rect RaiseResolutionButtonPosition = new Rect(Screen.width / 3 * 2, (Screen.height / 6 * 2), Screen.width / 3, Screen.height / 6);
                        buttonText = "Raise";
                        if (GUI.Button(RaiseResolutionButtonPosition, buttonText))
                        {
                            if (m_Resolution > 0)
                            {
                                m_Resolution -= 1;
                            }

                            switch (m_Resolution)
                            {
                                case Resolution.Res2160p:
                                    {
                                        m_WidthRes = 3840;
                                        m_HeightRes = 2160;
                                        break;
                                    }

                                case Resolution.Res1440p:
                                    {
                                        m_WidthRes = 2560;
                                        m_HeightRes = 1440;
                                        break;
                                    }

                                case Resolution.Res1080p:
                                    {
                                        m_WidthRes = 1920;
                                        m_HeightRes = 1080;
                                        break;
                                    }

                                case Resolution.Res720p:
                                    {
                                        m_WidthRes = 1280;
                                        m_HeightRes = 720;
                                        break;
                                    }

                                case Resolution.Res768p:
                                    {
                                        m_WidthRes = 1024;
                                        m_HeightRes = 768;
                                        break;
                                    }

                                case Resolution.Res480p:
                                    {
                                        m_WidthRes = 852;
                                        m_HeightRes = 480;
                                        break;
                                    }

                                case Resolution.Res360p:
                                    {
                                        m_WidthRes = 640;
                                        m_HeightRes = 360;
                                        break;
                                    }


                            }



                        }

                        Rect LowerResolutionButtonPosition = new Rect(0, (Screen.height / 6 * 2), Screen.width / 3, Screen.height / 6);
                        buttonText = "Lower";
                        if (GUI.Button(LowerResolutionButtonPosition, buttonText))
                        {
                            if (m_Resolution < Resolution.Res360p)
                            {
                                m_Resolution += 1;
                            }


                            switch (m_Resolution)
                            {
                                case Resolution.Res2160p:
                                    {
                                        m_WidthRes = 3840;
                                        m_HeightRes = 2160;
                                        break;
                                    }

                                case Resolution.Res1440p:
                                    {
                                        m_WidthRes = 2560;
                                        m_HeightRes = 1440;
                                        break;
                                    }

                                case Resolution.Res1080p:
                                    {
                                        m_WidthRes = 1920;
                                        m_HeightRes = 1080;
                                        break;
                                    }

                                case Resolution.Res720p:
                                    {
                                        m_WidthRes = 1280;
                                        m_HeightRes = 720;
                                        break;
                                    }

                                case Resolution.Res768p:
                                    {
                                        m_WidthRes = 1024;
                                        m_HeightRes = 768;
                                        break;
                                    }

                                case Resolution.Res480p:
                                    {
                                        m_WidthRes = 852;
                                        m_HeightRes = 480;
                                        break;
                                    }

                                case Resolution.Res360p:
                                    {
                                        m_WidthRes = 640;
                                        m_HeightRes = 360;
                                        break;
                                    }


                            }


                        }

                        Rect ResolutionPosition = new Rect(Screen.width / 3, (Screen.height / 6 * 2), Screen.width / 3, Screen.height / 6);
                        buttonText = "Resolustion: \n\n" + m_WidthRes + " * " + m_HeightRes;
                        GUI.TextArea(ResolutionPosition, buttonText);


                        Rect RaiseRefreshRateButtonPosition = new Rect(Screen.width / 3 * 2, (Screen.height / 6), Screen.width / 3, Screen.height / 6);
                        buttonText = "Raise Refresh Rate";
                        if (GUI.Button(RaiseRefreshRateButtonPosition, buttonText))
                        {
                            if (m_Refresh > 0)
                            {
                                m_Refresh -= 1;
                            }

                            switch (m_Refresh)
                            {
                                case Refresh.Refresh120:
                                    {
                                        m_RefreshRate = 120;
                                        break;
                                    }

                                case Refresh.Refresh90:
                                    {
                                        m_RefreshRate = 90;
                                        break;
                                    }

                                case Refresh.Refresh60:
                                    {
                                        m_RefreshRate = 60;
                                        break;
                                    }

                                case Refresh.Refresh45:
                                    {
                                        m_RefreshRate = 45;
                                        break;
                                    }

                                case Refresh.Refresh30:
                                    {
                                        m_RefreshRate = 30;
                                        break;
                                    }


                            }

                        }


                        Rect LowerRefreshRateButtonPosition = new Rect(0, (Screen.height / 6), Screen.width / 3, Screen.height / 6);
                        buttonText = "Lower Refresh Rate";
                        if (GUI.Button(LowerRefreshRateButtonPosition, buttonText))
                        {
                            if (m_Refresh < Refresh.Refresh30)
                            {
                                m_Refresh += 1;
                            }

                            switch (m_Refresh)
                            {
                                case Refresh.Refresh120:
                                    {
                                        m_RefreshRate = 120;
                                        break;
                                    }

                                case Refresh.Refresh90:
                                    {
                                        m_RefreshRate = 90;
                                        break;
                                    }

                                case Refresh.Refresh60:
                                    {
                                        m_RefreshRate = 60;
                                        break;
                                    }

                                case Refresh.Refresh45:
                                    {
                                        m_RefreshRate = 45;
                                        break;
                                    }

                                case Refresh.Refresh30:
                                    {
                                        m_RefreshRate = 30;
                                        break;
                                    }


                            }

                        }


                        Rect RefreshPosition = new Rect(Screen.width / 3, (Screen.height / 6), Screen.width / 3, Screen.height / 6);
                        buttonText = "Refresh: \n\n" + m_RefreshRate;
                        GUI.TextArea(RefreshPosition, buttonText);


                        Rect RaiseQualityButtonPosition = new Rect(Screen.width / 3 * 2, (Screen.height / 6 * 3), Screen.width / 3, Screen.height / 6);
                        buttonText = "Raise";
                        if (GUI.Button(RaiseQualityButtonPosition, buttonText))
                        {
                            if (m_QualityText > 0)
                            {
                                m_QualityText -= 1;
                            }

                            switch (m_QualityText)
                            {
                                case Quality.Ultra:
                                    {
                                        m_Quality = QualityLevel.Fantastic;
                                        break;
                                    }

                                case Quality.High:
                                    {
                                        m_Quality = QualityLevel.Beautiful;
                                        break;
                                    }

                                case Quality.Medium:
                                    {
                                        m_Quality = QualityLevel.Good;
                                        break;
                                    }

                                case Quality.Low:
                                    {
                                        m_Quality = QualityLevel.Simple;
                                        break;
                                    }

                                case Quality.ImUsingACalculator:
                                    {
                                        m_Quality = QualityLevel.Fastest;
                                        break;
                                    }


                            }



                        }

                        Rect LowerQualityButtonPosition = new Rect(0, (Screen.height / 6 * 3), Screen.width / 3, Screen.height / 6);
                        buttonText = "Lower";
                        if (GUI.Button(LowerQualityButtonPosition, buttonText))
                        {
                            if (m_QualityText < Quality.ImUsingACalculator)
                            {
                                m_QualityText += 1;
                            }

                            switch (m_QualityText)
                            {
                                case Quality.Ultra:
                                    {
                                        m_Quality = QualityLevel.Fantastic;
                                        break;
                                    }

                                case Quality.High:
                                    {
                                        m_Quality = QualityLevel.Beautiful;
                                        break;
                                    }

                                case Quality.Medium:
                                    {
                                        m_Quality = QualityLevel.Good;
                                        break;
                                    }

                                case Quality.Low:
                                    {
                                        m_Quality = QualityLevel.Simple;
                                        break;
                                    }

                                case Quality.ImUsingACalculator:
                                    {
                                        m_Quality = QualityLevel.Fastest;
                                        break;
                                    }


                            }

                        }

                        Rect QualityPosition = new Rect(Screen.width / 3, (Screen.height / 6 * 3), Screen.width / 3, Screen.height / 6);
                        buttonText = "Quality: \n\n" + m_QualityText;
                        GUI.TextArea(QualityPosition, buttonText);


                        Rect ApplyButtonPosition = new Rect(Screen.width / 2, (Screen.height / 6 * 5), Screen.width / 2, Screen.height / 6);
                        buttonText = "Apply Changes";
                        if (GUI.Button(ApplyButtonPosition, buttonText))
                        {
                            applyChanges();
                        }


                        Rect CreditsButtonPosition = new Rect(Screen.width / 2 - Screen.width / 4, (Screen.height / 6 * 4), Screen.width / 2, Screen.height / 6);
                        buttonText = "Credits";
                        if (GUI.Button(CreditsButtonPosition, buttonText))
                        {
                            m_MenuState = MenuState.Credit;
                        }
                        

                        Rect BackButtonPosition = new Rect(0, (Screen.height / 6 * 5), Screen.width / 2, Screen.height / 6);
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
                case MenuState.Controls:
                    {
                        Rect ControlTextPosition = new Rect(Screen.width / 3, 0, Screen.width / 3, Screen.height / 4 * 3);
                        buttonText = "Movement:             WASD              -           Left Stick \nJump:                    Space               -           A\nCharacter Switch:   Tab                   -           Y\nInteract:                 F                      -           B\nCamera Control:    Hold Right Click  -           Right Stick\nUse Item / Attack:  Left Click           -           X\nAim:                      Shift                  -           Left Bumper\nPause:                   Escape             -           Start\n";
                        GUI.TextArea(ControlTextPosition, buttonText);

                        Rect BackButtonPosition = new Rect(Screen.width / 3, (Screen.height / 4 * 3), Screen.width / 3, Screen.height / 4);
                        buttonText = "Back";
                        if (GUI.Button(BackButtonPosition, buttonText))
                        {
                            m_MenuState = MenuState.Options;
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

		m_CurrentLevel =  (Level)PlayerPrefs.GetInt("Level" + slot);

		m_StartingStage = (Stage)PlayerPrefs.GetInt("LevelStage" + slot);

///		PlayerPrefs.SetString("CurrentCheckpoint", PlayerPrefs.GetString("Checkpoint" + slot));
//		PlayerPrefs.SetString ("CurrentLevelStage", PlayerPrefs.GetString ("LevelStage" + slot));


        if (m_PlayerOneSelected != null)
        {
            setPlayer();
        }
     }


        void applyChanges()
        {
            Screen.SetResolution(m_WidthRes, m_HeightRes, m_FullScreen, m_RefreshRate);
            QualitySettings.SetQualityLevel((int)m_Quality);

            SoundManager.Instance.m_Volume = m_Volume;
            //TODO access sound manager to change volume to setting.
        }





        void setPlayer()
    {
		PlayerPrefs.SetInt("CurrentPlayerOne", (int)m_PlayerOne);
		PlayerPrefs.SetInt("CurrentPlayerTwo", (int)m_PlayerTwo);
		PlayerPrefs.SetInt("CurrentLevel", (int)m_CurrentLevel);
        PlayerPrefs.SetInt("CurrentLevelStage", (int)m_StartingStage);
		Application.LoadLevel("Level1-2");

	}


}
