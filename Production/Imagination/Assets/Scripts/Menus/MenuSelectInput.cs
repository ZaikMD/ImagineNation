/*
*MenuSelectInput
*
*resposible for setting what player is using what input
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class MenuSelectInput : Menu
{
	//the classes that hold the selection data
    PlayerInputSelection m_PlayerOne;
    PlayerInputSelection m_PlayerTwo;

	//the selection point (used to display inoput slection type
    public GameObject PlayerOneSelection;
    public GameObject PlayerTwoSelection;

	//models for key board and gamepad
    public GameObject GamePadPrefab;
    public GameObject KeyboardPrefab;

	//the textures to show the player is ready
    public GUITexture ReadyTexturePlayerOne;
    public GUITexture ReadyTexturePlayerTwo;

	//the next menu
    public Menu NextMenu;

	//enum for a function later
    enum UpdateType
    {
        Selection,
        ReadyState
    }

    protected override void start()
	{
		//disable the textures
        ReadyTexturePlayerOne.enabled = false;
        ReadyTexturePlayerTwo.enabled = false;

		//create the input selection objects
        m_PlayerOne = new PlayerInputSelection(PlayerOneSelection, ReadyTexturePlayerOne);
        m_PlayerTwo = new PlayerInputSelection(PlayerTwoSelection, ReadyTexturePlayerTwo);        
	}
	
	// Update is called once per frame
	public override void update () 
	{
		//if both players are ready go to the next menu
        if (m_PlayerOne.IsReady && m_PlayerTwo.IsReady)
        {
            if (NextMenu != null)
            {
				//set the gameData values
                GameData.Instance.m_PlayerOneInput = m_PlayerOne.CurrentSelection;
                GameData.Instance.m_PlayerTwoInput = m_PlayerTwo.CurrentSelection;

				//reset the inputSelections
                m_PlayerOne.reset();
                m_PlayerTwo.reset();

                MenuManager.Instance.changeMenu(NextMenu);
            }
            else
            {
				#if DEBUG || UNITY_EDITOR
                	Debug.LogError("Adam you set this up wrong");
				#endif
            }
        }
        else
        {
			//set the selection data for both players
            m_PlayerOne = setInputSelection(m_PlayerOne, m_PlayerTwo);
            m_PlayerTwo = setInputSelection(m_PlayerTwo, m_PlayerOne);
        }
	}

	//used to show the players the selection data
    void showPlayerSelection(PlayerInputSelection player, UpdateType updateType)
    {
		//are we updating ready state or input selection
        switch (updateType)
        {
            case UpdateType.Selection:
                {
					//is the point on the menu to show the players their selection is not null
                    if (player.m_SelectionMountPoint != null)
                    {
						//delete any children the mount point has
                        foreach(Transform child in player.m_SelectionMountPoint.transform)
                        {
                            GameObject.Destroy(child.gameObject);
                        }
						
						//create a key board or mouse at the location of the mount point and set its parent to the mount point
                        switch (player.CurrentSelection)
                        {
                            case PlayerInput.Keyboard:
                                {
                                    GameObject newObj = (GameObject)GameObject.Instantiate(KeyboardPrefab, player.m_SelectionMountPoint.transform.position, player.m_SelectionMountPoint.transform.rotation);
                                    newObj.transform.parent = player.m_SelectionMountPoint.transform;
                                    break;
                                }
                            case PlayerInput.GamePadOne:
                                {
                                    GameObject newObj = (GameObject)GameObject.Instantiate(GamePadPrefab, player.m_SelectionMountPoint.transform.position, player.m_SelectionMountPoint.transform.rotation);
                                    newObj.transform.parent = player.m_SelectionMountPoint.transform;
                                    break;
                                }

                            case PlayerInput.GamePadTwo:
                                {
                                    GameObject newObj = (GameObject)GameObject.Instantiate(GamePadPrefab, player.m_SelectionMountPoint.transform.position, player.m_SelectionMountPoint.transform.rotation);
                                    newObj.transform.parent = player.m_SelectionMountPoint.transform;
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
                break;
            case UpdateType.ReadyState:
                {
                    if (player.IsReady)
                    {
						//show the ready texture
                        player.m_ReadyTexure.enabled = true;
						
						//mopve the texture to be over top the selection
                        Vector3 screenPoint = Camera.main.WorldToScreenPoint(player.m_SelectionMountPoint.transform.position);

                        player.m_ReadyTexure.transform.position = new Vector3(screenPoint.x / Screen.width, screenPoint.y / Screen.height, 0.0f);
                    }
                    else
                    {
						//player is not ready so hide the texture
                        player.m_ReadyTexure.enabled = false;
                    }
                }
                break;
        }
    }

    PlayerInputSelection setInputSelection(PlayerInputSelection playerToSet, PlayerInputSelection playerToNotSet)
    {
        if (playerToSet.IsReady)
        {
			//if the player hit back set the variable and update what to show the player
            if (InputManager.getShowHudDown(playerToSet.CurrentSelection))
            {
                playerToSet.IsReady = false;
                showPlayerSelection(playerToSet, UpdateType.ReadyState);
            }
        }
        else
        {
            if (playerToSet.isSet())
            {
				// if the player has their input set see if they tried to set themselves as ready
                if (InputManager.getJumpDown(playerToSet.CurrentSelection))
                {
                    playerToSet.IsReady = true;
                    showPlayerSelection(playerToSet, UpdateType.ReadyState);
                }
                else
                {
					//see if the player wants to unset their input selection
                    if (InputManager.getShowHudDown(playerToSet.CurrentSelection))
                    {
                        playerToSet.CurrentSelection = PlayerInput.None;
                        showPlayerSelection(playerToSet, UpdateType.Selection);
                    }
                }
            }
            else
            {
				//see if any valid inputs were pressed
                playerToSet = getInput(playerToSet, playerToNotSet);
                if (playerToSet.CurrentSelection != PlayerInput.None)
                {
					//show the players new input selection
                    showPlayerSelection(playerToSet, UpdateType.Selection);
                }
            }
        }
        return playerToSet;
    }

    PlayerInputSelection getInput(PlayerInputSelection playerToSet, PlayerInputSelection playerToNotSet)
    {
		//depeng on what the other player has their input set to we need to check different inputs (so we dont double up on who has what input)
		//if there is a valid input from a valid source set the players input selection
        switch(playerToNotSet.CurrentSelection)
        {
            case PlayerInput.Keyboard:
                if (InputManager.getPauseDown(PlayerInput.GamePadOne))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadOne;
                }
                else if (InputManager.getPauseDown(PlayerInput.GamePadTwo))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadTwo;
                }
                break;

            case PlayerInput.GamePadOne:
                if (InputManager.getPauseDown(PlayerInput.Keyboard))
                {
                    playerToSet.CurrentSelection = PlayerInput.Keyboard;
                }
                else if (InputManager.getPauseDown(PlayerInput.GamePadTwo))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadTwo;
                }
                break;

            case PlayerInput.GamePadTwo:
                if (InputManager.getPauseDown(PlayerInput.Keyboard))
                {
                    playerToSet.CurrentSelection = PlayerInput.Keyboard;
                }
                else if (InputManager.getPauseDown(PlayerInput.GamePadOne))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadOne;
                }
                break;

            default:
            if (InputManager.getPauseDown(PlayerInput.Keyboard))
            {
                playerToSet.CurrentSelection = PlayerInput.Keyboard;
            }
            else if (InputManager.getPauseDown(PlayerInput.GamePadOne))
            {
                playerToSet.CurrentSelection = PlayerInput.GamePadOne;
            }
            else if (InputManager.getPauseDown(PlayerInput.GamePadTwo))
            {
                playerToSet.CurrentSelection = PlayerInput.GamePadTwo;
            }
            break;
        }
        return playerToSet;

    }

	//used to group variables for both players
    protected class PlayerInputSelection
    {
		//currently selected input
        PlayerInput m_CurrentSelection = PlayerInput.None;
		//the mount point for showing their selection tpye
        public GameObject m_SelectionMountPoint;
		//the texture to show that theyre ready
        public GUITexture m_ReadyTexure;

        public PlayerInputSelection(GameObject selectionMountPoint, GUITexture readyTexture)
        {
            m_SelectionMountPoint = selectionMountPoint;
            m_ReadyTexure = readyTexture;
        }

		//resets all the variables
        public void reset()
        {
            m_ReadyTexure.enabled = false;
            m_CurrentSelection = PlayerInput.None;
            m_IsReady = false;

            foreach (Transform child in m_SelectionMountPoint.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public PlayerInput CurrentSelection
        {
            get { return m_CurrentSelection; }
            set { m_CurrentSelection = value; }
        }

        bool m_IsReady = false;
        public bool IsReady
        {
            get { return m_IsReady; }
            set { m_IsReady = value; }
        }

        public bool isSet()
        {
            return m_CurrentSelection != PlayerInput.None;
        }
    }
}
