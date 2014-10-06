using UnityEngine;
using System.Collections;

public class MenuSelectInput : Menu
{
    PlayerInputSelection m_PlayerOne;
    PlayerInputSelection m_PlayerTwo;

    public GameObject PlayerOneSelection;
    public GameObject PlayerTwoSelection;

    public GameObject GamePadPrefab;
    public GameObject KeyboardPrefab;

    public GUITexture ReadyTexturePlayerOne;
    public GUITexture ReadyTexturePlayerTwo;

    public Menu NextMenu;

    enum UpdateType
    {
        Selection,
        readyState
    }

    protected override void start()
	{
        ReadyTexturePlayerOne.enabled = false;
        ReadyTexturePlayerTwo.enabled = false;


        m_PlayerOne = new PlayerInputSelection(PlayerOneSelection, ReadyTexturePlayerOne);
        m_PlayerTwo = new PlayerInputSelection(PlayerTwoSelection, ReadyTexturePlayerTwo);        
	}
	
	// Update is called once per frame
	public override void update () 
	{
        if (m_PlayerOne.IsReady && m_PlayerTwo.IsReady)
        {
            if (NextMenu != null)
            {
                GameData.Instance.m_PlayerOneInput = m_PlayerOne.CurrentSelection;
                GameData.Instance.m_PlayerTwoInput = m_PlayerTwo.CurrentSelection;

                m_PlayerOne.reset();
                m_PlayerTwo.reset();

                MenuManager.Instance.changeMenu(NextMenu);
            }
            else
            {
                Debug.LogError("Adam you set this up wrong");
            }
        }
        else
        {
            m_PlayerOne = setInputSelection(m_PlayerOne, m_PlayerTwo);
            m_PlayerTwo = setInputSelection(m_PlayerTwo, m_PlayerOne);
        }
	}

    void showPlayerSelection(PlayerInputSelection player, UpdateType updateType)
    {
        switch (updateType)
        {
            case UpdateType.Selection:
                {
                    if (player.m_SelectionMountPoint != null)
                    {
                        foreach(Transform child in player.m_SelectionMountPoint.transform)
                        {
                            GameObject.Destroy(child.gameObject);
                        }

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
            case UpdateType.readyState:
                {
                    if (player.IsReady)
                    {
                        player.m_ReadyTexure.enabled = true;

                        Vector3 screenPoint = Camera.main.WorldToScreenPoint(player.m_SelectionMountPoint.transform.position);

                        player.m_ReadyTexure.transform.position = new Vector3(screenPoint.x / Screen.width, screenPoint.y / Screen.height, 0.0f);
                    }
                    else
                    {
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
            if (InputManager.getShowHudDown(playerToSet.CurrentSelection))
            {
                playerToSet.IsReady = false;
                showPlayerSelection(playerToSet, UpdateType.readyState);
            }
        }
        else
        {
            if (playerToSet.isSet())
            {
                if (InputManager.getJumpDown(playerToSet.CurrentSelection))
                {
                    playerToSet.IsReady = true;
                    showPlayerSelection(playerToSet, UpdateType.readyState);
                }
                else
                {
                    if (InputManager.getShowHudDown(playerToSet.CurrentSelection))
                    {
                        playerToSet.CurrentSelection = PlayerInput.None;
                        showPlayerSelection(playerToSet, UpdateType.Selection);
                    }
                }
            }
            else
            {
                playerToSet = getInput(playerToSet, playerToNotSet);
                if (playerToSet.CurrentSelection != PlayerInput.None)
                {
                    showPlayerSelection(playerToSet, UpdateType.Selection);
                }
            }
        }
        return playerToSet;
    }

    PlayerInputSelection getInput(PlayerInputSelection playerToSet, PlayerInputSelection playerToNotSet)
    {
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

    protected class PlayerInputSelection
    {
        PlayerInput m_CurrentSelection = PlayerInput.None;
        public GameObject m_SelectionMountPoint;
        public GUITexture m_ReadyTexure;

        public PlayerInputSelection(GameObject selectionMountPoint, GUITexture readyTexture)
        {
            m_SelectionMountPoint = selectionMountPoint;
            m_ReadyTexure = readyTexture;
        }

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
